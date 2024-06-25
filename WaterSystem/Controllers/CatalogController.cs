
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAyCC.Bussiness.Common;
using SAyCC.Bussiness.DrinkingWaterSystem;
using SAyCC.Bussiness.Login;
using SAyCC.Bussiness.SystemAdmin;
using SAyCC.Entities.Common;
using SAyCC.Entities.Login;
using SAyCC.Entities.SystemAdmin;
using SAyCC.Entities.WaterSystem;
using SAyCC.WaterSystem.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WaterSystem.Controllers
{
    public class CatalogController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32(Sessions.IdUser) != null)
            {
                #region Configura Menú
                ViewBag.IdUser = HttpContext.Session.GetInt32(Sessions.IdUser);
                ViewBag.Modules = GetModulesAllowed(HttpContext.Session.GetInt32(Sessions.IdUser), HttpContext.Session.GetInt32(Sessions.IdApp));
                //ViewBag.Modules = GetModulesAllowed(HttpContext.Session.GetInt32(Sessions.RolUser), HttpContext.Session.GetInt32(Sessions.IdApp));
                string imgUp = HttpContext.Session.GetString(Sessions.ImagenUpload);
                ViewBag.ImagenBytesIlustrative = string.IsNullOrEmpty(imgUp) ? null : Convert.FromBase64String(imgUp);
                ViewBag.Alta = ViewBag.Modificacion = true;
                ViewBag.NombreUsuario = HttpContext.Session.GetString(Sessions.UserName);
                #endregion

                ViewBag.resultado = TempData["resultado"];
                ViewBag.MensajeErr = TempData["MensajeErr"];

                ViewBag.Applications = GetApplications();
                ViewBag.Catalogs = GetCatalog((int)Enumerador.CatalogTypeWaterSystem.TipoLectura);
                return View();
            }
            else
            {
                return Redirect(DBSet.urlRedirect);
            }
        }

        public void validateSession()
        {
            if (HttpContext.Session.GetInt32(Sessions.IdUser) != null)
            {
                int IdUser = (int)HttpContext.Session.GetInt32(Sessions.IdUser);
                int IdApp = (int)HttpContext.Session.GetInt32(Sessions.IdApp);
                int RolUser = (int)HttpContext.Session.GetInt32(Sessions.RolUser);
                HttpContext.Session.SetInt32(Sessions.IdUser, (int)IdUser);
                HttpContext.Session.SetInt32(Sessions.IdApp, (int)IdApp);
                HttpContext.Session.SetInt32(Sessions.RolUser, (int)RolUser);
            }
            else
            {
                Redirect(DBSet.urlRedirect);
            }
        }

        public CatalogEntityWS GetCatalog(int IDCatalog) {
            using (WaterSystemBusiness AppNegocio = new WaterSystemBusiness())
            {
                var resultado = AppNegocio.GetCatalog(IDCatalog);
                return resultado;
            }
        }
        public List<ModuleEntity> GetModulesAllowed(int? IdUsuario, int? IdApp)
        {
            using (ApplicationBusiness AppNegocio = new ApplicationBusiness())
            {
                var resultado = AppNegocio.GetModulesAllowed(IdUsuario, IdApp);
                return resultado;
            }
        }

        public List<ApplicationEntity> GetApplications()
        {
            using (LoginBusiness AppNegocio = new LoginBusiness())
            {
                var resultado = AppNegocio.getApplications(new int?());
                return resultado;
            }
        }

        public JsonResult saveProfile(int IdProfile, int IdApp, string Descripcion)
        {
            validateSession();
            using (ApplicationBusiness nego = new ApplicationBusiness())
            {
                List<ProfileEntity> resultado;
                try
                {
                    int IDUser = (int)HttpContext.Session.GetInt32(Sessions.IdUser);
                    int? IDUserMod = IdProfile != 0 ? IDUser : new int?();
                    nego.saveProfiles(IdProfile, IdApp, Descripcion, IdProfile == 0 ? IDUser : new int?(), IDUserMod);
                    resultado = nego.GetProfiles(IdApp: IdApp);
                }
                catch (Exception)
                {
                    resultado = new List<ProfileEntity>();
                    throw;
                }

                return Json(new { data = resultado });
            }
        }

        public JsonResult blockUnblockProfile(int IdProfile, int Status, int IdApp)
        {
            validateSession();
            using (ApplicationBusiness nego = new ApplicationBusiness())
            {
                List<ProfileEntity> resultado;
                try
                {
                    nego.changeStatusProfile(IdProfile, !Convert.ToBoolean(Status), (int)HttpContext.Session.GetInt32(Sessions.IdUser));
                    resultado = nego.GetProfiles(IdApp: IdApp);
                }
                catch (Exception)
                {
                    resultado = new List<ProfileEntity>();
                    throw;
                }

                return Json(new { data = resultado });
            }
        }

        public JsonResult DeleteProfile(int IdProfile, int IdApp)
        {
            validateSession();
            using (ApplicationBusiness nego = new ApplicationBusiness())
            {
                List<ProfileEntity> resultado;
                try
                {
                    int IDUser = (int)HttpContext.Session.GetInt32(Sessions.IdUser);
                    int? IDUserMod = IdProfile != 0 ? IDUser : new int?();
                    nego.DeleteProfile(IdProfile);
                    resultado = nego.GetProfiles(IdApp: IdApp);
                }
                catch (Exception ex)
                {
                    resultado = new List<ProfileEntity>() { new ProfileEntity() { Id=0, Detalle = ex.Message } };
                }

                return Json(new { data = resultado });
            }
        }

        public JsonResult getDetailProfile(int IdPerfil)
        {
            validateSession();
            using (ApplicationBusiness nego = new ApplicationBusiness())
            {
                var resultado = nego.GetProfiles(IdPerfil:IdPerfil).FirstOrDefault();
                var apps = nego.getAsignacionAplicacion(IdPerfil);
                resultado.AsignacionAplicacion = apps.Where(x => x.IdPerfil != 0).ToList();
                resultado.AppSinAcceso = apps.Where(x => x.IdPerfil == 0 && x.Activo).ToList();
                return Json(new { data = resultado });
            }
        }

        public JsonResult getModules(int IdApp)
        {
            validateSession();
            using (ApplicationBusiness nego = new ApplicationBusiness())
            {
                var resultado = nego.GetModulesAllowed(IdApp: IdApp);
                return Json(new { data = resultado });
            }
        }

        public JsonResult getProfiles(int IdApp)
        {
            validateSession();
            using (ApplicationBusiness nego = new ApplicationBusiness())
            {
                var resultado = nego.GetProfilesByAppActives(IdApp);
                return Json(new { data = resultado });
            }
        }

        public JsonResult getPermission(int IdPerfil, int IdApp)
        {
            validateSession();
            using (ApplicationBusiness nego = new ApplicationBusiness())
            {
                var resultado = nego.getPermission(IdPerfil, IdApp);
                return Json(new { data = resultado });
            }
        }

        public JsonResult DeleteCatalog(int IDCatalog, int Id)
        {
            validateSession();
            using (ApplicationBusiness nego = new ApplicationBusiness())
            {
                List<CatalogEntity> resultado;
                try
                {
                    int IDUser = (int)HttpContext.Session.GetInt32(Sessions.IdUser);
                    nego.DeleteCatalog(IDCatalog, Id);
                    resultado = nego.GetCatalog(IDCatalog);
                }
                catch (Exception Ex)
                {
                    resultado = new List<CatalogEntity>() { new CatalogEntity() { Id=0, Descripcion = Ex.Message } };
                return Json(new { data = resultado });
                }

                return Json(new { data = resultado });
            }
        }
        
        public IActionResult SaveCatalog(int IDCatalog, int Id, string Detalle, int IDAplicacion)
        {
            validateSession();
            using (ApplicationBusiness nego = new ApplicationBusiness())
            {
                List<CatalogEntity> resultado;
                try
                {
                    switch (IDCatalog)
                    {
                       case (int)Enumerador.CatalogType.Profile:
                            int? IdUserAlta = Id == 0 ? (int)HttpContext.Session.GetInt32(Sessions.IdUser) : new int?();
                            int? IdUserMod = Id > 0 ? (int)HttpContext.Session.GetInt32(Sessions.IdUser) : new int?();
                            nego.saveProfiles(Id, IDAplicacion, Detalle, IdUserAlta, IdUserMod);
                            break;
                        case (int)Enumerador.CatalogType.Apple:
                        case (int)Enumerador.CatalogType.Month:
                            nego.SaveCatalog(IDCatalog, Id, Detalle);
                            break;
                        default:
                            break;
                    }
                   
                    resultado = nego.GetCatalog(IDCatalog);
                    TempData["resultado"] = "Success";
                }
                catch (Exception ex)
                {
                    TempData["resultado"] = "Error";
                    TempData["MensajeErr"] = ex.Message;
                    resultado = new List<CatalogEntity>() { new CatalogEntity() { Id = 0, Descripcion = ex.Message } };
                }

                return RedirectToAction("Index");
            }
        }

        public JsonResult GetCatalogJson(int IDCatalog)
        {
            validateSession();
            using (WaterSystemBusiness nego = new WaterSystemBusiness())
            {
                CatalogEntityWS res;
                try
                {
                    res = nego.GetCatalog(IDCatalog);
                }
                catch (Exception)
                {
                    res = new CatalogEntityWS();
                    throw;
                }
                return Json(new { data = res }) ;
            }
        }

        public JsonResult getProfilesWithStats(int IdApp)
        {
            validateSession();
            using (ApplicationBusiness nego = new ApplicationBusiness())
            {
                var resultado = nego.GetProfiles(IdApp: IdApp);
                return Json(new { data = resultado });
            }
        }
        public JsonResult GetWaterMeterJson()
        {
            validateSession();
            using (WaterSystemBusiness nego = new WaterSystemBusiness())
            {
                List<WaterMeterEntity> res;
                try
                {
                    res = nego.GetWaterMeter();
                }
                catch (Exception)
                {
                    res = new List<WaterMeterEntity>();
                    throw;
                }
                return Json(new { data = res });
            }
        }

        public JsonResult ModifyWaterMeter(int IdWaterMeter, int ModifyTypeWaterMeter, string NoMedidor, decimal? LecturaAnterior, decimal? LecturaActual, int? IdManzana, int? IdTitular)
        {
            validateSession();
            List<WaterMeterEntity> res = new List<WaterMeterEntity>();
            try
            {
                using (ApplicationBusiness appNego = new ApplicationBusiness())
                {
                    switch (ModifyTypeWaterMeter)
                    {
                        case (int)Enumerador.ModifyTypeWaterMeter.Disassociate:
                        case (int)Enumerador.ModifyTypeWaterMeter.Unsubscribe:
                        case (int)Enumerador.ModifyTypeWaterMeter.Deactivate:
                        appNego.UpdateWaterMeterById(IdWaterMeter, ModifyTypeWaterMeter, new int?());
                            break;
                        case (int)Enumerador.ModifyTypeWaterMeter.Delete:
                            appNego.DeleteWaterMeterById(IdWaterMeter);
                            break;
                        case (int)Enumerador.ModifyTypeWaterMeter.Update:
                            WaterMeterEntity entity = new WaterMeterEntity()
                            {
                                Id = IdWaterMeter,
                                Numero = NoMedidor,
                                LecturaActual = (decimal)LecturaActual,
                                LecturaAnterior = (decimal)LecturaAnterior,
                                IdManzana = (int)IdManzana,
                            };
                            appNego.SaveWaterMeter(entity);
                            break;
                        case (int)Enumerador.ModifyTypeWaterMeter.associate:
                        appNego.UpdateWaterMeterById(IdWaterMeter, ModifyTypeWaterMeter, IdTitular);
                            break;
                        default:
                            break;

                    }
                }
                using (WaterSystemBusiness nego = new WaterSystemBusiness())
                {
                    res = nego.GetWaterMeter();
                }
            }
            catch (Exception)
            {
                res = new List<WaterMeterEntity>();
                throw;
            }
                return Json(new { data = res });
        }

        public JsonResult getUserByParameter(string name)
        {
            validateSession();
            using (ApplicationBusiness nego = new ApplicationBusiness())
            {
                var resultado = nego.getUserByParameter(name);

                return Json(new { data = resultado });
            }
        }










    }
}
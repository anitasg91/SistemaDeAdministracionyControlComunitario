
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
using SAyCC.SystemAdmin.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SystemAdmin.Controllers
{
    [SessionValidator]
    public class CatalogController : Controller
    {
        private readonly IGenerals _generals;
        public CatalogController(IGenerals generals)
        {
            _generals = generals;
        }
        public IActionResult Index(int? id)
        {
            #region Varibles de configuración para armar el Menú
              if (id != new int?())
              {
                  //Importante guardar la paginaActual cada que se navegue
                  HttpContext.Session.SetInt32(Sessions.CurrentPage, (int)id);
                  if (!_generals.AllPagesByAppList.Any(_ => _.Id == id)) {
                      return View(PartialViewEnum.PageNotAccess);
                  }
              }
            #endregion

            ViewBag.resultado = TempData["resultado"];
            ViewBag.MensajeErr = TempData["MensajeErr"];

            ViewBag.Applications = GetApplications();
            ViewBag.Catalogs = GetCatalog((int)Enumerador.CatalogType.Apple);
            return View();
        }
        
        public List<CatalogEntity> GetCatalog(int IDCatalog) {
            using (ApplicationBusiness AppNegocio = new ApplicationBusiness())
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
            using (ApplicationBusiness nego = new ApplicationBusiness())
            {
                var resultado = nego.GetModulesAllowed(IdApp: IdApp);
                return Json(new { data = resultado });
            }
        }

        public JsonResult getProfiles(int IdApp)
        {
            using (ApplicationBusiness nego = new ApplicationBusiness())
            {
                var resultado = nego.GetProfilesByAppActives(IdApp);
                return Json(new { data = resultado });
            }
        }

        public JsonResult getPermission(int IdPerfil, int IdApp)
        {
            using (ApplicationBusiness nego = new ApplicationBusiness())
            {
                var resultado = nego.getPermission(IdPerfil, IdApp);
                return Json(new { data = resultado });
            }
        }

        public JsonResult DeleteCatalog(int IDCatalog, int Id)
        {
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

        //public JsonResult SaveCatalog(int IDCatalog, int Id, string Detalle)
        //{
        //    validateSession();
        //    using (ApplicationBusiness nego = new ApplicationBusiness())
        //    {
        //        List<CatalogEntity> resultado;
        //        try
        //        {
        //            nego.SaveCatalog(IDCatalog, Id, Detalle);
        //            resultado = nego.GetCatalog(IDCatalog);
        //        }
        //        catch (Exception ex)
        //        {
        //            resultado = new List<CatalogEntity>() { new CatalogEntity() { Id=0, Descripcion = ex.Message } };
        //        }

        //        return Json(new { data = resultado });
        //    }
        //}
        public IActionResult SaveCatalog(int IDCatalog, int Id, string Detalle, int IDAplicacion)
        {
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
            using (ApplicationBusiness nego = new ApplicationBusiness())
            {
                List<CatalogEntity> res;
                try
                {
                    res = nego.GetCatalog(IDCatalog);
                }
                catch (Exception)
                {
                    res = new List<CatalogEntity>();
                    throw;
                }
                return Json(new { data = res }) ;
            }
        }

        /*public JsonResult getProfilesWithStats(int IdApp)
        {
            using (ApplicationBusiness nego = new ApplicationBusiness())
            {
                var resultado = nego.GetProfiles(IdApp: IdApp);
                return Json(new { data = resultado });
            }
        }*/
        public JsonResult GetWaterMeterJson()
        {
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
            using (ApplicationBusiness nego = new ApplicationBusiness())
            {
                var resultado = nego.getUserByParameter(name);

                return Json(new { data = resultado });
            }
        }

        public JsonResult GetPermissionCatalogJson(int? IdPermiso = new int?())
        {
            using (ApplicationBusiness nego = new ApplicationBusiness())
            {
                var resultado = nego.GetPermissionCatalog(IdPermiso);
                return Json(new { data = resultado });
            }
        }








    }
}
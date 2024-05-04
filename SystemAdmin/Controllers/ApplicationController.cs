using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAyCC.Bussiness.Common;
using SAyCC.Bussiness.Login;
using SAyCC.Bussiness.SystemAdmin;
using SAyCC.Entities.Common;
using SAyCC.Entities.Login;
using SAyCC.Entities.SystemAdmin;
using SAyCC.SystemAdmin.Utilities;

namespace SystemAdmin.Controllers
{
    public class ApplicationController : Controller
    {
        //public IActionResult Index(int? id)
        public IActionResult Index(int? id)
        {
            try
            {
                if (HttpContext.Session.GetInt32(Sessions.IdUser) != null)
                {
                    #region Configura Menú
                    if(id != new int?())
                    HttpContext.Session.SetInt32(Sessions.IdModule,(int)id);

                    ViewBag.IdUser = HttpContext.Session.GetInt32(Sessions.IdUser);
                    string imgUp = HttpContext.Session.GetString(Sessions.ImagenUpload);
                    ViewBag.ImagenBytesIlustrative = string.IsNullOrEmpty(imgUp) ? null : Convert.FromBase64String(imgUp);
                    var mods = GetModulesAllowed(HttpContext.Session.GetInt32(Sessions.RolUser), (int)HttpContext.Session.GetInt32(Sessions.IdApp));
                    ViewBag.Modules = mods;
                    int IDMod =(int) HttpContext.Session.GetInt32(Sessions.IdModule);
                    ViewBag.Alta = mods.FirstOrDefault(x => x.Id == IDMod).Alta;
                    ViewBag.Modificacion = mods.FirstOrDefault(x => x.Id == IDMod).Modificacion;
                    ViewBag.NombreUsuario = HttpContext.Session.GetString(Sessions.UserName);
                    #endregion

                    ViewBag.MessageError = HttpContext.Session.GetString(Sessions.MessageError);
                    ViewBag.Resultado = TempData["resultado"];
                    ViewBag.Applications = GetApplications();
                    return View();
                }
                else
                {
                    return Redirect(DBSet.urlRedirect);
                }
            }
            catch (Exception e)
            {
                ViewBag.MessageError = e.Message + " - " + e.Source;
                ViewBag.Resultado = "Error";
                return View();
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

        public List<ModuleEntity> GetModulesAllowed(int? IdPerfil, int? IdApp)
        {
            using (ApplicationBusiness AppNegocio = new ApplicationBusiness())
            {
                var resultado = AppNegocio.GetModulesAllowed(IdPerfil, IdApp);
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

        public JsonResult getDetailApplication(int IdApp)
        {
            validateSession();
            using (LoginBusiness nego = new LoginBusiness())
            {
                var resultado = nego.getApplications(IdApp).FirstOrDefault();
                resultado.Modules = GetModulesAllowed(new int?(), IdApp);
                resultado.Profiles = GetProfiles(IdApp);

                if (resultado.Profiles.Count()>0)
                resultado.permission = getPermission(resultado.Profiles.FirstOrDefault().Id, IdApp);
                return Json(new { data = resultado });
            }
        }

        public IActionResult saveApplication(ApplicationEntity Entidad)
        {
            try
            {
                validateSession();
                using (ApplicationBusiness nego = new ApplicationBusiness())
                {
                    if (Entidad.Id == 0)
                        Entidad.IdUsuarioAlta = (int)HttpContext.Session.GetInt32(Sessions.IdUser);
                    else
                        Entidad.IdUsuarioModificacion = (int)HttpContext.Session.GetInt32(Sessions.IdUser);

                    nego.saveApplication(Entidad);
                    if (Entidad.permission != null && Entidad.permission.Count>0)
                    nego.savePermission(Entidad.permission);
                }
                TempData["resultado"] = "SaveSuccess";
            }
            catch (Exception e)
            {
                HttpContext.Session.SetString(Sessions.MessageError, e.Message + " - " + e.Source);
                TempData["resultado"] = "SaveError";
            }
            return RedirectToAction("Index");
        }

        public IActionResult execAction(int TypeAction, int IdApp, int Status)
        {
            try
            {
                validateSession();
                using (ApplicationBusiness nego = new ApplicationBusiness())
                {
                    switch (TypeAction)
                    {
                        case (int)Enumerador.TypeAction.InActive:
                            nego.changeStatusApplication(IdApp,!Convert.ToBoolean(Status), (int)HttpContext.Session.GetInt32(Sessions.IdUser));
                            TempData["resultado"] = Convert.ToBoolean(Status)? "BlockSuccess": "UnBlockSuccess";
                            break;
                        case (int)Enumerador.TypeAction.Delete:
                            nego.DeleteApplication(IdApp);
                            TempData["resultado"] = "DeletedSuccess";
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                HttpContext.Session.SetString(Sessions.MessageError, e.Message);
                TempData["resultado"] = "SaveError";
            }
            return RedirectToAction("Index");

        }
       
        public List<ProfileEntity> GetProfiles(int idApp)
        {
            using (ApplicationBusiness AppNegocio = new ApplicationBusiness())
            {
                var resultado = AppNegocio.GetProfiles(IdApp:idApp);
                return resultado;
            }
        }

        public List<PermissionEntity> getPermission(int IdPerfil,int IdApp)
        {
            using (ApplicationBusiness nego = new ApplicationBusiness())
            {
                return  nego.getPermission(IdPerfil, IdApp);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SAyCC.Bussiness.Common;
using SAyCC.Bussiness.Login;
using SAyCC.Bussiness.SystemAdmin;
using SAyCC.Entities.Common;
using SAyCC.Entities.Login;
using SAyCC.Entities.SystemAdmin;
using SAyCC.SystemAdmin.Utilities;

namespace SystemAdmin.Controllers
{
    [SessionValidator]
    public class ApplicationController : Controller
    {
        private readonly IGenerals _generals;
        public ApplicationController(IGenerals generals)
        {
            _generals = generals;
        }

        public IActionResult Index(int? id)
        {
            try
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

                ViewBag.MessageError = HttpContext.Session.GetString(Sessions.MessageError);
                ViewBag.Resultado = TempData["resultado"];
                ViewBag.Applications = GetApplications();
                return View();

            }
            catch (Exception e)
            {
                ViewBag.MessageError = e.Message + " - " + e.Source;
                ViewBag.Resultado = "Error";
                return View();
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

        public JsonResult getDetailApplication(int IdApp)
        {
            using (LoginBusiness nego = new LoginBusiness())
            {
                var resultado = nego.getApplications(IdApp).FirstOrDefault();
                resultado.Modules = GetModulesAllowed(new int?(), IdApp);
                resultado.Profiles = GetProfiles(IdApp);

                /*if (resultado.Profiles.Count()>0)
                resultado.permission = getPermission(resultado.Profiles.FirstOrDefault().Id, IdApp);*/
                return Json(new { data = resultado });
            }
        }

        public IActionResult saveApplication(ApplicationEntity Entidad)
        {
            try
            {
                using (ApplicationBusiness nego = new ApplicationBusiness())
                {
                    if (Entidad.Id == 0)
                        Entidad.IdUsuarioAlta = (int)HttpContext.Session.GetInt32(Sessions.IdUser);
                    else
                        Entidad.IdUsuarioModificacion = (int)HttpContext.Session.GetInt32(Sessions.IdUser);

                    nego.saveApplication(Entidad);
                    /*if (Entidad.permission != null && Entidad.permission.Count>0)
                    nego.savePermission(Entidad.permission);*/
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
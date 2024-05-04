using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ModuleController : Controller
    {
        public IActionResult Index(int? id)
        {
            try
            {
                if (HttpContext.Session.GetInt32(Sessions.IdUser) != null)
                {
                    #region Configura Menú
                    ViewBag.IdUser = HttpContext.Session.GetInt32(Sessions.IdUser);
                    ViewBag.Modules = GetModulesAllowed(HttpContext.Session.GetInt32(Sessions.RolUser), HttpContext.Session.GetInt32(Sessions.IdApp));
                    string imgUp = HttpContext.Session.GetString(Sessions.ImagenUpload);
                    ViewBag.ImagenBytesIlustrative = string.IsNullOrEmpty(imgUp) ? null : Convert.FromBase64String(imgUp);
                    ViewBag.Alta = ViewBag.Modificacion = true;
                    ViewBag.NombreUsuario = HttpContext.Session.GetString(Sessions.UserName);
                    #endregion


                    ViewBag.MessageError = HttpContext.Session.GetString(Sessions.MessageError);
                    ViewBag.Mods = GetModulesAllowed();
                    ViewBag.Applications = GetApplications();
                    ViewBag.Resultado = TempData["resultado"];
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

        public List<ModuleEntity> GetModulesAllowed(int? IdPerfil = new int?(), int? IdApp = new int?())
        {
            using (ApplicationBusiness AppNegocio = new ApplicationBusiness())
            {
                var resultado = AppNegocio.GetModulesAllowed(IdPerfil, IdApp);
                return resultado;
            }
        }

        public JsonResult saveModule(int id, int IdApp, string Titulo, string Descripcion, string Icono, string Controlador, string Accion, int Orden)
        {
            validateSession();
            using (ApplicationBusiness nego = new ApplicationBusiness())
            {
                List<ModuleEntity> resultado;
                try
                {
                    int IDUser = (int)HttpContext.Session.GetInt32(Sessions.IdUser);
                    int? IDUserMod = id != 0 ? IDUser : new int?();
                    nego.saveModules(id, IdApp, Titulo, Descripcion, Icono, Controlador, Accion, Orden, id == 0 ? IDUser : new int?(), IDUserMod);
                    resultado = nego.GetModulesAllowed(new int?(), IdApp);
                }
                catch (Exception ex)
                {
                    resultado = new List<ModuleEntity>();
                    throw;
                }

                return Json(new { data = resultado });
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
       
        public JsonResult blockUnblockModule(int IdModule, int Status, int IdApp)
        {
            validateSession();
            using (ApplicationBusiness nego = new ApplicationBusiness())
            {
                List<ModuleEntity> resultado;
                try
                {
                    nego.changeStatusModule(IdModule, !Convert.ToBoolean(Status), (int)HttpContext.Session.GetInt32(Sessions.IdUser));
                    resultado = nego.GetModulesAllowed(new int?(),IdApp);
                }
                catch (Exception)
                {
                    resultado = new List<ModuleEntity>();
                    throw;
                }

                return Json(new { data = resultado });
            }
        }
        
        public JsonResult DeleteModule(int IdModule, int IdApp)
        {
            validateSession();
            using (ApplicationBusiness nego = new ApplicationBusiness())
            {
                List<ModuleEntity> resultado;
                try
                {
                    nego.DeleteModule(IdModule);
                    resultado = nego.GetModulesAllowed(new int?(), IdApp);
                }
                catch (Exception)
                {
                    resultado = new List<ModuleEntity>();
                    throw;
                }

                return Json(new { data = resultado });
            }
        }
       
        public JsonResult getDetailModule(int IdModule, int IdApp)
        {
            validateSession();
            using (LoginBusiness nego = new LoginBusiness())
            {
                var resultado = GetModulesAllowed(new int?(), IdApp).FirstOrDefault(x=>x.Id==IdModule);
                return Json(new { data = resultado });
            }
        }
        
    }

}
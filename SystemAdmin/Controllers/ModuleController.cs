using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
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
    [SessionValidator]
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
                    ViewBag.Modules = GetModulesAllowed(HttpContext.Session.GetInt32(Sessions.IdUser), HttpContext.Session.GetInt32(Sessions.IdApp));
                    string imgUp = HttpContext.Session.GetString(Sessions.ImagenUpload);
                    ViewBag.ImagenBytesIlustrative = string.IsNullOrEmpty(imgUp) ? null : Convert.FromBase64String(imgUp);
                    ViewBag.Alta = ViewBag.Modificacion = true;
                    ViewBag.NombreUsuario = HttpContext.Session.GetString(Sessions.UserName);
                    ViewBag.IdPagina = (int)id;
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
       
        public List<ModuleEntity> GetModulesAllowed(int? IdUsuario = new int?(), int? IdApp = new int?())
        {
            using (ApplicationBusiness AppNegocio = new ApplicationBusiness())
            {
                var resultado = AppNegocio.GetModulesAllowed(IdUsuario, IdApp);
                return resultado;
            }
        }

        public JsonResult saveModule(int id, int IdApp, string Titulo, string Descripcion, string Icono, string Controlador, string Accion, int Orden, int IdPadre)
        {
            using (ApplicationBusiness nego = new ApplicationBusiness())
            {
                List<ModuleEntity> resultado;
                try
                {
                    int IDUser = (int)HttpContext.Session.GetInt32(Sessions.IdUser);
                    int? IDUserMod = id != 0 ? IDUser : new int?();
                    nego.saveModules(id, IdApp, Titulo, Descripcion, Icono, Controlador, Accion, Orden, id == 0 ? IDUser : new int?(), IDUserMod, IdPadre == 0? new int?(): IdPadre);
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
            using (LoginBusiness nego = new LoginBusiness())
            {
                var resultado = GetModulesAllowed(new int?(), IdApp).FirstOrDefault(x=>x.Id==IdModule);
                var permissionsM = GetPermissionCatalogByNotExistInPage(IdModule);
                var permissionA = GetPermissionCatalogByPage(IdModule);
                return Json(new { data = resultado, permissionMissing = permissionsM, permissionAsigned = permissionA });
               // return Json(new { data = resultado });
            }
        }

        public JsonResult GetPagesFather(int IdApp)
        {
            using (ApplicationBusiness nego = new ApplicationBusiness())
            {
                var resultado = nego.GetPagesFathers(IdApp);
                return Json(new { data = resultado });
            }
        }

        public List<PermissionCatalogEntity> GetPermissionCatalogByNotExistInPage(int IdPagina)
        {
            using (ApplicationBusiness AppNegocio = new ApplicationBusiness())
            {
                var permissions = AppNegocio.GetPermissionCatalogByNotExistInPage(IdPagina);
                return permissions;
            }
        }

        public List<PermissionCatalogEntity> GetPermissionCatalogByPage(int IdPagina)
        {
            using (ApplicationBusiness AppNegocio = new ApplicationBusiness())
            {
                var permissions = AppNegocio.GetPermissionCatalogByPage(IdPagina);
                return permissions;
            }
        }

        public JsonResult SavePermisoPagina(int IdPermiso, int IdPagina)
        {
            using (ApplicationBusiness nego = new ApplicationBusiness())
            {
                nego.SavePermissionPage(IdPermiso, IdPagina);
                var permissionsM = GetPermissionCatalogByNotExistInPage(IdPagina);
                var permissionA = GetPermissionCatalogByPage(IdPagina);
                return Json(new { permissionMissing = permissionsM, permissionAsigned = permissionA });
            }
        }
        public JsonResult DeletePermissionPagina(int IdPermisoPagina, int IdPagina)
        {
            try
            {
                using (ApplicationBusiness nego = new ApplicationBusiness())
                {
                    nego.DeletePermissionPagina(IdPermisoPagina);
                    var permissionsM = GetPermissionCatalogByNotExistInPage(IdPagina);
                    var permissionA = GetPermissionCatalogByPage(IdPagina);
                    return Json(new { success = true, permissionMissing = permissionsM, permissionAsigned = permissionA });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
                throw;
            }
        }
    }

}
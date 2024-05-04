using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAyCC.Bussiness.Common;
using SAyCC.Bussiness.Login;
using SAyCC.Bussiness.SystemAdmin;
using SAyCC.Entities.Login;
using SAyCC.Entities.SystemAdmin;
using SAyCC.SystemAdmin.Utilities;

namespace SystemAdmin.Controllers
{
    public class PermissionController : Controller
    {
        public IActionResult Index()
        {
            validateSession();
            #region Configura Menú
            ViewBag.Modules = GetModulesAllowed(HttpContext.Session.GetInt32(Sessions.RolUser), HttpContext.Session.GetInt32(Sessions.IdApp));
            string imgUp = HttpContext.Session.GetString(Sessions.ImagenUpload);
            ViewBag.ImagenBytesIlustrative = string.IsNullOrEmpty(imgUp) ? null : Convert.FromBase64String(imgUp);
            ViewBag.Alta = ViewBag.Modificacion = true;
            ViewBag.NombreUsuario = HttpContext.Session.GetString(Sessions.UserName);
            #endregion

            List<ApplicationEntity> apps = GetApplications();
            List<ProfileEntity> perm = GetProfiles(apps.FirstOrDefault().Id);
            ViewBag.resultado = TempData["resultado"];
            ViewBag.MensajeErr = TempData["MensajeErr"];
            ViewBag.Applications = apps;
            ViewBag.Profiles = perm;
            ViewBag.Permission = getPermission(perm.FirstOrDefault().Id, apps.FirstOrDefault().Id);

            return View();
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

        public List<ProfileEntity> GetProfiles(int IdApp)
        {
            using (ApplicationBusiness AppNegocio = new ApplicationBusiness())
            {
                var resultado = AppNegocio.GetProfiles(IdApp:IdApp).Where(x => x.Activo).ToList();
                return resultado;
            }
        }

        public List<PermissionEntity> getPermission(int IdPerfil, int IdApp)
        {
            using (ApplicationBusiness nego = new ApplicationBusiness())
            {
                return nego.getPermission(IdPerfil, IdApp);
            }
        }

        public JsonResult saveAsignacionAplicacion(int Id, int IdProfile, int IdApp)
        {
            validateSession();
            using (ApplicationBusiness nego = new ApplicationBusiness())
            {
                List<ProfileEntity> resultado = new List<ProfileEntity>() { new ProfileEntity() { Id = IdProfile } };
                try
                {
                    nego.saveAsignacionAplicacion(Id, IdProfile, IdApp);
                }
                catch (Exception ex)
                {
                    resultado = new List<ProfileEntity>() { new ProfileEntity() { Id = 0, Detalle = ex.Message } };
                }

                return Json(new { data = resultado });
            }
        }

    }
}
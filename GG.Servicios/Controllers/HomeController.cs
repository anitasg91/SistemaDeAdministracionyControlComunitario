
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Hosting;
using SAyCC.GG.Servicios.Utilities;
using SAyCC.Bussiness.Common;
using SAyCC.Entities.SystemAdmin;
using SAyCC.Bussiness.SystemAdmin;

namespace GG.Servicios.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHostingEnvironment _environment;
        private IConfiguration Configuracion { get; }
        public HomeController(IConfiguration config, IHostingEnvironment IHostingEnvironment)
        {
            _environment = IHostingEnvironment;
            Configuracion = config;
        }

        public IActionResult Index(int? id)
        {
            if (id != new int?())
                HttpContext.Session.SetInt32(Sessions.IdModule, (int)id);

            if (validateSession())
            {
                #region Configura Menú
                // HttpContext.Session.SetInt32(Sessions.IdApp,(int) id);
                ViewBag.IdUser = HttpContext.Session.GetInt32(Sessions.IdUser);
                string imgUp = HttpContext.Session.GetString(Sessions.ImagenUpload);
                ViewBag.ImagenBytesIlustrative = string.IsNullOrEmpty(imgUp) ? null : Convert.FromBase64String(imgUp);
                var mods = GetModulesAllowed(HttpContext.Session.GetInt32(Sessions.RolUser), (int)HttpContext.Session.GetInt32(Sessions.IdApp));
                ViewBag.Modules = mods;
                int IDMod = (int)HttpContext.Session.GetInt32(Sessions.IdModule);
                ViewBag.Alta = mods.FirstOrDefault(x => x.Id == IDMod).Alta;
                ViewBag.Modificacion = mods.FirstOrDefault(x => x.Id == IDMod).Modificacion;
                ViewBag.NombreUsuario = HttpContext.Session.GetString(Sessions.UserName);
                #endregion

               // ViewBag.Profiles = GetProfiles();
               // ViewBag.CatalogApple = GetCatalog();

                ViewBag.Resultado = TempData["resultado"];
                ViewBag.userSession = TempData["userSession"];
                ViewBag.MessageError = TempData["Error"];

                //ViewBag.tableUser = GetUsersList();
                return View();
            }
            else
            {
                return Redirect(DBSet.urlRedirect);
            }
        }

        public bool validateSession()
        {
            if (HttpContext.Session.GetInt32(Sessions.IdUser) != null)
            {
                int IdUser = (int)HttpContext.Session.GetInt32(Sessions.IdUser);
                int IdApp = (int)HttpContext.Session.GetInt32(Sessions.IdApp);
                int RolUser = (int)HttpContext.Session.GetInt32(Sessions.RolUser);
                int IDMod = (int)HttpContext.Session.GetInt32(Sessions.IdModule);
                HttpContext.Session.SetInt32(Sessions.IdUser, (int)IdUser);
                HttpContext.Session.SetInt32(Sessions.IdApp, (int)IdApp);
                HttpContext.Session.SetInt32(Sessions.RolUser, (int)RolUser);
                HttpContext.Session.SetInt32(Sessions.IdModule, (int)IDMod);
                return true;
            }
            else
            {
                return false;
                //Redirect(DBSet.urlRedirect);
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

        public IActionResult CierraSesion()
        {
            var Id = HttpContext.Session.GetInt32(Sessions.IdUser);
            /*using (LoginBussiness UsrNegocio = new LoginBussiness(Configuracion))
            {
                UsrNegocio.CierraSession(Id);
            }*/
            HttpContext.Session.Remove(Sessions.IdUser);
            HttpContext.Session.Remove(Sessions.IdApp);
            HttpContext.Session.Remove(Sessions.RolUser);
            HttpContext.Session.Remove(Sessions.UserName);
            HttpContext.Session.Remove(Sessions.urlAplicacionRedirect);
            HttpContext.Session.Remove(Sessions.ImagenUpload);
            HttpContext.Session.Remove(Sessions.MessageError);
            HttpContext.Session.Remove(Sessions.IdModule);
            return Redirect(DBSet.urlRedirect);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}

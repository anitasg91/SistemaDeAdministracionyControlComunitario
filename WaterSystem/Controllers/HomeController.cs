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
using SAyCC.Bussiness.Common;
using SAyCC.Bussiness.DrinkingWaterSystem;
using SAyCC.Bussiness.SystemAdmin;
using SAyCC.Entities.Common;
using SAyCC.Entities.Login;
using SAyCC.Entities.SystemAdmin;
using SAyCC.WaterSystem.Utilities;
using System.Drawing;
using System.Net.Http.Headers;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using WaterSystem.Models;

namespace WaterSystem.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index(int? id)
        {
            if (validateSession())
            {
                #region Configura Menú
                if (id != new int?())
                    HttpContext.Session.SetInt32(Sessions.IdModule, (int)id);
                ViewBag.IdUser = HttpContext.Session.GetInt32(Sessions.IdUser);
                //ViewBag.Modules = GetModulesAllowed(HttpContext.Session.GetInt32(Sessions.RolUser), (int)HttpContext.Session.GetInt32(Sessions.IdApp));
                ViewBag.Modules = GetModulesAllowed(HttpContext.Session.GetInt32(Sessions.IdUser), (int)HttpContext.Session.GetInt32(Sessions.IdApp));
                string imgUp = HttpContext.Session.GetString(Sessions.ImagenUpload);
                ViewBag.ImagenBytesIlustrative = string.IsNullOrEmpty(imgUp) ? null : Convert.FromBase64String(imgUp);
                ViewBag.Alta = ViewBag.Modificacion = true;
                ViewBag.NombreUsuario = HttpContext.Session.GetString(Sessions.UserName);
                #endregion

                //ViewBag.Profiles = GetProfiles();
                //ViewBag.CatalogApple = GetCatalog();

                //ViewBag.Resultado = TempData["resultado"];
                //ViewBag.userSession = TempData["userSession"];
                //ViewBag.MessageError = TempData["Error"];

                ViewBag.tableUser = GetUsersList();
            return View();
            }
            else
            {
                return Redirect(DBSet.urlRedirect);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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

        public bool validateSession()
        {
            if (HttpContext.Session.GetInt32(Sessions.IdUser) != null)
            {
                int IdUser = (int)HttpContext.Session.GetInt32(Sessions.IdUser);
                int IdApp = (int)HttpContext.Session.GetInt32(Sessions.IdApp);
                int RolUser = (int)HttpContext.Session.GetInt32(Sessions.RolUser);
                HttpContext.Session.SetInt32(Sessions.IdUser, (int)IdUser);
                HttpContext.Session.SetInt32(Sessions.IdApp, (int)IdApp);
                HttpContext.Session.SetInt32(Sessions.RolUser, (int)RolUser);
                return true;
            }
            else
                return false;
        }

        public List<ModuleEntity> GetModulesAllowed(int? IdUsuario, int? IdApp)
        {
            using (ApplicationBusiness AppNegocio = new ApplicationBusiness())
            {
                var resultado = AppNegocio.GetModulesAllowed(IdUsuario, IdApp);
                return resultado;
            }
        }

        public List<UserEntity> GetUsersList()
        {
            using (ApplicationBusiness AppNegocio = new ApplicationBusiness())
            {
                var resultado = AppNegocio.GetUsers(true).Where(x=> x.Activo && x.Medidor.Any()).ToList();
                return resultado;
            }
        }
    }
}

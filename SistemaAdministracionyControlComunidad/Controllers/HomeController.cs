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
using SAyCC.Login.Utilities;
using SistemaAdministracionyControlComunidad.Models;
using SAyCC.Bussiness.Common;
using SAyCC.Bussiness.Login;
using SAyCC.Entities.Login;
using SAyCC.Entities.Common;

namespace SistemaAdministracionyControlComunidad.Controllers
{
    public class HomeController : Controller
    {
        private IConfiguration Configuracion { get; }
        public HomeController(IConfiguration config)
        {
            Configuracion = config;
        }

        public IActionResult Index(int? id)
        {
            HttpContext.Session.Remove(Sessions.IdUser);
            HttpContext.Session.Remove(Sessions.UserName);
            HttpContext.Session.Remove(Sessions.IdApp);
            ViewBag.CambioPassword = TempData["CambioPassword"];
            ViewBag.RecuperaPassword = TempData["RecoverPassword"];

            if (id != null)
            {
                //Obtiene datos de la url redireccionada, la guarda en una variable de sesion
                using (LoginBusiness ObjAdmin = new LoginBusiness())
                {
                    DBSet.DBcnn = Configuracion["ConnectionString:DefaultConnection"];
                    ApplicationEntity app = ObjAdmin.getApplications(id).FirstOrDefault();
                    HttpContext.Session.SetString(Sessions.urlAplicacionRedirect, "http://" + app.Dominio + "/" + app.Controlador + "/" + app.Accion + "/{0}/" + id);
                    DBSet.DBcnn = string.Empty;
                    return Redirect(DBSet.urlRedirect);
                }
            }
            return View();
        }
        
        public List<ApplicationEntity> GetApplicationsAllowed(int IdUser)
        {
            using (LoginBusiness AppNegocio = new LoginBusiness())
            {
                var resultado = AppNegocio.GetApplicationsAllowed(IdUser);
                return resultado;
            }
        }

        public IActionResult PrincipalView(int? id)
        {
            if (id != new int?())
            {
                UserEntity user = GetUserById((int)id).FirstOrDefault();
                HttpContext.Session.SetInt32(Sessions.IdUser, Convert.ToInt32(id));
                HttpContext.Session.SetString(Sessions.ImagenUpload, user.ImagenUpload);
                HttpContext.Session.SetString(Sessions.UserName, user.NombreCompleto );

            }
            if (HttpContext.Session.GetInt32(Sessions.IdUser) != null)
            {
                int IdUser = (int)HttpContext.Session.GetInt32(Sessions.IdUser);
                ViewBag.IdUser = IdUser;
                ViewBag.App = GetApplicationsAllowed(IdUser);
                ViewBag.Usr = IdUser;

                ViewBag.NombreUsuario = HttpContext.Session.GetString(Sessions.UserName);
                string imgUp = HttpContext.Session.GetString(Sessions.ImagenUpload);
                ViewBag.ImagenBytesIlustrative = string.IsNullOrEmpty(imgUp) ? null : Convert.FromBase64String(imgUp);

                List<NoticeEntity> NoticeList = GetNotice();

                return View(NoticeList);
            }
            else
            {
                return Redirect(Constantes.RutaLogin);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult RecoverPassword()
        {
            ViewBag.RecuperaPassword = TempData["RecoverPassword"];
            return View();
        }
        public IActionResult CierraSesion()
        {
            var Id = HttpContext.Session.GetInt32(Sessions.IdUser);
            /*using (LoginBussiness UsrNegocio = new LoginBussiness(Configuracion))
            {
                UsrNegocio.CierraSession(Id);
            }*/
            HttpContext.Session.Clear();
            return Redirect(DBSet.urlRedirect);
        }

        public List<UserEntity> GetUserById(int user)
        {
            using (LoginBusiness UsrNegocio = new LoginBusiness())
            {
                var resultado = UsrNegocio.GetUser(user);
                return resultado;
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region HTTPOST
        [HttpPost]
        [AllowAnonymous]
        public IActionResult validateUserExistLogin(LoginViewModel model)
        {
            String Password = GlobalBusines.GetSHA256(model.Password);
            //string pattern = @"^[0-9]+$";
            var User = model.SessionStart;
            // bool esExpediente = Regex.IsMatch(InicioSesion, pattern);

            using (LoginBusiness nego = new LoginBusiness())
            {
                DBSet.DBcnn = Configuracion["ConnectionString:DefaultConnection"];

                //var resultado = nego.AllowAccess(esExpediente ? null : InicioSesion, Password, esExpediente ? Convert.ToInt32(InicioSesion) : 0);
                var resultado = nego.AllowAccess(User, Password).FirstOrDefault();

                if (resultado != null)
                {
                    if (resultado.PrimerLogin)
                    {
                        TempData["Id"] = resultado.Id;
                        return View("CambiaContraseña");
                    }
                    if ( !resultado.Activo || !resultado.PerfilActivo)
                    {
                        ViewData["resultado"] = !resultado.Activo ? "inactiveLogin": "PerfilBlock";
                    }
                    else
                    {
                        int Id = resultado.Id;
                       // string Nombreusr = resultado.Nombre + " " + resultado.APaterno + " " + resultado.AMaterno;
                        HttpContext.Session.SetInt32(Sessions.IdUser, Id);
                        HttpContext.Session.SetString(Sessions.UserName, resultado.NombreCompleto);
                        //if (!string.IsNullOrEmpty(resultado.ImagenUpload))
                        HttpContext.Session.SetString(Sessions.ImagenUpload, !string.IsNullOrEmpty(resultado.ImagenUpload)? resultado.ImagenUpload:string.Empty);
                        if (HttpContext.Session.GetString(Sessions.urlAplicacionRedirect) != null)
                        {
                            string url = string.Format(HttpContext.Session.GetString(Sessions.urlAplicacionRedirect), Id);
                            HttpContext.Session.Remove(Sessions.urlAplicacionRedirect);
                            return Redirect(url);
                        }
                        return RedirectToAction("PrincipalView");
                    }
                }
                else
                {
                    int intentos = nego.AllowAccess(User).Count()>0 ? nego.AllowAccess(User).FirstOrDefault().NoIntentosFallidos:0;

                    if (intentos == int.Parse(GlobalBusines.GetConfiguracionGlobal().FirstOrDefault(x=>x.Nombre== "BloqueoLogin").Valor))
                    {
                        ViewData["resultado"] = "BlockLogin";
                        return View("index");
                    }
                    else if (intentos > 0 && intentos < int.Parse(GlobalBusines.GetConfiguracionGlobal().FirstOrDefault(x => x.Nombre == "BloqueoLogin").Valor))
                    {
                        ViewData["resultado"] = "ErrorLogin";
                        return View("index");
                    }
                    else
                    {
                        ViewData["resultado"] = "ErrorInfo";
                        return View("index");
                    }
                }
            }
            return View("Index");
        }
        [HttpPost]
        public async Task<IActionResult> CambioContraseña(ContrasenaViewModel model)
        {
            try
            {
                int user = Convert.ToInt32(TempData["Id"]);
                //string password = GlobalBusines.GetSHA256(model.Input.Password);
                string passwordnuevo = GlobalBusines.GetSHA256(model.Input.PasswordNuevo);
                //string correo = null;

                using (LoginBusiness UsrNegocio = new LoginBusiness())
                {
                    UsrNegocio.ChangePassword(user, passwordnuevo);
                    // UsrNegocio.PrimerLoginoff(user);
                    TempData["CambioPassword"] = "1";
                }
            }
            catch (Exception ex)
            {
                TempData["CambioPassword"] = "0";

            }
            return RedirectToAction("Index");
        }
        #endregion

        #region HTTGET

        public List<NoticeEntity> GetNotice()
        {
            using (LoginBusiness AppNegocio = new LoginBusiness())
            {
                var resultado = AppNegocio.getNoticeByIdUser(null);
                return resultado;
            }
        }
        #endregion


    }
}
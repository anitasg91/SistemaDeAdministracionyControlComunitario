using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAyCC.Bussiness.Common;
using SAyCC.Bussiness.Login;
using SAyCC.Entities.Common;
using SAyCC.Entities.Login;
using SAyCC.Login.Utilities;

namespace SAyCC.Login.Controllers
{
    public class DirectoryController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32(Sessions.IdUser) != null)
            {
                #region Configura Menú
                ViewBag.App = GetApplicationsAllowed((int)HttpContext.Session.GetInt32(Sessions.IdUser));
                ViewBag.IdUser = HttpContext.Session.GetInt32(Sessions.IdUser);
                #endregion

                ViewBag.NombreUsuario = HttpContext.Session.GetString(Sessions.UserName);
                string imgUp = HttpContext.Session.GetString(Sessions.ImagenUpload);
                ViewBag.ImagenBytesIlustrative = string.IsNullOrEmpty(imgUp) ? null : Convert.FromBase64String(imgUp);

                ViewBag.Users = GetUserList().Where(x => x.IdPerfil != (int)Enumerador.Perfil.Usuario && x.Activo).ToList();
                return View();
            }
            else
            {
                return Redirect(DBSet.urlRedirect);
            }
        }
        public List<UserEntity> GetUserList()
        {
            using (LoginBusiness UsrNegocio = new LoginBusiness())
            {
                var resultado = UsrNegocio.GetUser();
                return resultado;
            }
        }
        public List<ApplicationEntity> GetApplicationsAllowed(int IdUser)
        {
            using (LoginBusiness AppNegocio = new LoginBusiness())
            {
                var resultado = AppNegocio.GetApplicationsAllowed(IdUser);
                return resultado;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAyCC.Bussiness.Common;
using SAyCC.Bussiness.Login;
using SAyCC.Entities.Common;
using SAyCC.Entities.Login;
using SAyCC.Login.Utilities;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SAyCC.Login.Controllers
{
    public class NoticeController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            if (validateSession())
            {
                int idUser = (int)HttpContext.Session.GetInt32(Sessions.IdUser);
                #region Configura Menú
                ViewBag.App = GetApplicationsAllowed(idUser);
                ViewBag.IdUser = idUser;

                ViewBag.NombreUsuario = HttpContext.Session.GetString(Sessions.UserName);
                string imgUp = HttpContext.Session.GetString(Sessions.ImagenUpload);
                ViewBag.ImagenBytesIlustrative = string.IsNullOrEmpty(imgUp) ? null : Convert.FromBase64String(imgUp);
                #endregion

                ViewBag.tableNotice = getNoticeByIdUser(idUser);
                ViewBag.Applications = GetApplications(idUser);
                ViewBag.TipoAviso = getTipoAviso();
                ViewBag.Prioridad = getPrioridad();
                
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
               // int RolUser = (int)HttpContext.Session.GetInt32(Sessions.RolUser);
                HttpContext.Session.SetInt32(Sessions.IdUser, (int)IdUser);
               // HttpContext.Session.SetInt32(Sessions.RolUser, (int)RolUser);
                return true;
            }
            else
            {
                return false;
                //Redirect(DBSet.urlRedirect);
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
        public List<NoticeEntity> getNoticeByIdUser(int IdUser)
        {
            using (LoginBusiness AppNegocio = new LoginBusiness())
            {
                var resultado = AppNegocio.getNoticeByIdUser(IdUser);
                //resultado.ForEach(y => y.Descripcion = y.Id == (int)Enumerador.Aplicacion.AdministracionDelSistema ? "General" : y.Descripcion);

                return resultado;
            }
        }
        public List<TipoAvisoEntity> getTipoAviso()
        {
            using (LoginBusiness AppNegocio = new LoginBusiness())
            {
                var resultado = AppNegocio.getTipoAviso();
                return resultado;
            }
        }
        public List<PrioridadEntity> getPrioridad()
        {
            using (LoginBusiness AppNegocio = new LoginBusiness())
            {
                var resultado = AppNegocio.getPrioridad();
                return resultado;
            }
        }

        public List<ApplicationEntity> GetApplications(int idUser)
        {
            using (LoginBusiness AppNegocio = new LoginBusiness())
            {
                var resultado = AppNegocio.GetApplicationsAllowed(idUser).Where(x=>x.Id != (int)Enumerador.Aplicacion.Avisos).ToList();
                resultado.ForEach(y => y.Descripcion = y.Id == (int)Enumerador.Aplicacion.AdministracionDelSistema ? "General" : y.Descripcion);
                return resultado;
            }
        }


        #region HTTPOST
        [HttpPost]
        [AllowAnonymous]
        public IActionResult saveNotice(NoticeEntity entity)
        {
            if (validateSession())
            {
                try
                {
                    using (LoginBusiness nego = new LoginBusiness())
                    {
                        entity.IdUsuario = (int)HttpContext.Session.GetInt32(Sessions.IdUser);
                        int IdAviso = nego.SaveUpdateAviso(entity);

                        TempData["resultado"] = entity.Id == 0 ? "SaveSuccess" : "UpdateSuccess";
                    }
                }
                catch (Exception e)
                {
                    TempData["resultado"] = "SaveError";
                    TempData["Error"] = e.Message + " - " + e.Source;
                }
                return RedirectToAction("Index");
            }
            else
            {
                return Redirect(DBSet.urlRedirect);
            }
        }
        #endregion

    }
}

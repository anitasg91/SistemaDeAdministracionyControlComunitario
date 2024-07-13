using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SAyCC.Bussiness.Common;
using SAyCC.Bussiness.Login;
using SAyCC.Bussiness.SystemAdmin;
using SAyCC.Entities.Login;
using SAyCC.Entities.SystemAdmin;
using SAyCC.GG.Servicios.Utilities;

namespace GG.Servicios.Controllers
{
    public class StartApplicationController : Controller
    {
        private IConfiguration _Config { get; }
        public StartApplicationController(IConfiguration Configuracion)
        {
            _Config = Configuracion;
        }
        public IActionResult configSession(int IdUsuario, int IdApp)
        {
            DBSet.DBcnn = _Config["ConnectionString:DefaultConnection"];
            HttpContext.Session.SetInt32(Sessions.IdUser, IdUsuario);
            HttpContext.Session.SetInt32(Sessions.IdApp, IdApp);
            getUser(IdUsuario);
            var Modulo = GetModulesAllowed(HttpContext.Session.GetInt32(Sessions.IdUser), IdApp).OrderBy(x=>x.Orden).FirstOrDefault();
            //var Modulo = GetModulesAllowed(HttpContext.Session.GetInt32(Sessions.RolUser), IdApp).OrderBy(x=>x.Orden).FirstOrDefault();
           // HttpContext.Session.SetInt32(Sessions.IdModule, Modulo!= null? Modulo.Id:0);

            return RedirectToAction(Modulo != null? Modulo.Accion: "Privacy", Modulo != null ? Modulo.Controlador :"Home", new { id = Modulo != null? Modulo.Id:0 });
          //  return RedirectToAction(Modulo != null? Modulo.Accion: "Privacy", Modulo != null ? Modulo.Controlador :"Home", new { id = IdApp });
        }
        public void getUser(int IDUser)
        {
            using (LoginBusiness UsrNegocio = new LoginBusiness())
            {
                UserEntity usuario = UsrNegocio.GetUser(IDUser).FirstOrDefault();
                //HttpContext.Session.SetString(Sessions.UserName, usuario.Nombre + " " + usuario.APaterno + " " + usuario.AMaterno);
                HttpContext.Session.SetString(Sessions.UserName, usuario.NombreCompleto);
                HttpContext.Session.SetString(Sessions.ImagenUpload, usuario.ImagenUpload);
               // HttpContext.Session.SetInt32(Sessions.RolUser, usuario.IdPerfil);
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
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SAyCC.Bussiness.Common;
using SAyCC.Bussiness.Login;
using SAyCC.Bussiness.SystemAdmin;
using SAyCC.Entities.Login;
using SAyCC.Entities.SystemAdmin;
using SAyCC.WaterSystem.Utilities;

namespace WaterSystem.Controllers
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
            DBSet.DBcnn = _Config[Sessions.DefaultConnection];
            DBSet.urlRedirect = _Config[Sessions.LoginApp];

            ConfigureSesionUser(IdUsuario, IdApp);
            var Modulo = GetModuleCurrent(IdUsuario, IdApp);
            return RedirectToAction(Modulo != null ? Modulo.Accion : Sessions.Privacy, Modulo != null ? Modulo.Controlador : Sessions.Home, new { id = Modulo != null ? Modulo.Id : 0 });
        }

        private void ConfigureSesionUser(int IdUsuario, int IdApp)
        {
            //Obtiene usuario
            GetUser(IdUsuario);

            //Configura Roles
            GetRolesByUserAndAplication(IdUsuario, IdApp);

            //Configura permisos
            GetPermissionPageCurrent(IdUsuario, IdApp);

            //Configura las manzanas que tiene asociadas, guardarlo en variable de sesión
            GetBlockAsignedToUser(IdUsuario, IdApp);
        }

        private void GetUser(int IDUser)
        {
            using (LoginBusiness UsrNegocio = new LoginBusiness())
            {
                UserEntity usuario = UsrNegocio.GetUser(IDUser).FirstOrDefault();
                if (usuario != null)
                {
                    HttpContext.Session.SetInt32(Sessions.IdUser, IDUser);
                    HttpContext.Session.SetString(Sessions.UserName, usuario.NombreCompleto);
                    HttpContext.Session.SetString(Sessions.ImagenUpload, usuario.ImagenUpload);
                    HttpContext.Session.SetInt32(Sessions.IdManzana, usuario.IdManzana);
                }
            }
        }

        private ModuleEntity GetModuleCurrent(int IdUsuario, int IdApp)
        {
            ModuleEntity pagina = new ModuleEntity();
            using (ApplicationBusiness AppNegocio = new ApplicationBusiness())
            {
                var resultado = AppNegocio.GetModulesAllowed(IdUsuario, IdApp).OrderBy(x => x.Orden).ToList();
                if (resultado.Any()) {
                    string json = JsonConvert.SerializeObject(resultado);
                    HttpContext.Session.SetString(Sessions.AllPagesByApp, json);
                    HttpContext.Session.SetInt32(Sessions.IdApp, IdApp);
                    pagina = resultado.FirstOrDefault(x => !string.IsNullOrEmpty(x.Accion));
                }
                //return resultado;
                return pagina;
            }
        }

        private void GetPermissionPageCurrent(int IdUsuario, int IdApp)
        {
            using (ApplicationBusiness AppNegocio = new ApplicationBusiness())
            {
                var resultado = AppNegocio.GetPermissionPageByUserApp(IdUsuario, IdApp);
                if (resultado.Any()) {
                    string jsonPPC = JsonConvert.SerializeObject(resultado);
                    HttpContext.Session.SetString(Sessions.PermissionPageCurrent, jsonPPC);
                }
            }
        }

        private void GetRolesByUserAndAplication(int IdUsuario, int IdApp)
        {
            using (ApplicationBusiness AppNegocio = new ApplicationBusiness())
            {
                var resultado = AppNegocio.GetRolesByUserAndAplication(IdUsuario, IdApp);
                if (resultado.Any()) {
                    string json = JsonConvert.SerializeObject(resultado);
                    HttpContext.Session.SetString(Sessions.RolesList, json);
                }
            }
        }

        private void GetBlockAsignedToUser(int IdUsuario, int IdApp)
        {
            using (ApplicationBusiness AppNegocio = new ApplicationBusiness())
            {
                var resultado = AppNegocio.GetBlockAsignedToUser(IdUsuario, IdApp).Select(x => new { Id = x.Id, Descripcion = x.Descripcion }).ToList();
                if (resultado.Any())
                {
                    string jsonBATU = JsonConvert.SerializeObject(resultado);
                    HttpContext.Session.SetString(Sessions.BlockAsignedList, jsonBATU);
                }
            }
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
    }
}
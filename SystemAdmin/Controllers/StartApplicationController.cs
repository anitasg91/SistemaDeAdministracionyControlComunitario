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
using SAyCC.SystemAdmin.Utilities;

namespace SystemAdmin.Controllers
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
            HttpContext.Session.SetInt32(Sessions.IdUser, IdUsuario);
            HttpContext.Session.SetInt32(Sessions.IdApp, IdApp);
            ConfigureSesionUser(IdUsuario, IdApp);

            var Paginas = GetModulesAllowed(HttpContext.Session.GetInt32(Sessions.IdUser), IdApp).OrderBy(x => x.Orden).ToList();
            string json = JsonConvert.SerializeObject(Paginas);
            HttpContext.Session.SetString(Sessions.AllPagesByApp, json);

            var Modulo = Paginas.FirstOrDefault(x=> !string.IsNullOrEmpty(x.Accion));
            return RedirectToAction(Modulo != null? Modulo.Accion: "Privacy", Modulo != null ? Modulo.Controlador :"Home", new { id = Modulo != null? Modulo.Id:0 });
        }
        private void ConfigureSesionUser(int IdUsuario, int IdApp)
        {
            using (LoginBusiness UsrNegocio = new LoginBusiness())
            {
                UserEntity usuario = UsrNegocio.GetUser(IdUsuario).FirstOrDefault();
                HttpContext.Session.SetString(Sessions.UserName, usuario.NombreCompleto);
                HttpContext.Session.SetString(Sessions.ImagenUpload, usuario.ImagenUpload);
                //HttpContext.Session.SetInt32(Sessions.RolUser, usuario.IdPerfil);
            }
            var rolesAsign = GetRolesByUserAndAplication(IdUsuario, IdApp);
            string json = JsonConvert.SerializeObject(rolesAsign);
            HttpContext.Session.SetString(Sessions.RolesList, json);

            //Otener los permisos, guardarlo en variable de sesión
            List<PermissionPageCurrentEntity> PPC = GetPermissionPageCurrent(IdUsuario, IdApp);
            string jsonPPC = JsonConvert.SerializeObject(PPC);
            HttpContext.Session.SetString(Sessions.PermissionPageCurrent, jsonPPC);

            //Otener las manzanas que tiene asociadas, guardarlo en variable de sesión
            var BATU = GetBlockAsignedToUser(IdUsuario, IdApp).Select(x => new {Id=x.Id, Descripcion = x.Descripcion }).ToList();
            string jsonBATU = JsonConvert.SerializeObject(BATU);
            HttpContext.Session.SetString(Sessions.BlockAsignedList, jsonBATU);
        }
        private List<ModuleEntity> GetModulesAllowed(int? IdUsuario, int? IdApp)
        {
            using (ApplicationBusiness AppNegocio = new ApplicationBusiness())
            {
                var resultado = AppNegocio.GetModulesAllowed(IdUsuario, IdApp);
                return resultado;
            }
        }

        private List<PermissionPageCurrentEntity> GetPermissionPageCurrent(int IdUsuario, int IdApp)
        {
            using (ApplicationBusiness AppNegocio = new ApplicationBusiness())
            {
                var resultado = AppNegocio.GetPermissionPageByUserApp(IdUsuario, IdApp);
                return resultado;
            }
        }

        private List<ProfileSesionEntity> GetRolesByUserAndAplication(int IdUsuario, int IdApp)
        {
            using (ApplicationBusiness AppNegocio = new ApplicationBusiness())
            {
                var resultado = AppNegocio.GetRolesByUserAndAplication(IdUsuario, IdApp);
                return resultado;
            }
        }

        private List<CatalogEntity> GetBlockAsignedToUser(int IdUsuario, int IdApp)
        {
            using (ApplicationBusiness AppNegocio = new ApplicationBusiness())
            {
                var resultado = AppNegocio.GetBlockAsignedToUser(IdUsuario, IdApp);
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
            HttpContext.Session.Clear();
            return Redirect(DBSet.urlRedirect);
        }
    }
}
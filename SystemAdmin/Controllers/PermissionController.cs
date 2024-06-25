using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SAyCC.Bussiness.Common;
using SAyCC.Bussiness.Login;
using SAyCC.Bussiness.SystemAdmin;
using SAyCC.Entities.Common;
using SAyCC.Entities.Login;
using SAyCC.Entities.SystemAdmin;
using SAyCC.SystemAdmin.Utilities;
using static SAyCC.Entities.Common.Enumerador;

namespace SystemAdmin.Controllers
{
     [SessionValidator]
    public class PermissionController : Controller
    {
        private readonly IGenerals _generals;
        public PermissionController(IGenerals generals)
        {
            _generals = generals;
        }

        public IActionResult Index(int? id)
        {
            #region Varibles de configuración para armar el Menú
              if (id != new int?())
              {
                  //Importante guardar la paginaActual cada que se navegue
                  HttpContext.Session.SetInt32(Sessions.CurrentPage, (int)id);
                  if (!_generals.AllPagesByAppList.Any(_ => _.Id == id)) {
                      return View(PartialViewEnum.PageNotAccess);
                  }
              }
            #endregion

            ViewBag.resultado = TempData["resultado"];
            ViewBag.MensajeErr = TempData["MensajeErr"];

            List<ApplicationEntity> apps = GetApplications();
            ViewBag.Applications = apps;
            ViewBag.ConfigurationsPermissions = GetConfigurationPermissions(apps.FirstOrDefault().Id);
            var userList = GetUsersList();

            return View(userList);
        }
        
        public List<ModuleEntity> GetModulesAllowed(int? IdUsuario, int? IdApp)
        {
            using (ApplicationBusiness AppNegocio = new ApplicationBusiness())
            {
                var resultado = AppNegocio.GetModulesAllowed(IdUsuario, IdApp);
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

        public List<UserUtilityEntity> GetUsersList()
        {
            using (ApplicationBusiness AppNegocio = new ApplicationBusiness())
            {
                var resultado = AppNegocio.GetUsersListForPermissions();
                return resultado;
            }
        }

        public PermissionListEntity GetConfigurationPermissions(int IdApp) {
            using (ApplicationBusiness AppNegocio = new ApplicationBusiness())
            {
                PermissionListEntity entity = new PermissionListEntity();

                entity.Block = AppNegocio.GetCatalog((int)Enumerador.CatalogType.Apple);
                entity.Role = AppNegocio.GetProfiles(null,IdApp = IdApp);

                //var resultado = AppNegocio.GetConfigurationPermissions(IdApp);
                return entity;
            }
        }

        public JsonResult SaveConfigPermission(string model)
        {
            try
            {
                PermissionsConfigJson entity = JsonConvert.DeserializeObject<PermissionsConfigJson>(model);
                PermissionsConfigRequest request = new PermissionsConfigRequest();

                foreach (var item in entity.Block)
                {
                    UsuarioManzana block = new UsuarioManzana()
                    {
                        IdManzana = item,
                        IdUsuario = entity.IdUsuario,
                        IdAplicacion = entity.IdAplicacion
                    };
                    request.Block.Add(block);
                }
                foreach (var item in entity.Roles)
                {
                    UsuarioPerfil role = new UsuarioPerfil() { 
                    IdUsuario = entity.IdUsuario,
                    IdPerfil = item,
                    IdAplicacion = entity.IdAplicacion
                    };
                    request.Roles.Add(role);
                }
                using (ApplicationBusiness AppNegocio = new ApplicationBusiness())
                {
                    AppNegocio.SaveConfigPermissionsByUser(request);
                }

                return Json(new { success = true, message = "Se han guardado los datos con éxito." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPermissionsByApp( int IdApp)
        {
            try
            {
              var list =  GetConfigurationPermissions(IdApp);

              return PartialView("_PermissionsList", list);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPermissionsListByUser(int IdUser, int IdApp)
        {
            try
            {
                PermissionListEntity response = GetConfigurationPermissions(IdApp);
                using (ApplicationBusiness AppNegocio = new ApplicationBusiness())
                {
                    response.UserProfile = AppNegocio.GetRolesByUserAndAplication(IdUser, IdApp);
                    response.UserBlock = AppNegocio.GetBlockByUserAndAplication(IdUser, IdApp);
                }

                return PartialView(PartialViewEnum.PermissionsList, response);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
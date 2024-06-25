using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
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
    public class PermisoPaginaPerfilController : Controller
    {
        private readonly IGenerals _generals;
        public PermisoPaginaPerfilController(IGenerals generals)
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

            ViewBag.resultado = TempData[TempDataEnum.Resultado];
            ViewBag.MensajeErr = TempData[TempDataEnum.MensajeErr];
            List<ApplicationEntity> apps = GetApplications();
            ViewBag.Applications = apps;

            var roles = GetProfiles(apps.FirstOrDefault().Id);
            return View(roles);
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
                var resultado = AppNegocio.GetProfiles(IdApp:IdApp).ToList();
                return resultado;
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetProfilesByApp(int IdApp)
        {
            try
            {
                var list = GetProfiles(IdApp);
                return PartialView(PartialViewEnum.Table, list);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetProfilesById(int IdApp, int IdPerfil)
        {
            try
            {
                PermisoPaginaPerfilResponse response = new PermisoPaginaPerfilResponse();
                response.Perfil = IdPerfil>0? GetProfiles(IdApp).FirstOrDefault(_=>_.Id == IdPerfil):new ProfileEntity();
                response.Paginas = GetModulesAllowed(new int?(),IdApp);
                response.PermisosPagina = GetPermissionPageAndAsign(IdPerfil, IdApp);

                return PartialView(PartialViewEnum.PerfilAsign, response);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public List<PermisosByPagina> GetPermissionPageAndAsign(int IdPerfil, int? IdApp, int? IdPagina = new int?())
        {
            using (ApplicationBusiness AppNegocio = new ApplicationBusiness())
            {
                var resultado = AppNegocio.GetPermissionPageAndAsign(IdPerfil, IdApp, IdPagina);
                return resultado;
            }
        }

        public JsonResult SaveDetailProfile(string model)
        {
            try
            {
                string decode = _generals.DecompressFromBase64(model);
                var request = JsonConvert.DeserializeObject<PermisoPaginaPerfilRequest>(decode);
                request.IdUsuarioModificacion = (int)HttpContext.Session.GetInt32(Sessions.IdUser);
                using (ApplicationBusiness AppNegocio = new ApplicationBusiness())
                {
                    AppNegocio.SaveDetailProfileAndPermissionList(request);
                }

                return Json(new { success = true, message = "Se han guardado los datos con éxito." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetModalLockUnlockProfile(int IdPerfil, bool Estatus)
        {
            try
            {
                ProfileEntity pe = new ProfileEntity() { Id = IdPerfil, Activo = Estatus };

                return PartialView(PartialViewEnum.LockUnlockModal, pe);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public JsonResult blockUnblockProfile(int IdProfile, bool Status)
        {
            using (ApplicationBusiness nego = new ApplicationBusiness())
            {
                List<ProfileEntity> resultado;
                try
                {
                    nego.changeStatusProfile(IdProfile, !Convert.ToBoolean(Status), _generals.IdUser);
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.Message });
                }
                return Json(new { success = true, message = Constantes.UpdateRecord });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetModalDeleteProfile(int IdPerfil)
        {
            try
            {
                return PartialView(PartialViewEnum.DeleteProfileModal, IdPerfil);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        public JsonResult DeleteProfile(int IdProfile)
        {
            using (ApplicationBusiness nego = new ApplicationBusiness())
            {
                List<ProfileEntity> resultado;
                try
                {
                    nego.DeleteProfile(IdProfile);
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.Message });
                }
                return Json(new { success = true, message = Constantes.UpdateRecord });
            }
        }

    }
}
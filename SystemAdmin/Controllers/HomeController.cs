
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
using SAyCC.SystemAdmin.Utilities;
using SystemAdmin.Models;
using System.Drawing;
using System.Net.Http.Headers;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using SAyCC.Entities.WaterSystem;
using System.Net.NetworkInformation;
using Microsoft.VisualBasic;

namespace SystemAdmin.Controllers
{
    [SessionValidator]
    public class HomeController : Controller
    {
        private readonly IGenerals _generals;
        private readonly IHostingEnvironment _environment;

        private IConfiguration _Config { get; }
        //private readonly ILogger<HomeController> _logger;
        public HomeController(IConfiguration config, IHostingEnvironment IHostingEnvironment, IGenerals generals)
        {
            _environment = IHostingEnvironment;
            _Config = config;
            _generals = generals;
            DBSet.urlRedirect = _Config[Sessions.LoginApp];

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

                ViewBag.Profiles = GetProfiles();
                ViewBag.CatalogApple = GetCatalog();

                ViewBag.Resultado = TempData["resultado"];
                ViewBag.userSession = TempData["userSession"];
                ViewBag.MessageError = TempData["Error"];

                ViewBag.tableUser = GetUsersList();
                return View();
        }

        public IActionResult Privacy()
        {
            return View();
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
                var resultado = AppNegocio.GetUsers();
                return resultado;
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public List<ProfileEntity> GetProfiles()
        {
            using (ApplicationBusiness AppNegocio = new ApplicationBusiness())
            {
                var resultado = AppNegocio.GetProfiles().Where(x=> x.Activo).ToList();
                return resultado;
            }
        }

        public List<CatalogEntity> GetCatalog()
        {
            using (ApplicationBusiness AppNegocio = new ApplicationBusiness())
            {
                var resultado = AppNegocio.GetCatalog((int)Enumerador.CatalogType.Apple);
                return resultado;
            }
        }
       
        public string returnArrayVarbinaryImage(IFormFile file, ref string fileName)
        {
            fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            string newFileName = fileName + Path.GetExtension(fileName);
            string ruta = Path.Combine(_environment.WebRootPath, "ImagesTemporal");
            if (!Directory.Exists(ruta))
                Directory.CreateDirectory(ruta);

            fileName = ruta + $@"\{newFileName}";
            FileStream fs = System.IO.File.Create(fileName);
            file.CopyTo(fs);
            fs.Close();
            System.Drawing.Image img = System.Drawing.Image.FromFile(fileName);

            //Bitmap objBitmap = new Bitmap(img, new Size(500, 500));
            //System.Drawing.Image imgnew = objBitmap;
            //img.Dispose();
            //string varBinary = Convert.ToBase64String(Image2Bytes(imgnew));
            string varBinary = Convert.ToBase64String(Image2Bytes(img));

            return varBinary;
        }

        public static byte[] Image2Bytes(System.Drawing.Image img)
        {
            string sTemp = Path.GetTempFileName();
            FileStream fs = new FileStream(sTemp, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            img.Save(fs, System.Drawing.Imaging.ImageFormat.Jpeg);
            fs.Position = 0;
            int imgLength = Convert.ToInt32(fs.Length);
            byte[] bytes = new byte[imgLength];
            fs.Read(bytes, 0, imgLength);

            fs.Close();
            img.Dispose();
            return bytes;
        }
        public List<ConfiguracionGlobalEntity> getConfiguracionGlobal( string Nombre)
        {
            using (CommonBusiness AppNegocio = new CommonBusiness())
            {
                var resultado = AppNegocio.getConfiguracionGlobal(Nombre);
                return resultado;
            }
        }

        #region HTTPOST
        [HttpPost]
        [AllowAnonymous]
        public IActionResult saveUser(UserEntity entity)
        {

            try
            {
                string imageUp = "";
                var file = Request.Form.Files.Count() > 0 ? Request.Form.Files[0] : null;
                if (file != null && file.ContentType.Contains("image"))
                {
                    string fileName = "";
                    imageUp = returnArrayVarbinaryImage(file, ref fileName);
                    System.IO.File.Delete(fileName);
                }
                entity.ImagenUpload = string.IsNullOrEmpty(imageUp) ? null : imageUp;
                entity.Password = entity.CreateUser ? GlobalBusines.GetSHA256(getConfiguracionGlobal(DBSet.GenPass).FirstOrDefault().Valor) : null;

                using (ApplicationBusiness nego = new ApplicationBusiness())
                {
                    int Id = 0;
                    entity.IdUsuarioAlta = _generals.IdUser;
                    entity.IdUsuarioMod = entity.Id == 0 ? new int?() : _generals.IdUser; ;
                    string usuario = nego.saveUser(entity, ref Id);

                    if (Id > 0 && entity.Medidor != null && entity.Medidor.Count() > 0)
                    {
                        var medidor = nego.GetWaterMeterByIdUser(entity.Id);

                        var eliminar = (from t in medidor where !entity.Medidor.Any(x => x.Id == t.Id) select t).ToList();
                        foreach (var item in eliminar)
                        {
                            nego.UpdateWaterMeterById(item.Id, (int)Enumerador.ModifyTypeWaterMeter.Disassociate, new int?());
                        }
                        foreach (var item in entity.Medidor)
                        {
                            item.IdTitular = Id;
                            nego.SaveWaterMeter(item);
                        }
                    }

                    TempData["resultado"] = entity.Id == 0 ? "SaveSuccess" : "UpdateSuccess";
                    TempData["userSession"] = entity.CreateUser ? "Tu usuario de sesión es: " + usuario : "";
                }
            }
            catch (Exception e)
            {
                TempData["resultado"] = "SaveError";
                TempData["Error"] = e.Message + " - " + e.Source;
            }
            return RedirectToAction("Index");

        }
        #endregion

        #region JSon
        public JsonResult GetWaterMeter(int Numero)
        {
           
                using (WaterSystemBusiness nego = new WaterSystemBusiness())
                {
                    var resultado = nego.GetWaterMeter(Numero: Numero).FirstOrDefault();
                    return Json(new { data = resultado });
                }
            
        }
        public JsonResult getDetailUserByIdUser(int IdUser)
        {
            using (ApplicationBusiness nego = new ApplicationBusiness())
            {
                var resultado = nego.GetUserDetailById(IdUser);
                resultado.Medidor = nego.GetWaterMeterByIdUser(IdUser);

                return Json(new { data = resultado });
            }
        }

        #endregion

        [HttpGet]
        public async Task<IActionResult> GetModalLockUnlockUser(int IdUsuario, int Estatus)
        {
            try
            {
                UserUtilityEntity pe = new UserUtilityEntity() { Id = IdUsuario, IdEstatus = Estatus };
                return PartialView(PartialViewEnum.LockUnlockUserModal, pe);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetModalDeleteUser(int IdUsuario)
        {
            try
            {
                UserUtilityEntity pe = new UserUtilityEntity() { Id = IdUsuario, IdEstatus = (int)EstatusUsuario.Eliminacion };
                return PartialView(PartialViewEnum.DeleteUserModal, pe);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetModalApproveMoveUser(int IdUsuario, int Estatus)
        {
            try
            {
                UserUtilityEntity pe = new UserUtilityEntity() { Id = IdUsuario, IdEstatus = Estatus };
                GetConfigurationModalApproveMoveUser(ref pe);
                return PartialView(PartialViewEnum.ApproveMovementUser, pe);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        private void GetConfigurationModalApproveMoveUser(ref UserUtilityEntity entity)
        {
            switch (entity.IdEstatus)
            {
                case (int)EstatusUsuario.EnvíoInactivación:
                case (int)EstatusUsuario.ProcesoAprobaciónInactivación:
                    entity.TitleModal = "Bloquear usuario";
                    entity.IconoModal = "lock";
                    entity.MensajeModal = Constantes.ApproveLockUser;
                    break;
                case (int)EstatusUsuario.EnvíoActivación:
                case (int)EstatusUsuario.ProcesoAprobaciónActivación:
                    entity.TitleModal = "Desbloquear usuario";
                    entity.IconoModal = "lock-open";
                    entity.MensajeModal = Constantes.ApproveUnlockUser;
                    break;
                case (int)EstatusUsuario.EnvíoEliminación:
                case (int)EstatusUsuario.ProcesoAprobaciónEliminación:
                    entity.TitleModal = "Eliminar usuario";
                    entity.IconoModal = "trash-alt";
                    entity.MensajeModal = Constantes.ApproveDeleteUser;
                    break;
                default:
                    entity.TitleModal = "Recuperar usuario eliminado";
                    entity.IconoModal = "fa-user-plus";
                    entity.MensajeModal = Constantes.ApproveMoveUser;
                    break;
            }
        }

        public JsonResult SaveBlockUnblockUser(int IdUsuario, int Estatus, string Motivo = null)
        {
            string strmessage = "";
            using (ApplicationBusiness nego = new ApplicationBusiness())
            {

                try
                {
                    if (_generals.IsSuperAdmin)
                    {
                        Motivo = GetMessageStatusValue(Estatus);
                        nego.ChangeStatusUserBySuperAdmin(IdUsuario, GetNewStatusValueForSuperAdmin(Estatus), Motivo, _generals.IdUser);
                        strmessage = Motivo + ", exitosamente.";
                    }
                    else
                    {
                        List<ProfileEntity> resultado;
                        int newState = GetNewStatusValue(Estatus);
                        nego.ChangeStatusUser(IdUsuario, newState, Motivo, _generals.IdUser);

                        strmessage = retunrMessageValidation(Estatus);

                            /*Estatus == (int)EstatusUsuario.Activo || Estatus == (int)EstatusUsuario.Inactivo ?
                            string.Format(Constantes.SendToUpdateRecord, Estatus == (int)EstatusUsuario.Activo ? "inactivación" : "activación") :
                            Estatus == (int)EstatusUsuario.Eliminacion ? string.Format(Constantes.SendToUpdateRecord, "eliminación"):
                            Estatus == (int)EstatusUsuario.EnvíoInactivación || Estatus == (int)EstatusUsuario.EnvíoActivación || Estatus == (int)EstatusUsuario.EnvíoEliminación ?
                            "Se aprobó el movimiento correctamente" : "";*/
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.Message });
                }
                return Json(new { success = true, message = strmessage });
            }
        }

        private int GetNewStatusValue(int CurrentStatus) {

            switch (CurrentStatus)
            {
                case (int)EstatusUsuario.Activo:
                    return (int)EstatusUsuario.EnvíoInactivación;

                case (int)EstatusUsuario.Inactivo:
                    return (int)EstatusUsuario.EnvíoActivación;

                case (int)EstatusUsuario.Eliminacion:
               // case (int)EstatusUsuario.Eliminado:
                    return (int)EstatusUsuario.EnvíoEliminación;

                case (int)EstatusUsuario.EnvíoEliminación:
                    return (int)EstatusUsuario.ProcesoAprobaciónEliminación;

                case (int)EstatusUsuario.EnvíoInactivación:
                    return (int)EstatusUsuario.ProcesoAprobaciónInactivación;

                case (int)EstatusUsuario.EnvíoActivación:
                    return (int)EstatusUsuario.ProcesoAprobaciónActivación;

                case (int)EstatusUsuario.EnvíoRecuperación:
                    return (int)EstatusUsuario.ProcesoAprobaciónActivación;

                case (int)EstatusUsuario.ProcesoAprobaciónInactivación:
                case (int)EstatusUsuario.ProcesoAprobaciónActivación:
                case (int)EstatusUsuario.ProcesoAprobaciónEliminación:
                    return CurrentStatus;
                default:
                    return CurrentStatus;
            }
        }

        private string GetMessageStatusValue(int Status) {
            string mensaje = "";
            switch (Status)
            {
                case (int)EstatusUsuario.Inactivo:
                case (int)EstatusUsuario.EnvíoActivación:
                case (int)EstatusUsuario.ProcesoAprobaciónActivación:
                    mensaje = string.Format(Constantes.MensajeCambioEstatus, "activado", _generals.UserName);
                    break;
                case (int)EstatusUsuario.Activo:
                case (int)EstatusUsuario.EnvíoInactivación:
                case (int)EstatusUsuario.ProcesoAprobaciónInactivación:
                    mensaje = string.Format(Constantes.MensajeCambioEstatus, "desactivado", _generals.UserName);
                    break;
                case (int)EstatusUsuario.Eliminacion:
                case (int)EstatusUsuario.Eliminado:
                case (int)EstatusUsuario.EnvíoEliminación:
                case (int)EstatusUsuario.ProcesoAprobaciónEliminación:
                    mensaje = string.Format(Constantes.MensajeCambioEstatus, "eliminado", _generals.UserName);
                    break;
            }
            return mensaje;
        }

        private int GetNewStatusValueForSuperAdmin(int CurrentStatus)
        {
            switch (CurrentStatus)
            {
                case (int)EstatusUsuario.Activo:
                case (int)EstatusUsuario.EnvíoInactivación:
                case (int)EstatusUsuario.ProcesoAprobaciónInactivación:
                    return (int)EstatusUsuario.Inactivo;

                case (int)EstatusUsuario.Inactivo:
                case (int)EstatusUsuario.EnvíoActivación:
                case (int)EstatusUsuario.ProcesoAprobaciónActivación:
                    return (int)EstatusUsuario.Activo;

                case (int)EstatusUsuario.Eliminacion:
                case (int)EstatusUsuario.Eliminado:
                case (int)EstatusUsuario.EnvíoEliminación:
                case (int)EstatusUsuario.ProcesoAprobaciónEliminación:
                    return (int)EstatusUsuario.Eliminado;

                default:
                    return CurrentStatus;
            }
        }

        private string retunrMessageValidation(int Status) {
            switch (Status)
            {
                case (int)EstatusUsuario.Eliminacion:
                    return string.Format(Constantes.SendToUpdateRecord, "eliminación");
                case (int)EstatusUsuario.Activo:
                    return string.Format(Constantes.SendToUpdateRecord, "inactivación");
                case (int)EstatusUsuario.Inactivo:
                    return string.Format(Constantes.SendToUpdateRecord, "activación");
                case (int)EstatusUsuario.EnvíoInactivación:
                case (int)EstatusUsuario.EnvíoActivación:
                case (int)EstatusUsuario.EnvíoEliminación:
                    return "Se aprobó el movimiento correctamente";
                default:
                    return string.Empty;
            }
        }
    }
}

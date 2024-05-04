
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

namespace SystemAdmin.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHostingEnvironment _environment;

        private IConfiguration Configuracion { get; }
        //private readonly ILogger<HomeController> _logger;
        public HomeController(IConfiguration config, IHostingEnvironment IHostingEnvironment)
        {
            _environment = IHostingEnvironment;
            Configuracion = config;
        }

        public IActionResult Index(int? id)
        {
            if (validateSession())
            {
                #region Configura Menú
                if (id != new int?())
                    HttpContext.Session.SetInt32(Sessions.IdModule, (int)id);
                 // HttpContext.Session.SetInt32(Sessions.IdApp,(int) id);
                ViewBag.IdUser = HttpContext.Session.GetInt32(Sessions.IdUser);
                string imgUp = HttpContext.Session.GetString(Sessions.ImagenUpload);
                ViewBag.ImagenBytesIlustrative = string.IsNullOrEmpty(imgUp) ? null : Convert.FromBase64String(imgUp);
                var mods = GetModulesAllowed(HttpContext.Session.GetInt32(Sessions.RolUser), (int)HttpContext.Session.GetInt32(Sessions.IdApp));
                ViewBag.Modules = mods;
                int IDMod = (int)HttpContext.Session.GetInt32(Sessions.IdModule);
                ViewBag.Alta = mods.FirstOrDefault(x => x.Id == IDMod).Alta;
                ViewBag.Modificacion = mods.FirstOrDefault(x => x.Id == IDMod).Modificacion;
                ViewBag.NombreUsuario = HttpContext.Session.GetString(Sessions.UserName);
                #endregion

                ViewBag.Profiles = GetProfiles();
                ViewBag.CatalogApple = GetCatalog();

                ViewBag.Resultado = TempData["resultado"];
                ViewBag.userSession = TempData["userSession"];
                ViewBag.MessageError = TempData["Error"];

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

        public bool validateSession() {
            if (HttpContext.Session.GetInt32(Sessions.IdUser) != null)
            {
                int IdUser =(int) HttpContext.Session.GetInt32(Sessions.IdUser);
                int IdApp = (int) HttpContext.Session.GetInt32(Sessions.IdApp);
                int RolUser = (int)HttpContext.Session.GetInt32(Sessions.RolUser);
                int IDMod = (int)HttpContext.Session.GetInt32(Sessions.IdModule);
                HttpContext.Session.SetInt32(Sessions.IdUser, (int)IdUser);
                HttpContext.Session.SetInt32(Sessions.IdApp, (int)IdApp);
                HttpContext.Session.SetInt32(Sessions.RolUser, (int)RolUser);
                HttpContext.Session.SetInt32(Sessions.IdModule, (int)IDMod);
                return true;
            }
            else
            {
                return false;
                //Redirect(DBSet.urlRedirect);
            }
        }

        public List<ModuleEntity> GetModulesAllowed(int? IdPerfil, int? IdApp)
        {
            using (ApplicationBusiness AppNegocio = new ApplicationBusiness())
            {
                var resultado = AppNegocio.GetModulesAllowed(IdPerfil, IdApp);
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
            if (validateSession())
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
                        entity.IdUsuarioMod = entity.Id == 0? new int?(): (int)HttpContext.Session.GetInt32(Sessions.IdUser);
                        string usuario = nego.saveUser(entity, ref Id);

                        if (Id > 0 && entity.Medidor != null && entity.Medidor.Count() > 0)
                        {
                            var medidor = nego.GetWaterMeterByIdUser(entity.Id);

                           var eliminar = (from t in medidor where !entity.Medidor.Any(x => x.Id==t.Id) select t).ToList();
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
                       
                        TempData["resultado"] = entity.Id == 0? "SaveSuccess" : "UpdateSuccess";
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
            else
            {
                return Redirect(DBSet.urlRedirect);
            }
        }
        #endregion

        #region JSon
        public JsonResult GetWaterMeter(int Numero)
        {
            if (validateSession())
            {
                using (WaterSystemBusiness nego = new WaterSystemBusiness())
                {
                    var resultado = nego.GetWaterMeter(Numero: Numero).FirstOrDefault();
                    return Json(new { data = resultado });
                }
            }
            else
            {
                 RedirectToAction(DBSet.urlRedirect);
                 return Json(new { data = new List<WaterMeterEntity>() });
            }
        }
        public JsonResult getDetailUserByIdUser(int IdUser)
        {
            validateSession();
            using (ApplicationBusiness nego = new ApplicationBusiness())
            {
                var resultado = nego.GetUserDetailById(IdUser);
                resultado.Medidor = nego.GetWaterMeterByIdUser(IdUser);

                return Json(new { data = resultado });
            }
        }

        #endregion
    }
}

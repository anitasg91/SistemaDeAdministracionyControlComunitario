using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SAyCC.Bussiness.Common;
using SAyCC.Bussiness.Login;
using SAyCC.Entities.Login;
using SAyCC.Login.Utilities;

using System.Drawing;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Web;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Authorization;

namespace SAyCC.Login.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IHostingEnvironment _environment;
        public ProfileController(IHostingEnvironment IHostingEnvironment)
        {
            _environment = IHostingEnvironment;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32(Sessions.IdUser) != null)
            {
                #region Configura Menú
                ViewBag.App = GetApplicationsAllowed((int)HttpContext.Session.GetInt32(Sessions.IdUser));
                ViewBag.IdUser = HttpContext.Session.GetInt32(Sessions.IdUser);
                #endregion

                ViewBag.Aplicacion = "Mi perfil";
                ViewBag.NombreUsuario = HttpContext.Session.GetString(Sessions.UserName);
                var user = HttpContext.Session.GetInt32(Sessions.IdUser);
                ViewBag.Datos = GetUserById((int)user).FirstOrDefault();
                string imgUp = ViewBag.Datos.ImagenUpload;
                ViewBag.Actualizacion = TempData["resultado"];
                ViewBag.ImagenBytesIlustrative = string.IsNullOrEmpty(imgUp) ? null : Convert.FromBase64String(imgUp);
                HttpContext.Session.SetString(Sessions.ImagenUpload, imgUp);

                return View();
            }
            else
            {
                return Redirect(DBSet.urlRedirect);
            }
        }

        public List<UserEntity> GetUserById(int user)
        {
            using (LoginBusiness UsrNegocio = new LoginBusiness())
            {
                var resultado = UsrNegocio.GetUser(user);
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
        [HttpPost]
        [AllowAnonymous]
        public ActionResult guardaImagenFileUpload(UserEntity UserInfo)
        {
            string mensaje = "";
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
                UserInfo.ImagenUpload = string.IsNullOrEmpty(imageUp) ? null : imageUp;
                UserInfo.Password = string.IsNullOrEmpty(UserInfo.Password)?null: GlobalBusines.GetSHA256(UserInfo.Password);
                using (LoginBusiness nego = new LoginBusiness())
                {
                    if (nego.UpdateInfoUser(UserInfo) == 1)
                    {
                        TempData["resultado"] = "ok";
                    }
                }
            }
            catch (Exception e)
            {
                mensaje = "¡Error!/" + e.Message + "/error";
            }
            return RedirectToAction("Index");
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

    }
}
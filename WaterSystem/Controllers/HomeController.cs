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
using SAyCC.WaterSystem.Utilities;
using System.Drawing;
using System.Net.Http.Headers;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using WaterSystem.Models;

namespace WaterSystem.Controllers
{
    [SessionValidator]
    public class HomeController : Controller
    {
        private readonly IGenerals _generals;
        private IConfiguration _Config { get; }
        public HomeController(IGenerals generals, IConfiguration config)
        {
            _generals = generals;
            _Config = config;
            DBSet.urlRedirect = _Config[Sessions.LoginApp];
        }

        public IActionResult Index(int? id)
        {
            #region Varibles de configuración para armar el Menú
            if (id != new int?())
            {
                //Importante guardar la paginaActual cada que se navegue
                HttpContext.Session.SetInt32(Sessions.CurrentPage, (int)id);
                if (!_generals.AllPagesByAppList.Any(_ => _.Id == id))
                {
                    return View(PartialViewEnum.PageNotAccess);
                }
            }
            #endregion

            ViewBag.tableUser = GetUsersList();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }        

        public List<UserEntity> GetUsersList()
        {
            using (ApplicationBusiness AppNegocio = new ApplicationBusiness())
            {
                var resultado = AppNegocio.GetUsers(true).Where(x=> x.Activo && x.Medidor.Any()).ToList();
                return resultado;
            }
        }
    }
}

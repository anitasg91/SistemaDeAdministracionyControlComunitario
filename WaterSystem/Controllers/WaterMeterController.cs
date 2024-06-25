
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAyCC.Bussiness.Common;
using SAyCC.Bussiness.DrinkingWaterSystem;
using SAyCC.Bussiness.Login;
using SAyCC.Bussiness.SystemAdmin;
using SAyCC.Entities.Common;
using SAyCC.Entities.Login;
using SAyCC.Entities.SystemAdmin;
using SAyCC.Entities.WaterSystem;
using SAyCC.WaterSystem.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WaterSystem.Controllers
{
    public class WaterMeterController : Controller
    {
        public static int IdManzana = 0;
        public IActionResult Index(int? id)
        {
            if (HttpContext.Session.GetInt32(Sessions.IdUser) != null)
            {
                #region Configura Menú
                if (id != new int?())
                    HttpContext.Session.SetInt32(Sessions.IdModule, (int)id);
                ViewBag.IdUser = HttpContext.Session.GetInt32(Sessions.IdUser);
                string imgUp = HttpContext.Session.GetString(Sessions.ImagenUpload);
                ViewBag.ImagenBytesIlustrative = string.IsNullOrEmpty(imgUp) ? null : Convert.FromBase64String(imgUp);
               // ViewBag.Alta = ViewBag.Modificacion = true;
               
               // var mods = GetModulesAllowed(HttpContext.Session.GetInt32(Sessions.RolUser), (int)HttpContext.Session.GetInt32(Sessions.IdApp));
                var mods = GetModulesAllowed(HttpContext.Session.GetInt32(Sessions.IdUser), (int)HttpContext.Session.GetInt32(Sessions.IdApp));
                ViewBag.Modules = mods;
                int IDMod = (int)HttpContext.Session.GetInt32(Sessions.IdModule);
                ViewBag.Alta = mods.FirstOrDefault(x => x.Id == IDMod).Alta;
                ViewBag.Modificacion = mods.FirstOrDefault(x => x.Id == IDMod).Modificacion;

                ViewBag.NombreUsuario = HttpContext.Session.GetString(Sessions.UserName);
                #endregion

                ViewBag.resultado = TempData["resultado"];
                ViewBag.MensajeErr = TempData["MensajeErr"];

                ViewBag.Apple = GetPeriod((int)Enumerador.CatalogType.Apple);
                ViewBag.Period = GetPeriod((int)Enumerador.CatalogType.Month);
                ViewBag.tableMedidor = GetWaterMeter().Where(x=> (IdManzana == 0) ?x.IdManzana == 1: x.IdManzana == IdManzana).ToList();
                ViewBag.BtnAgregar = false;
                using (WaterSystemBusiness nego = new WaterSystemBusiness())
                {
                    var res = nego.GetWaterMeter().Where(x => x.IdManzana == 1).ToList();
                    if (res.Any())
                    {
                        var fechaActual = DateTime.Now.Month;
                        var fecha = res.FirstOrDefault().FechaLectura.Month;
                        ViewBag.BtnAgregar = fecha == fechaActual;
                    }
                }

                return View();
            }
            else
            {
                return Redirect(DBSet.urlRedirect);
            }
        }
        #region configura menú
        public void validateSession()
        {
            if (HttpContext.Session.GetInt32(Sessions.IdUser) != null)
            {
                int IdUser = (int)HttpContext.Session.GetInt32(Sessions.IdUser);
                int IdApp = (int)HttpContext.Session.GetInt32(Sessions.IdApp);
                int RolUser = (int)HttpContext.Session.GetInt32(Sessions.RolUser);
                HttpContext.Session.SetInt32(Sessions.IdUser, (int)IdUser);
                HttpContext.Session.SetInt32(Sessions.IdApp, (int)IdApp);
                HttpContext.Session.SetInt32(Sessions.RolUser, (int)RolUser);
            }
            else
            {
                Redirect(DBSet.urlRedirect);
            }
        }
        public bool validateSessionWithReturn()
        {
            if (HttpContext.Session.GetInt32(Sessions.IdUser) != null)
            {
                int IdUser = (int)HttpContext.Session.GetInt32(Sessions.IdUser);
                int IdApp = (int)HttpContext.Session.GetInt32(Sessions.IdApp);
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
        #endregion

        public List<CatalogEntity> GetPeriod(int IDCatalog)
        {
            validateSession();
            using (ApplicationBusiness AppNegocio = new ApplicationBusiness())
            {
                var resultado = AppNegocio.GetCatalog(IDCatalog);
                return resultado;
            }
        }
        public CatalogEntityWS GetCatalog(int IDCatalog)
        {
            validateSession();
            using (WaterSystemBusiness AppNegocio = new WaterSystemBusiness())
            {
                var resultado = AppNegocio.GetCatalog(IDCatalog);
                return resultado;
            }
        }
        public List<WaterMeterEntity> GetWaterMeter()
        {
            validateSession();
            using (WaterSystemBusiness nego = new WaterSystemBusiness())
            {
                return nego.GetWaterMeter();
            }
        }

        public JsonResult GetWaterMeterJson(int idManzana, bool AllSearch = false)
        {
            validateSession();
            using (WaterSystemBusiness nego = new WaterSystemBusiness())
            {
                List<WaterMeterEntity> res;
                try
                {
                    if(AllSearch)
                    res = nego.GetWaterMeter().Where(x => x.IdManzana == idManzana).ToList();
                    else
                     res = nego.GetWaterMeter().Where(x=> x.IdManzana == idManzana && x.Activo && x.FechaBaja == null && x.IdTitular != new int?()).ToList();

                }
                catch (Exception)
                {
                    res = new List<WaterMeterEntity>();
                    throw;
                }
                return Json(new { data = res });
            }
        }

        #region HTTPOST
        [HttpPost]
        public IActionResult saveNewReading(WaterMeterList list)
        {
            if (validateSessionWithReturn())
            {
                try
                {
                    using (ApplicationBusiness nego = new ApplicationBusiness())
                    {
                         list.Medidor.ForEach(x => {
                             WaterMeterEntity entity = new WaterMeterEntity() { 
                             Id = x.Id,
                             LecturaAnterior = x.LecturaAnterior,
                             LecturaActual = x.LecturaActual,
                             IdManzana = list.IdManzana
                             };
                             nego.SaveWaterMeter(entity);
                         });
                     // var comprobante =  armaComprobanteAndDesglose(list);
                        IdManzana = list.IdManzana;
                        TempData["resultado"] = "UpdateSuccess";
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

        public List<VoucherEntity> armaComprobanteAndDesglose(WaterMeterList wMList)
        {
            List<VoucherEntity> lista = new List<VoucherEntity>();

            foreach (var item in wMList.Medidor)
            {
                VoucherEntity comprobante = new VoucherEntity();
                comprobante.IdMedidor = long.Parse(item.Numero);
                comprobante.IdUsuario = 0;
                comprobante.IdManzana = item.IdManzana;
                comprobante.IdMes = DateTime.Now.Month;
                comprobante.Estado = "Hidalgo";
                comprobante.Fecha = DateTime.Now;
                comprobante.Folio = "";
                comprobante.Periodo = DateTime.Now.ToString("MMMM").ToUpper();
                comprobante.LecturaAnt = (int)item.LecturaAnterior;
                comprobante.LecturaAct = (int)item.LecturaActual;
                comprobante.TotalImUsado = (int)(item.LecturaActual - item.LecturaAnterior);
                comprobante.Desglose = ConfiguraDesglose(comprobante.TotalImUsado);
                comprobante.TotalPagado = comprobante.Desglose.Sum(x=>x.TotalPagar);
                lista.Add(comprobante);
            }

            return lista;
        }

        public List<BreakdownEntity> ConfiguraDesglose(int TotalMUsado) {
            List<BreakdownEntity> desglose = new List<BreakdownEntity>();
            using (WaterSystemBusiness nego = new WaterSystemBusiness())
            {
                var tipoLectura = nego.GetCatalog((int)Enumerador.CatalogTypeWaterSystem.TipoLectura).TipoLectura;

                var restante = new int?();
                int MetrosUsados = 0;
                foreach (var lect in tipoLectura)
                {
                    BreakdownEntity des = new BreakdownEntity();

                    if (desglose.Count() == 0)
                    {
                        MetrosUsados = TotalMUsado<(int)lect.Limite? TotalMUsado: (int)lect.Limite;//(int)(TotalMUsado - lect.Limite) == 0 ? TotalMUsado : (int)(TotalMUsado - lect.Limite);
                        if (MetrosUsados >= lect.Inicial && MetrosUsados <= lect.Limite)
                        {
                            des.Identificador = lect.Id;
                            des.MetrosUsados = MetrosUsados;
                            des.TotalPagar = lect.Precio;
                        }
                        desglose.Add(des);
                        restante = TotalMUsado - MetrosUsados;
                    }
                    else if (lect.Limite == new int?() && restante > 0) {
                        des.Identificador = lect.Id;
                        des.MetrosUsados = MetrosUsados;
                        des.TotalPagar =(int)restante * lect.Precio;
                        desglose.Add(des);
                        restante = 0;
                    }
                    else
                    {
                        if (restante > 0)
                        {
                           // MetrosUsados = (int)(lect.Limite - lect.Inicial);
                            MetrosUsados = (int)(restante < (lect.Limite - lect.Inicial) ? restante : (lect.Limite - lect.Inicial));

                            des.Identificador = lect.Id;
                            des.MetrosUsados = MetrosUsados;
                            des.TotalPagar = MetrosUsados * lect.Precio;
                            desglose.Add(des);
                            restante = restante - MetrosUsados;
                        }
                    }
                }

                return desglose;
            }
        }
        #endregion
    }
}

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
using System.Threading.Tasks;

namespace WaterSystem.Controllers
{
    [SessionValidator]
    public class WaterMeterReadingController : Controller
    {
        //public static int IdManzana = 0;
        private readonly IGenerals _generals;
        public WaterMeterReadingController(IGenerals generals)
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
                if (!_generals.AllPagesByAppList.Any(_ => _.Id == id))
                {
                    return View(PartialViewEnum.PageNotAccess);
                }
            }
            #endregion
            ViewBag.resultado = TempData["resultado"];
            ViewBag.MensajeErr = TempData["MensajeErr"];

            

            var IdManzana = _generals.HasRol(roles.Juez) && !_generals.HasRol(roles.Presidente_Agua) ? _generals.IdManzana : 1;
            var MedidorList = GetWaterMeter(IdManzana);
            var WaterMeterSaved = GetWaterMeter(IdManzana).Count(x=>x.FechaLectura.Month == DateTime.Now.Month);
            WaterMeterResponse response = new WaterMeterResponse();
            response.IdManzana = IdManzana;
            response.IdPeriodo = DateTime.Now.Month;

            response.Manzana = GetPeriod((int)Enumerador.CatalogType.Apple);
            response.Mes = GetPeriod((int)Enumerador.CatalogType.Month);
            response.Medidor = MedidorList;
            response.CrearComprobante = MedidorList.Count() == WaterMeterSaved;
            //ViewBag.Apple = GetPeriod((int)Enumerador.CatalogType.Apple);
            //ViewBag.Period = GetPeriod((int)Enumerador.CatalogType.Month);
            //ViewBag.tableMedidor = GetWaterMeter().Where(x => (IdManzana == 0) ? x.IdManzana == 1 : x.IdManzana == IdManzana).ToList();
            //ViewBag.BtnAgregar = false;
            //using (WaterSystemBusiness nego = new WaterSystemBusiness())
            //{
            //    var res = nego.GetWaterMeter().Where(x => x.IdManzana == 1).ToList();
            //    if (res.Any())
            //    {
            //        var fechaActual = DateTime.Now.Month;
            //        var fecha = res.FirstOrDefault().FechaLectura.Month;
            //        ViewBag.BtnAgregar = fecha == fechaActual;
            //    }
            //}

            return View(response);

            
        }

        public List<CatalogEntity> GetPeriod(int IDCatalog)
        {
            using (ApplicationBusiness AppNegocio = new ApplicationBusiness())
            {
                var resultado = AppNegocio.GetCatalog(IDCatalog);
                return resultado;
            }
        }

        //public CatalogEntityWS GetCatalog(int IDCatalog)
        //{
        //    using (WaterSystemBusiness AppNegocio = new WaterSystemBusiness())
        //    {
        //        var resultado = AppNegocio.GetCatalog(IDCatalog);
        //        return resultado;
        //    }
        //}

        [HttpGet]
        public async Task<IActionResult> GetTable(int idManzana)
        {
            List<WaterMeterEntity> res = GetWaterMeter(idManzana);
            return PartialView(PartialViewEnum.Table, res);
        }


        public List<WaterMeterEntity> GetWaterMeter(int idManzana)
        {
            using (WaterSystemBusiness nego = new WaterSystemBusiness())
            {
                var response = nego.GetWaterMeter().Where(x=>x.Activo && x.IdTitular != null && x.FechaBaja == null
                && (_generals.HasRol(roles.Juez) && !_generals.HasRol(roles.Presidente_Agua) ? x.IdManzana == _generals.IdManzana ://Si es juez y no es admin, obtiene los medidores de su manzana
                    x.IdManzana == idManzana)
                ).ToList();

                return response;
            }
        }

        //public JsonResult GetWaterMeterJson(int idManzana, bool AllSearch = false)
        //{
        //    using (WaterSystemBusiness nego = new WaterSystemBusiness())
        //    {
        //        List<WaterMeterEntity> res;
        //        try
        //        {
        //            if(AllSearch)
        //            res = nego.GetWaterMeter().Where(x => x.IdManzana == idManzana).ToList();
        //            else
        //             res = nego.GetWaterMeter().Where(x=> x.IdManzana == idManzana && x.Activo && x.FechaBaja == null && x.IdTitular != new int?()).ToList();

        //        }
        //        catch (Exception)
        //        {
        //            res = new List<WaterMeterEntity>();
        //            throw;
        //        }
        //        return Json(new { data = res });
        //    }
        //}

        #region HTTPOST

        [HttpPost]
        public IActionResult SaveListReading(WaterMeterResponse model)
        {
            DateTime fechaLectura = Convert.ToDateTime(string.Concat(DateTime.Now.Day,"/", model.IdPeriodo, "/", DateTime.Now.Year));
            try
            {
                using (ApplicationBusiness nego = new ApplicationBusiness())
                {
                    model.Medidor.ForEach(x =>
                    {
                        WaterMeterEntity entity = new WaterMeterEntity()
                        {
                            Id = x.Id,
                            LecturaAnterior = x.LecturaAnterior,
                            LecturaActual = x.LecturaActual,
                            IdManzana = model.IdManzana,
                            FechaLectura = fechaLectura
                        };
                        nego.SaveWaterMeter(entity);
                    });
                    //var comprobante = armaComprobanteAndDesglose(list);
                    //IdManzana = model.IdManzana;
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


        [HttpGet]
        public IActionResult SaveNewReading(WaterMeterToEdit Medidor)
        {
            try
            {
                using (ApplicationBusiness nego = new ApplicationBusiness())
                {
                    //Medidor.ForEach(x =>
                    //{
                    //    WaterMeterEntity entity = new WaterMeterEntity()
                    //    {
                    //        Id = x.Id,
                    //        LecturaAnterior = x.LecturaAnterior,
                    //        LecturaActual = x.LecturaActual,
                    //        IdManzana = x.IdManzana
                    //    };
                    //    nego.SaveWaterMeter(entity);
                    //});
                    // var comprobante =  armaComprobanteAndDesglose(list);
                   // IdManzana = Medidor.FirstOrDefault().IdManzana;
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
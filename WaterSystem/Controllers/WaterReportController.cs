
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
    [SessionValidator]
    public class WaterReportController : Controller
    {
        private readonly IGenerals _generals;
        public WaterReportController(IGenerals generals)
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

            ViewBag.Period = GetPeriod((int)Enumerador.CatalogType.Month);
            ViewBag.Apple = GetPeriod((int)Enumerador.CatalogType.Apple);
            ViewBag.PaymentVoucher = GetPaymentVoucher(new int?());
            
            return View();
        }

        public List<CatalogEntity> GetPeriod(int IDCatalog)
        {
            using (ApplicationBusiness AppNegocio = new ApplicationBusiness())
            {
                var resultado = AppNegocio.GetCatalog(IDCatalog);
                return resultado;
            }
        }
         public List<VoucherEntity> GetPaymentVoucher(int? Id)
         {
             using (WaterSystemBusiness AppNegocio = new WaterSystemBusiness())
             {
                 var resultado = AppNegocio.GetPaymentVoucher(Id);
                 return resultado;
             }
         }

        public JsonResult GetPaymentVoucherJson(int Id)
        {
            using (WaterSystemBusiness nego = new WaterSystemBusiness())
            {
                VoucherEntity res;
                try
                {
                    res = nego.GetPaymentVoucher(Id).FirstOrDefault();
                    res.Desglose = GetBreakdowns(res.Id);
                    res.TotalPagarAdeudo = res.Desglose.Sum(x=>x.TotalPagar);
                    res.Grafica = GetGraphicData(res.Id);
                }
                catch (Exception)
                {
                    res = new VoucherEntity();
                    throw;
                }
                return Json(new { data = res });
            }
        }

        public List<BreakdownEntity> GetBreakdowns(int idComprobante) {

            using (WaterSystemBusiness nego = new WaterSystemBusiness())
            {
                    List<BreakdownEntity> lista = new List<BreakdownEntity>();
                try
                {
                    var catTipoLectura = GetCatalog((int)Enumerador.CatalogTypeWaterSystem.TipoLectura);
                    var desgloseByIdComprobante = nego.GetbreakdownByIdComprobante(idComprobante);

                    foreach (var lect in catTipoLectura.TipoLectura)
                    {
                        BreakdownEntity ent = new BreakdownEntity();

                        var desgloseOriginal = desgloseByIdComprobante.FirstOrDefault(x => x.Identificador == lect.Id);

                        if (desgloseOriginal != null)
                        {
                            ent.Identificador = desgloseOriginal.Identificador;
                            ent.Concepto = desgloseOriginal.Concepto;
                            ent.MetrosUsados = desgloseOriginal.MetrosUsados;
                            ent.TotalPagar = desgloseOriginal.TotalPagar;
                        }
                        else
                        {
                            ent.Identificador = lect.Id;
                            ent.Concepto = lect.Descripcion;
                            ent.MetrosUsados = 0;
                            ent.TotalPagar = 0;
                        }
                        lista.Add(ent);
                    }

                    desgloseByIdComprobante.Where(x => x.Identificador == 0).ToList().ForEach(x =>
                    {
                        lista.Add(
                                new BreakdownEntity()
                                {
                                    Identificador = lista.Count() + 1,
                                    Concepto = x.Concepto,
                                    MetrosUsados = x.MetrosUsados,
                                    TotalPagar = x.TotalPagar
                                });
                    });
                  
                }
                catch (Exception)
                {
                    throw;
                }
                return lista;
            }

        }
        public CatalogEntityWS GetCatalog(int IDCatalog)
        {
            using (WaterSystemBusiness AppNegocio = new WaterSystemBusiness())
            {
                var resultado = AppNegocio.GetCatalog(IDCatalog);
                return resultado;
            }
        }
        public List<GraphicDataEntity> GetGraphicData(int IDCatalog)
        {
            using (WaterSystemBusiness AppNegocio = new WaterSystemBusiness())
            {
                var resultado = AppNegocio.GetGraphicData(IDCatalog);
                return resultado;
            }
        }
        


    }
}
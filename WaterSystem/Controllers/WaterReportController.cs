
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
    public class WaterReportController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32(Sessions.IdUser) != null)
            {
                #region Configura Menú
                ViewBag.IdUser = HttpContext.Session.GetInt32(Sessions.IdUser);
               // ViewBag.Modules = GetModulesAllowed(HttpContext.Session.GetInt32(Sessions.RolUser), HttpContext.Session.GetInt32(Sessions.IdApp));
                ViewBag.Modules = GetModulesAllowed(HttpContext.Session.GetInt32(Sessions.IdUser), HttpContext.Session.GetInt32(Sessions.IdApp));
                string imgUp = HttpContext.Session.GetString(Sessions.ImagenUpload);
                ViewBag.ImagenBytesIlustrative = string.IsNullOrEmpty(imgUp) ? null : Convert.FromBase64String(imgUp);
                ViewBag.Alta = ViewBag.Modificacion = true;
                ViewBag.NombreUsuario = HttpContext.Session.GetString(Sessions.UserName);
                #endregion

                ViewBag.resultado = TempData["resultado"];
                ViewBag.MensajeErr = TempData["MensajeErr"];

                ViewBag.Period = GetPeriod((int)Enumerador.CatalogType.Month);
                ViewBag.Apple = GetPeriod((int)Enumerador.CatalogType.Apple);
                ViewBag.PaymentVoucher = GetPaymentVoucher(new int?());
                
                // ViewBag.Applications = GetApplications();
                // ViewBag.Catalogs = GetCatalog((int)Enumerador.CatalogTypeWaterSystem.TipoLectura);
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
         public List<VoucherEntity> GetPaymentVoucher(int? Id)
         {
            validateSession();
             using (WaterSystemBusiness AppNegocio = new WaterSystemBusiness())
             {
                 var resultado = AppNegocio.GetPaymentVoucher(Id);
                 return resultado;
             }
         }

        public JsonResult GetPaymentVoucherJson(int Id)
        {
            validateSession();
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

            validateSession();
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
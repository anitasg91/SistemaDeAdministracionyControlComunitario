
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
using static SAyCC.Entities.Common.Enumerador;

namespace WaterSystem.Controllers
{
    [SessionValidator]
    public class WaterMeterController : Controller
    {
        public static int IdManzana = 0;
        private readonly IGenerals _generals;
        public WaterMeterController(IGenerals generals)
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

            WaterMeterResponse response = new WaterMeterResponse();

            response.Manzana = GetPeriod((int)Enumerador.CatalogType.Apple);

            response.Medidor = GetWaterMeter().Where(x => 
            _generals.HasRol(roles.Juez) && !_generals.HasRol(roles.Presidente_Agua) ? x.IdManzana == _generals.IdManzana ://Si es juez y no es admin, obtiene los medidores de su manzana
            x.IdManzana == 1 ).ToList();

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
       
        public List<WaterMeterEntity> GetWaterMeter()
        {
            using (WaterSystemBusiness nego = new WaterSystemBusiness())
            {
                return nego.GetWaterMeter();
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetTable(int idManzana)
        {
                List<WaterMeterEntity> res = new List<WaterMeterEntity>();
            using (WaterSystemBusiness nego = new WaterSystemBusiness())
            {
                try
                {
                    res = nego.GetWaterMeter().Where(x => x.IdManzana == idManzana).ToList();

                    //if (AllSearch)
                    //    res = nego.GetWaterMeter().Where(x => x.IdManzana == idManzana).ToList();
                    //else
                    //    res = nego.GetWaterMeter().Where(x => x.IdManzana == idManzana && x.Activo && x.FechaBaja == null && x.IdTitular != new int?()).ToList();

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return PartialView(PartialViewEnum.Table, res);
        }

        [HttpGet]
        public async Task<IActionResult> GetModalAction(int IdWaterMeter, ModifyTypeWaterMeter option)
        {
            ModalResponse modal = new ModalResponse()
            {
                Id = IdWaterMeter,
                Tipo = (int)option
            };
            switch (option)
            {
                case ModifyTypeWaterMeter.Disassociate:
                    modal.Titulo = "Desasociar usuario";
                    modal.Icono = "fas fa-user-slash fa-lg";
                    modal.Descripcion = "¿Esta seguro que desea desasociar el usuario? Los recibos de agua ya no saldrán a su nombre.";
                    break;
                case ModifyTypeWaterMeter.Unsubscribe:
                    modal.Titulo = "Dar de baja medidor";
                    modal.Icono = "fas fa-power-off fa-lg";
                    modal.Descripcion = "¿Esta seguro que desea Dar de baja el medidor permanentemente? Ya no podrán generar movimientos pero su historial se seguirá mostrando.";
                    break;
                case ModifyTypeWaterMeter.Deactivate:
                    modal.Titulo = "Desactivar medidor";
                    modal.Icono = "fas fa-ban fa-lg";
                    modal.Descripcion = "¿Esta seguro que desea desactivar el medidor? ";
                    break;
                case ModifyTypeWaterMeter.associate:
                    modal.Titulo = "Asociar usuario";
                    modal.Icono = "fas fa-user fa-lg";
                    modal.Descripcion = "¿Esta seguro que desea desasociar el usuario? Los recibos de agua ya no saldrán a su nombre.";
                    break;
                case ModifyTypeWaterMeter.Subscribe:
                    modal.Titulo = "Dar alta medidor";
                    modal.Icono = "fas fa-toggle-on fa-lg";
                    modal.Descripcion = "¿Esta seguro que desea dar el alta el medidor? Los recibos de agua volverán a salir a nombre del último usuario asignado.";
                    break;
                default:
                    modal.Titulo = "Activar medidor";
                    modal.Icono = "fas fa-unlock fa-lg";
                    modal.Descripcion = "¿Esta seguro que desea activar el medidor? ";
                    break;
            }

            return PartialView(PartialViewEnum.ModalActions, modal);
        }

    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SAyCC.Bussiness.Common;
using SAyCC.Bussiness.DrinkingWaterSystem;
using SAyCC.Bussiness.Login;
using SAyCC.Bussiness.SystemAdmin;
using SAyCC.Entities.Common;
using SAyCC.Entities.Login;
using SAyCC.Entities.SystemAdmin;
using SAyCC.Entities.WaterSystem;
using SAyCC.SystemAdmin.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using static SAyCC.Entities.Common.Enumerador;

namespace SystemAdmin.Controllers
{
    [SessionValidator]
    public class CatalogManagerController : Controller
    {
        private readonly IGenerals _generals;
        public CatalogManagerController(IGenerals generals)
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

            ViewBag.resultado = TempData["resultado"];
            ViewBag.MensajeErr = TempData["MensajeErr"];
            ViewBag.CatalogManager = GetCatalogManager();

            int[] catEnums = new int[] { (int)PermisoEn.Administrar_Manzana, (int)PermisoEn.Administrar_Mes, (int)PermisoEn.Administrar_Permisos };
            var ppc = _generals.PermissionPageCurrent.FirstOrDefault(x => catEnums.Contains(x.Id));
            var IdPermiso = ppc != null ? ppc.Id : 0;
            var catalogs = GetCatalogoFirst(IdPermiso);
            
            return View(catalogs);
        }
        private CatalogResponse GetCatalogoFirst(int IdPermiso) {
            CatalogResponse catalogs = new CatalogResponse();
            switch (IdPermiso)
            {
                case (int)PermisoEn.Administrar_Manzana:
                    catalogs.Catalog = GetCatalog((int)Enumerador.CatalogType.Apple);
                    break;
                case (int)PermisoEn.Administrar_Mes:
                    catalogs.Catalog = GetCatalog((int)Enumerador.CatalogType.Month);
                    break;
                case (int)PermisoEn.Administrar_Permisos:
                    catalogs.PermissionCatalog = GetPermission();
                    break;
            }
            return catalogs;
        }
        public List<CatalogEntity> GetCatalogManager()
        {
            using (ApplicationBusiness AppNegocio = new ApplicationBusiness())
            {
                var resultado = AppNegocio.GetCatalogManager(_generals.IdApplication);
                return resultado;
            }
        }
        public List<CatalogEntity> GetCatalog(int IdCatalogType, int? Id = new int? () )
        {
            using (ApplicationBusiness AppNegocio = new ApplicationBusiness())
            {
                var resultado = AppNegocio.GetCatalog(IdCatalogType, Id);
                return resultado;
            }
        }
        public List<PermissionCatalogEntity> GetPermission(int? IdPermiso = new int?())
        {
            using (ApplicationBusiness nego = new ApplicationBusiness())
            {
                var resultado = nego.GetPermissionCatalog(IdPermiso);
                return resultado;
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCatalogPartialView(int IdCatalogType)
        {
            var result = GetCatalog(IdCatalogType);
            return PartialView(PartialViewEnum.BlockMonthTable, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetPermissionPartialView()
        {
            var resp = GetPermission();
            return PartialView(PartialViewEnum.PermissionTable, resp);
        }

        [HttpGet]
        public async Task<IActionResult> GetNewPartialView(int IdCatalogType, bool EditMod, int? Id)
        {
            string partialView = string.Empty;
            switch (IdCatalogType)
            {
                case (int)CatalogType.Apple:
                case (int)CatalogType.Month:
                    CatalogEntity obj = new CatalogEntity();
                    obj.IdCatalogType = IdCatalogType;
                    if (EditMod)
                    {
                        obj = GetCatalog(IdCatalogType, Id).FirstOrDefault();
                    }
                    return PartialView(PartialViewEnum.CatalogNew, obj);
                case (int)CatalogType.Permission:
                    PermissionCatalogEntity objp = new PermissionCatalogEntity();
                    if (EditMod)
                    {
                        objp = GetPermission(Id).FirstOrDefault();
                    }
                    return PartialView(PartialViewEnum.PermissionsNew, objp);
                default:
                    CatalogEntity def = new CatalogEntity();
                    return PartialView(PartialViewEnum.CatalogNew, def);
            }

        }

        
        [HttpGet]
        public async Task<IActionResult> SaveInfoCatalogJson(string model, int IdCatalogType)
        {
            using (ApplicationBusiness nego = new ApplicationBusiness())
            {
                switch (IdCatalogType)
                {
                    case (int)CatalogType.Apple:
                    case (int)CatalogType.Month:
                        var jsn = JsonConvert.DeserializeObject<CatalogEntity>(model);
                        nego.SaveCatalog(IdCatalogType, jsn.Id, jsn.Descripcion);
                        var result = GetCatalog(IdCatalogType);
                        return PartialView(PartialViewEnum.BlockMonthTable, result);

                    case (int)CatalogType.Permission:
                        var obj = JsonConvert.DeserializeObject<PermissionCatalogEntity>(model);
                        nego.SavePermissionCatalog(obj);
                        var resp = GetPermission();
                        return PartialView(PartialViewEnum.PermissionTable, resp);
                    default:
                        CatalogEntity def = new CatalogEntity();
                        return PartialView(PartialViewEnum.BlockMonthTable, new List<CatalogEntity>());
                }
            }

        }

        [HttpGet]
        public async Task<IActionResult> GetDeletePartialView(int IdCatalogType, int IdDelete)
        {
            PersonalizeModalDelete delete = new PersonalizeModalDelete();
            delete.Id = IdDelete;

            switch (IdCatalogType)
            {
                case (int)CatalogType.Apple:
                    delete.Titulo = string.Format(ConfigModalDelete.Titulo, ConfigModalDelete.Manzana);
                    delete.Descripcion = string.Format(ConfigModalDelete.Descripcion, " la " + ConfigModalDelete.Manzana, " la " + ConfigModalDelete.Manzana);
                    break;
                case (int)CatalogType.Month:
                    delete.Titulo = string.Format(ConfigModalDelete.Titulo, ConfigModalDelete.Mes);
                    delete.Descripcion = string.Format(ConfigModalDelete.Descripcion, " el " + ConfigModalDelete.Mes, " el " + ConfigModalDelete.Mes);
                    break;
                case (int)CatalogType.Permission:
                    delete.Titulo = string.Format(ConfigModalDelete.Titulo, ConfigModalDelete.Permisos);
                    delete.Descripcion = string.Format(ConfigModalDelete.Descripcion, " el " + ConfigModalDelete.Permisos, " el " + ConfigModalDelete.Permisos);
                    break;
            }
            return PartialView(PartialViewEnum.DeletePartialView, delete);
        }

        public JsonResult DeleteCatalog(int IDCatalog, int IdDelete)
        {
            using (ApplicationBusiness nego = new ApplicationBusiness())
            {
                List<CatalogEntity> resultado;
                try
                {
                    nego.DeleteCatalog(IDCatalog, IdDelete);
                    resultado = nego.GetCatalog(IDCatalog);
                }
                catch (Exception Ex)
                {
                    resultado = new List<CatalogEntity>() { new CatalogEntity() { Id = 0, Descripcion = Ex.Message } };
                    return Json(new { data = resultado });
                }

                return Json(new { data = resultado });
            }
        }

        public JsonResult ActivateCatalog(int IdLock, int Activar)
        {
            using (ApplicationBusiness nego = new ApplicationBusiness())
            {
                CatalogEntity resultado = new CatalogEntity();
                try
                {
                    nego.LockPermission(IdLock, Convert.ToBoolean(Activar));
                    resultado.Id = 1;
                }
                catch (Exception Ex)
                {
                    resultado = new CatalogEntity() { Id = 0, Descripcion = Ex.Message };
                    return Json(new { data = resultado });
                }

                return Json(new { data = resultado });
            }
        }


        
        /*public JsonResult GetPermissionCatalogJson(int? IdPermiso = new int?())
        {
            using (ApplicationBusiness nego = new ApplicationBusiness())
            {
                var resultado = nego.GetPermissionCatalog(IdPermiso);
                return Json(new { data = resultado });
            }
        }*/



    }
}
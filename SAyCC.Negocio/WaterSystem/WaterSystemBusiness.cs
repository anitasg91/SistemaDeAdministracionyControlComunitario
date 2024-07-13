using SAyCC.Bussiness.Common;
using SAyCC.Data.Repository;
using SAyCC.Entities.WaterSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAyCC.Bussiness.DrinkingWaterSystem
{
    public class WaterSystemBusiness: IDisposable
    {
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<WaterMeterEntity> GetWaterMeter(int? Id = new int?(), int? Numero = new int?())
        {
            using (WaterSystemRepository objDBD = new WaterSystemRepository())
            {
                var Result = objDBD.GetWaterMeter(DBSet.DBcnn, Id, Numero);
                objDBD.Dispose();
                return Result.OrderByDescending(x=>x.Id).ToList();
            }
        }

        #region Catalogos
        public CatalogEntityWS GetCatalog(int IDCatalog, int? Id = new int?())
        {
            using (WaterSystemRepository objDBD = new WaterSystemRepository())
            {
                var Result = objDBD.GetCatalog(DBSet.DBcnn, IDCatalog, Id);
                objDBD.Dispose();
                return Result;
            }
        }
        //public void DeleteCatalog(int IDCatalog, int Id)
        //{
        //    using (AministrationRepository objDBD = new AministrationRepository())
        //    {
        //        objDBD.DeleteCatalog(DBSet.DBcnn, IDCatalog, Id);
        //        objDBD.Dispose();
        //    }
        //}
        //public void SaveCatalog(int IDCatalog, int Id, string Descripcion)
        //{
        //    using (AministrationRepository objDBD = new AministrationRepository())
        //    {
        //        objDBD.SaveCatalog(DBSet.DBcnn, IDCatalog, Id, Descripcion);
        //        objDBD.Dispose();
        //    }
        //}
        #endregion

        public List<VoucherEntity> GetPaymentVoucher(int? Id = new int?())
        {
            using (WaterSystemRepository objDBD = new WaterSystemRepository())
            {
                var Result = objDBD.GetPaymentVoucher(DBSet.DBcnn,Id);
                objDBD.Dispose();
                return Result;
            }
        }
        public List<BreakdownEntity> GetbreakdownByIdComprobante(int IdComprobante)
        {
            using (WaterSystemRepository objDBD = new WaterSystemRepository())
            {
                var Result = objDBD.GetbreakdownByIdComprobante(DBSet.DBcnn, IdComprobante);
                objDBD.Dispose();
                return Result;
            }
        }

        public List<GraphicDataEntity> GetGraphicData(int IdComprobante)
        {
            using (WaterSystemRepository objDBD = new WaterSystemRepository())
            {
                var Result = objDBD.GetGraphicData(DBSet.DBcnn, IdComprobante);
                objDBD.Dispose();
                return Result;
            }
        }
    }
}

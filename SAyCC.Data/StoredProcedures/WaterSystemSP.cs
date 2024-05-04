using System;
using System.Collections.Generic;
using System.Text;

namespace SAyCC.Data.StoredProcedures
{
    public static class WaterSystemSP
    {
        public static string GetWaterMeter = "[AguaPotable].[spGetWaterMeter]";
        public static string GetCatalog = "[AguaPotable].[GetCatalog]";
        public static string GetReceiptByParameter = "[AguaPotable].[spGetReceiptByParameter]";
        public static string GetbreakdownByIdComprobante = "[AguaPotable].[spGetbreakdownByIdComprobante]";
        public static string GetGraphicData = "[AguaPotable].[spGetGraphicData]";
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace SAyCC.Entities.WaterSystem
{
    public class GraphicDataEntity
    {
        public int IdComprobante { get; set; }
        public int TotalMUsados { get; set; }
        public decimal TotalPagado { get; set; }
        public string Fecha { get; set; }
    }
}

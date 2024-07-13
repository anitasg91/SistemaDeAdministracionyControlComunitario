using SAyCC.Entities.SystemAdmin;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAyCC.Entities.WaterSystem
{
    public class WaterMeterResponse
    {
        public int IdManzana { get; set; } = 0;
        public int IdPeriodo { get; set; } = 0;
        public List<CatalogEntity> Manzana { get; set; } = new List<CatalogEntity>();
        public List<CatalogEntity> Mes { get; set; } = new List<CatalogEntity>();
        public List<WaterMeterEntity> Medidor { get; set; } = new List<WaterMeterEntity>();
        public bool CrearComprobante { get; set; } = false;

    }

}

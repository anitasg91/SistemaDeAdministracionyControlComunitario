using System;
using System.Collections.Generic;
using System.Text;

namespace SAyCC.Entities.SystemAdmin
{
    public class WaterMeterEntity
    {
        public int Id { get; set; }
        public string Numero { get; set; }
        public DateTime FechaAlta { get; set; }
        public DateTime? FechaBaja { get; set; }
        public decimal LecturaActual { get; set; }
        public decimal LecturaAnterior { get; set; }
        public int IdTitular { get; set; }
    }
}

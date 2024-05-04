using System;
using System.Collections.Generic;
using System.Text;

namespace SAyCC.Entities.WaterSystem
{
   public class BreakdownEntity
    {
        public int Identificador { get; set; }
        public string Concepto { get; set; }
        public int MetrosUsados { get; set; }
        public decimal TotalPagar { get; set; }
             
    }
}

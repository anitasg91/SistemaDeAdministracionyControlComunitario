using System;
using System.Collections.Generic;
using System.Text;

namespace SAyCC.Entities.WaterSystem
{
    public class WaterMeterList
    {
        public int IdManzana { get; set; }
       public List<WaterMeterToEdit> Medidor { get; set; }
    }
    public class WaterMeterToEdit
    {
        public int Id { get; set; }
        public string Numero { get; set; }
        public int IdManzana { get; set; }
        public decimal LecturaActual { get; set; }
        public decimal LecturaAnterior { get; set; }
    }

}

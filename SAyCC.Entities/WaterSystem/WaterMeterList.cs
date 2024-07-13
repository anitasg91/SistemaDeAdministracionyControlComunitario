using System;
using System.Collections.Generic;
using System.Text;

namespace SAyCC.Entities.WaterSystem
{
    public class WaterMeterList
    {
        public int IdManzana { get; set; }
       public List<WaterMeterToEdit> Medidor { get; set; } = new List<WaterMeterToEdit>();
        //public List<WaterMeterReading> Reading { get; set; } = new List<WaterMeterReading>();
        
    }
    public class WaterMeterToEdit
    {
        public int Id { get; set; }
        public string Numero { get; set; }
        public int IdManzana { get; set; }
        public decimal LecturaAnterior { get; set; }
        public decimal LecturaActual { get; set; }
        public int Periodo { get; set; }
    }
    //public class WaterMeterReading
    //{
    //    public int Id { get; set; }
    //    public decimal LecturaActual { get; set; }
    //    public int Periodo { get; set; }
    //}
}

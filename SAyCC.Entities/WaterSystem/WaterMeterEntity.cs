using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace SAyCC.Entities.WaterSystem
{
    public class WaterMeterEntity
    {
        public int Id { get; set; }
        public string Numero { get; set; }
        public DateTime FechaAlta { get; set; }
        public DateTime? FechaBaja { get; set; }
        public decimal LecturaActual { get; set; }
        public decimal LecturaAnterior { get; set; }
        public int? IdTitular { get; set; }
        public string NombreTitular { get; set; }
        public bool Activo { get; set; }
        public int IdManzana { get; set; }
        public string Ubicacion { get; set; }
        public DateTime FechaLectura { get; set; }

       
       
        public string strClassFechaLectura
        {
            get
            {
                var fechaActual = DateTime.Now.Month;
                var fecha = FechaLectura.Month;
                return fecha == fechaActual ? "text-success" : Activo && FechaBaja == new DateTime?() ? "text-danger" : "text-black";

            }
        }
        public string strFechaLectura { get { return FechaLectura.ToString("dd/MMMM/yyyy").ToUpper(); } }
        public string Estatus { get { return Activo && FechaBaja == new DateTime?() ? "fas fa-check-circle" : "fas fa-ban"; } }
        public string Deactivate { get { return Activo ? "fas fa-ban" : "fas fa-unlock"; } }
        public string Color { get { return (Activo && FechaBaja == new DateTime?()) ? "Green" : (!Activo && FechaBaja == new DateTime?()) ? "Red" : "gray"; } }
        public string Assocciate { get { return IdTitular == new int?() ? "fa-user": "fa-user-slash"; } }
        public string ColorAssocciate { get { return IdTitular == new int?() ? "green" : "yellow"; } }
        public string ColorUnsubscribe { get { return FechaBaja == new DateTime?() ? "Red" : "DarkGrey"; } }
        public string ColorDeactivate { get { return Activo ? "DimGrey" : "Aqua"; } }
        public string TitleDeactivate { get { return Activo ? "Desactivar" : "Activar"; } }
        public string TitleAssocciate { get { return IdTitular == new int?() ? "Asociar" : "Desasociar"; } }
        public string TitleUnsubscribe { get { return FechaBaja == new DateTime?() ? "Dar de baja" : "medidor dado de baja"; } }

    }
}

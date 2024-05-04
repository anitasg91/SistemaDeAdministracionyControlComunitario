using System;
using System.Collections.Generic;
using System.Text;

namespace SAyCC.Entities.WaterSystem
{
    public class VoucherEntity
    {
        public int Id { get; set; }
        public long IdMedidor { get; set; }
        public long IdUsuario { get; set; }
        public int IdManzana { get; set; }
        public int IdMes { get; set; }
        public string Estado { get; set; }
        public DateTime Fecha { get; set; }
        public string Folio { get; set; }
        public string Periodo { get; set; }
        public int LecturaAnt { get; set; }
        public int LecturaAct { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int TotalImUsado { get; set; }
        public decimal TotalPagado { get; set; }

        public string Titular { get; set; }
        public string ClaveTitular { get; set; }
        public string DireccionMedidor { get; set; }
        public string NoMedidor { get; set; }
        public string Manzana { get; set; }
        public bool Pagado { get; set; }

        public decimal TotalPagarAdeudo { get; set; }
        public List<BreakdownEntity> Desglose { get; set; }
        public List<GraphicDataEntity> Grafica { get; set; }
        public string Estatus { get { return Pagado ? "Pagado" : (!Pagado &&   (Fecha.Month + '/'+ Fecha.Year == DateTime.Now.Month +'/'+ DateTime.Now.Year))? "Pendiente" : "Vencido"; } }
        public string TextColor { get { return Pagado ? "text-success" : (!Pagado && (Fecha.Month + '/' + Fecha.Year == DateTime.Now.Month + '/' + DateTime.Now.Year)) ? "text-primary" : "text-danger"; } }
    }
}

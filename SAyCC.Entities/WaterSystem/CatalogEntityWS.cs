using System;
using System.Collections.Generic;
using System.Text;

namespace SAyCC.Entities.WaterSystem
{
   public class CatalogEntityWS
    {
        public List<TipoLectura> TipoLectura { get; set; }
        public List<Folio> Folio { get; set; }
    }
    public class TipoLectura {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public int Inicial { get; set; }
        public int? Limite { get; set; }
        public int Precio { get; set; }
    }
  
    public class Folio {
        public int Id { get; set; }
        public int IdManzana { get; set; }
        public int Consecutivo { get; set; }
        public DateTime FechaAlta { get; set; }
        public long IdUserAlta { get; set; }
        public string UsuarioAlta { get; set; }
    }
}

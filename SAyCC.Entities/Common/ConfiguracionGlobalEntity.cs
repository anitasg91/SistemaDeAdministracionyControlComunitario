using System;
using System.Collections.Generic;
using System.Text;

namespace SAyCC.Entities.Common
{
    public class ConfiguracionGlobalEntity
    {
      public int ID { get; set; }
      public string Nombre { get; set; }
        public string Valor { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace SAyCC.Entities.SystemAdmin
{
    public class UserUtilityEntity
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public bool Activo { get; set; }
        public int IdEstatus { get; set; }

        //Auxiliares Modal
        public string TitleModal { get; set; } = string.Empty;
        public string IconoModal { get; set; } = string.Empty;
        public string MensajeModal { get; set; } = string.Empty;
    }
}

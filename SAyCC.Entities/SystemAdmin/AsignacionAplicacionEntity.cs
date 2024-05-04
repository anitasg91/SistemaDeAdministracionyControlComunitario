using System;
using System.Collections.Generic;
using System.Text;

namespace SAyCC.Entities.SystemAdmin
{
    public class AsignacionAplicacionEntity
    {
        public int Id { get; set; }
        public int IdPerfil { get; set; }
        public int IdAplicacion { get; set; }
        public string NombreApp { get; set; }
        public bool Activo { get; set; }
        public string Estatus { get { return Activo ? "fas fa-check-circle" : "fas fa-ban"; } }
        public string Color { get { return Activo ? "Green" : "Red"; } }

    }
}

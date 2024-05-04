using System;
using System.Collections.Generic;
using System.Text;

namespace SAyCC.Entities.SystemAdmin
{
    public class ProfileEntity
    {
        public int Id { get; set; }
        public string Detalle { get; set; }
        public bool Activo { get; set; }
        public int IdUsuarioAlta { get; set; }
        public DateTime FechaAlta { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public int? IdUsuarioModificacion { get; set; }
        public int IDAplicacion { get; set; }


        ///Propiedades extras
        public string Estatus { get { return Activo ? "fas fa-check-circle" : "fas fa-ban"; } }
        public string Color { get { return Activo ? "Green" : "Red"; } }
        public string Action { get { return Activo ? "fas fa-ban" : "fas fa-unlock"; } }
        public int IDCatalog { get; set; }
        public string Aplicacion { get; set; }
        public string NombreUsuarioAlta { get; set; }
        public string NombreUsuarioMod { get; set; }

        public List<AsignacionAplicacionEntity> AsignacionAplicacion { get; set; }
        public List<AsignacionAplicacionEntity> AppSinAcceso { get; set; }

    }
}

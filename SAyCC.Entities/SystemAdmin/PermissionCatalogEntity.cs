using System;
using System.Collections.Generic;
using System.Text;

namespace SAyCC.Entities.SystemAdmin
{
    public class PermissionCatalogEntity
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }
        public int IdUsuarioAlta { get; set; }
        public DateTime FechaCreacion { get; set; }

        ///Propiedades extras
        public string Estatus { get { return Activo ? "fas fa-check-circle" : "fas fa-ban"; } }
        public string Color { get { return Activo ? "text-success" : "text-danger"; } }
        public string Action { get { return Activo ? "fas fa-ban" : "fas fa-unlock"; } }
        public string strFechaCreacion { get { return FechaCreacion != new DateTime() ? FechaCreacion.ToString("dd/MM/yyyy") : string.Empty; } }
        public string Checked { get { return Activo ? "checked" : ""; } }
        public int IdPermisoPagina { get; set; } = 0;
        

    }

}

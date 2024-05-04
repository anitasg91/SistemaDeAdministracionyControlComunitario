using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SAyCC.Entities.SystemAdmin
{
    public class ModuleEntity
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Este campo es requerido.")]
        public string Titulo { get; set; }
        [Required(ErrorMessage = "Este campo es requerido.")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "Este campo es requerido.")]
        public string Controlador { get; set; }
        [Required(ErrorMessage = "Este campo es requerido.")]
        public string Accion { get; set; }
        public bool Activo { get; set; }
        public int IdUsuarioAlta { get; set; }
        public DateTime FechaAlta { get; set; }
        public DateTime ? FechaModificacion { get; set; }
        public int? IdUsuarioModificacion { get; set; }
        public int? IDPadre { get; set; }
        [Required(ErrorMessage = "Este campo es requerido.")]
        public int Orden { get; set; }
        [Required(ErrorMessage = "Este campo es requerido.")]
        public string i_class { get; set; }
        public int IdAplicacion { get; set; }

        ///Propiedades extras
        public string Estatus { get { return Activo ? "fas fa-check-circle" : "fas fa-ban"; } }
        public string Color { get { return Activo ? "Green" : "Red"; } }
        public string Action { get { return Activo ? "fas fa-ban" : "fas fa-unlock"; } }
        public string NombreUsuarioAlta { get; set; }
        public string NombreUsuarioMod { get; set; }
        public string Aplicacion { get; set; }
        public bool Alta { get; set; }
        public bool Modificacion { get; set; }
        public bool Consulta { get; set; }

    }
}

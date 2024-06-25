using SAyCC.Entities.SystemAdmin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SAyCC.Entities.Login
{
    public class ApplicationEntity
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Este campo es requerido.")]
        public string Descripcion { get; set; }
        public Boolean Activo { get; set; }
        public DateTime FechaAlta { get; set; }
        public int IdUsuarioAlta { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public int? IdUsuarioModificacion { get; set; }
        [Required(ErrorMessage = "Este campo es requerido.")]
        public string Icono { get; set; }
        [Required(ErrorMessage = "Este campo es requerido.")]
        public string Accion { get; set; }
        [Required(ErrorMessage = "Este campo es requerido.")]
        public string Controlador { get; set; }
        [Required(ErrorMessage = "Este campo es requerido.")]
        public string Dominio { get; set; }
        public int IdTipoAplicacion { get; set; }
        public string TipoAplicacion { get; set; }
        public string Estatus { get { return Activo ? "fas fa-check-circle" : "fas fa-ban"; } }
        public string Color { get { return Activo ? "Green" : "Red"; } }
        public string Action { get { return Activo ? "fas fa-ban" : "fas fa-unlock"; } }
        public string NombreUsuarioAlta { get; set; }
        public string NombreUsuarioMod { get; set; }
       public List<ModuleEntity> Modules { get; set; }
       public List<ProfileEntity> Profiles { get; set; }
       //public List<PermissionEntity> permission { get; set; }

    }
}

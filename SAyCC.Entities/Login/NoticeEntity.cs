using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SAyCC.Entities.Login
{
    public class NoticeEntity
    {

        [BindProperty]
        [TempData]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio")]
        public string Descripcion { get; set; }

        public DateTime FechaCreacion { get; set; }
        public DateTime FechaEvento { get; set; }
        public int IdAplicacion { get; set; }
        public int IdTipoAviso { get; set; }
        public int IdPrioridad { get; set; }
        public int IdUsuario { get; set; }
        public string iconoAplicacion { get; set; }
        public string nombreUsuario { get; set; }
        public string descripcionTipoAviso { get; set; }
        public string PrioridadDescripcion { get; set; }
        public string PrioridadClase { get; set; }
    }
}

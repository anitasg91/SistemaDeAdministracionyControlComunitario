using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaAdministracionyControlComunidad.Models
{
    public class ContrasenaViewModel
    {
        [BindProperty]
        public InputModel Input { get; set; }
        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "El campo es Obligatorio")]
            [DataType(DataType.Password)]
            [StringLength(15, ErrorMessage = "El Password debe de ser mínimo de 6 Caracteres", MinimumLength = 6)]
            public string Password { get; set; }
            [Required(ErrorMessage = "El campo es Obligatorio")]
            [DataType(DataType.Password)]
            [StringLength(15, ErrorMessage = "El Password debe de ser mínimo de 6 Caracteres", MinimumLength = 6)]
            public string PasswordNuevo { get; set; }
            [Required(ErrorMessage = "El campo es Obligatorio")]
            [DataType(DataType.Password)]
            [StringLength(15, ErrorMessage = "El Password debe de ser mínimo de 6 Caracteres", MinimumLength = 6)]
            [CompareAttribute("PasswordNuevo", ErrorMessage = "La contraseña no coincide")]
            public string PasswordConfirmacion { get; set; }
        }
    }
}

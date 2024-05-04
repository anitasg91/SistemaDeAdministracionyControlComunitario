using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;


namespace SistemaAdministracionyControlComunidad.Models
{
    public class LoginViewModel
    {
        [BindProperty]
        [TempData]
        public string ErrorMessage { get; set; }
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string SessionStart { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc;
using SAyCC.Entities.SystemAdmin;
using SAyCC.Entities.WaterSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace SAyCC.Entities.Login
{
    public class UserEntity
    {
        [BindProperty]
        [TempData]
        public string ErrorMessage { get; set; }
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo es obligatorio")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El campo es obligatorio")]
        public string APaterno { get; set; }
        [Required(ErrorMessage = "El campo es obligatorio")]
        public string AMaterno { get; set; }
        public string Telefono { get; set; }
        [EmailAddress(ErrorMessage = "El Email no es valido")]
        public string Email { get; set; }
        public DateTime FechaAlta { get; set; }
        public int IdUsuarioAlta { get; set; }
        public string Usuario { get; set; }
        public string Password { get; set; }
        public string ConfirmPassw { get; set; }
        public Boolean PrimerLogin { get; set; }
        public DateTime? InicioSesion { get; set; }
        public Boolean SesionActiva { get; set; }
        public int NoIntentosFallidos { get; set; }
        public Boolean Activo { get; set; }
        public Boolean PerfilActivo { get; set; }
        public DateTime? FechaMod { get; set; }
        public int? IdUsuarioMod { get; set; }
        public bool Sexo { get; set; }
        public int Genero { get; set; }
        public int IdManzana { get; set; }
        //public int IdPerfil { get; set; }
        public string ImagenUpload { get; set; }
        [Required(ErrorMessage = "El campo es obligatorio")]
        public DateTime FechaNacimiento { get; set; }
        public string Perfil { get; set; }
        public string NombreCompleto { get { return Nombre + " " + APaterno + " " + AMaterno; } }
        public string Manzana { get; set; }
        public string Estatus
        {
            get
            {
                return IdEstatus == (int)EstatusUsuario.Activo ? "fas fa-check-circle" :
                     IdEstatus == (int)EstatusUsuario.Inactivo ? "fas fa-ban" :
                     IdEstatus == (int)EstatusUsuario.Eliminado ? "fa fa-times-circle" :
                     IdEstatus == (int)EstatusUsuario.EnvíoEliminación || IdEstatus == (int)EstatusUsuario.EnvíoInactivación
                    || IdEstatus == (int)EstatusUsuario.EnvíoActivación || IdEstatus == (int)EstatusUsuario.EnvíoRecuperación ?
                    "fa fa-share" : "fa fa-clock";
            }
        }
        public string Action { get { return IdEstatus == (int)EstatusUsuario.Activo ? "fas fa-ban" : "fas fa-unlock"; } }
        //public string Color { get { return Activo ? "Green" : "Red"; } }
        public string Color
        {
            get
            {
                return IdEstatus == (int)EstatusUsuario.Activo ? "text-success" :
                    IdEstatus == (int)EstatusUsuario.Inactivo ? "text-secondary" :
                     IdEstatus == (int)EstatusUsuario.Eliminado ? "text-danger" :
                     IdEstatus == (int)EstatusUsuario.EnvíoEliminación || IdEstatus == (int)EstatusUsuario.EnvíoInactivación
                    || IdEstatus == (int)EstatusUsuario.EnvíoActivación || IdEstatus == (int)EstatusUsuario.EnvíoRecuperación ?
                    "text-info" : "text-warning";
            }
        }
        public List<WaterMeterEntity> Medidor { get; set; }
        public Boolean CreateUser { get; set; }
        public Boolean SendEmail { get; set; }
        public string Identificador { get; set; }
        public string FechaNac { get { return FechaNacimiento.ToString("yyyy-MM-dd"); } }
        public byte[] ImgFromBase64string { get { return string.IsNullOrEmpty(ImagenUpload) ? null : Convert.FromBase64String(ImagenUpload); } }
        public string ImgToBase64string { get { return string.IsNullOrEmpty(ImagenUpload) ? null : Convert.ToBase64String(ImgFromBase64string); } }

        public int IdEstatus { get; set; }
        public string NombreEstatus { get; set; }
        public bool HasApprovalSubdelegate { get; set; } = false;
        public bool HasApprovalJudge { get; set; } = false;
        public bool HasApprovalTreasurer { get; set; } = false;

        public string ApprovalSubdelegate { get { return HasApprovalSubdelegate ? "Aprobado por el subdelegado" : ""; } }
        public string ApprovalJudge { get { return HasApprovalJudge ? "Aprobado por el juez" : ""; } }
        public string ApprovalTreasurer { get { return HasApprovalTreasurer ? "Aprobado por el tesorero" : ""; } }

    }
}

using System;
using System.Collections.Generic;
using System.Text;
using static SAyCC.Entities.Common.Enumerador;

namespace SAyCC.Entities.SystemAdmin
{
    public class PermissionsConfigJson
    {
        public int IdUsuario { get; set; }
        public int IdAplicacion { get; set; }
        public List<int> Roles { get; set; }
        public List<int> Block { get; set; }
    }

    public class PermissionsConfigRequest
    {
        public List<UsuarioPerfil> Roles { get; set; } = new List<UsuarioPerfil>();
        public List<UsuarioManzana> Block { get; set; } = new List<UsuarioManzana>();
    }

    public class UsuarioPerfil
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public int IdPerfil { get; set; }
        public int IdAplicacion { get; set; }

    }
    public class UsuarioManzana
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public int IdManzana { get; set; }
        public int IdAplicacion { get; set; }
    }

    public class PermisoPaginaPerfilResponse
    {
        public ProfileEntity Perfil { get; set; } = new ProfileEntity();
        public List<ModuleEntity> Paginas { get; set; } = new List<ModuleEntity>();
        public List<PermisosByPagina> PermisosPagina { get; set; } = new List<PermisosByPagina>();
        //public List<PermisoPaginaPerfil> PermisoPaginaPerfilList { get; set; } = new List<PermisoPaginaPerfil>();
    }
    public class PermisosByPagina
    {
        public int IdPagina { get; set; }
        public string Pagina { get; set; }
        public int Orden { get; set; }
        public List<PermisoPaginaEntity> PermisosPaginas { get; set; } = new List<PermisoPaginaEntity>();
    }
    public class PermisoPaginaEntity
    {
        public int Id { get; set; }
       /*public int IdPagina { get; set; }
        public int IdPermiso { get; set; }*/
        public string NombrePermiso { get; set; } = null;
        public string DescripcionPermiso { get; set; } = null;
        public bool Asignado { get; set; }

    }

    public class PermisoPaginaPerfilRequest
    {
        public int IdPerfil { get; set; } = 0;
        public string Nombre { get; set; }
        public bool Activo { get; set; }
        public int IdUsuarioModificacion { get; set; } = 0;
        public List<PermisoPaginaPerfilEntity> PermisoPaginaPerfil { get; set; } = new List<PermisoPaginaPerfilEntity>();
    }

    public class PermisoPaginaPerfilEntity
    {
        public int Id { get; set; } = 0;
        public int IdPermisoPagina { get; set; }
        public int IdPerfil { get; set; }
      
    }

    public class PermissionPageCurrentEntity
    {
        public int Id { get; set; } = 0;
        public string Permiso { get; set; }
        public int IdPagina { get; set; } = 0;

    }

}

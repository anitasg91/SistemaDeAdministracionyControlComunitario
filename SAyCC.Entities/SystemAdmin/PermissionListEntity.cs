using System;
using System.Collections.Generic;
using System.Text;

namespace SAyCC.Entities.SystemAdmin
{
    public class PermissionListEntity
    {
        public List<CatalogEntity> Block { get; set; }
        public List<ProfileEntity> Role { get; set; }
        public List<CatalogEntity> UserBlock { get; set; } = new List<CatalogEntity>();
        public List<ProfileSesionEntity> UserProfile { get; set; } = new List<ProfileSesionEntity>();
       // public List<UsuarioPerfil> UserProfile { get; set; } = new List<UsuarioPerfil>();
        
    }
    //public class BlockEntity
    //{
    //    public int Id { get; set; }
    //    public int Descripcion { get; set; }
    //    public bool Activo { get; set; } = false;

    //}
}

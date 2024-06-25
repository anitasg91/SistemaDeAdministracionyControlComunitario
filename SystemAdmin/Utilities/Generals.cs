using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SAyCC.SystemAdmin.Utilities;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using SAyCC.Entities.SystemAdmin;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace SAyCC.SystemAdmin.Utilities
{
    [SessionValidator]
    public class Generals : IGenerals
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        public Generals(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        private string Roles => _httpContextAccessor.HttpContext.Session.GetString(Sessions.RolesList);
        private int idUser => (int)_httpContextAccessor.HttpContext.Session.GetInt32(Sessions.IdUser);
        private int idApplication => (int)_httpContextAccessor.HttpContext.Session.GetInt32(Sessions.IdApp);
        private string userName => _httpContextAccessor.HttpContext.Session.GetString(Sessions.UserName);
        private string imagenUpload => _httpContextAccessor.HttpContext.Session.GetString(Sessions.ImagenUpload);
        private string permissionPageCurrent => _httpContextAccessor.HttpContext.Session.GetString(Sessions.PermissionPageCurrent);
        private int idCurrentPage => (int)_httpContextAccessor.HttpContext.Session.GetInt32(Sessions.CurrentPage);
        private string AllPagesByApp => _httpContextAccessor.HttpContext.Session.GetString(Sessions.AllPagesByApp);
        private string ManzanasAsignadas => _httpContextAccessor.HttpContext.Session.GetString(Sessions.BlockAsignedList);
        public string DecompressFromBase64(string base64EncodedString)
        {

            byte[] compressedData = Convert.FromBase64String(base64EncodedString);

            using (MemoryStream ms = new MemoryStream(compressedData))
            using (MemoryStream decompressedMs = new MemoryStream())
            {
                using (GZipStream gzipStream = new GZipStream(ms, CompressionMode.Decompress))
                {
                    gzipStream.CopyTo(decompressedMs);
                }

                return Encoding.UTF8.GetString(decompressedMs.ToArray());
            }
        }
        /// <summary>
        /// Obtiene roles asignados.
        /// </summary>
        public List<ProfileSesionEntity> RolesAsign
        {
            get
            {
                List<ProfileSesionEntity> roles = new List<ProfileSesionEntity>();
                if (!string.IsNullOrEmpty(Roles))
                {
                    roles = JsonConvert.DeserializeObject<List<ProfileSesionEntity>>(Roles);
                }
                return roles;
            }
        }
        /// <summary>
        /// Regresa el id del usuario logueado.
        /// </summary>
        public int IdUser { get { return idUser; } }
        /// <summary>
        /// Regresa el id de la pagina actual, hay que actualizar la variable de sesión cada que cambie de página.
        /// </summary>
        public int IdCurrentPage { get { return idCurrentPage; } }
        /// <summary>
        /// Regresa el id de la aplicación actual.
        /// </summary>
        public int IdApplication { get { return idApplication; } }
        /// <summary>
        /// Regresa el nombre completo del usuario logueado
        /// </summary>
        public string UserName { get { return userName; } }
        /// <summary>
        /// Regresa el arreglo de bytes para convertir a imagen la foto de perfil del usuario logueado.
        /// </summary>
        public byte[] ImagenBytesIlustrative
        {
            get
            {
                return string.IsNullOrEmpty(imagenUpload) ? new byte[0] : Convert.FromBase64String(imagenUpload);
            }
        }
        /// <summary>
        /// Indica si el usuario logueado tiene el perfil de superadmin.
        /// </summary>
        public bool IsSuperAdmin
        {
            get
            {
                return RolesAsign.Any(_ => _.Id == (int)roles.Superadmin);
            }
        }
        /// <summary>
        /// Regresa las lista de permisos que tiene el usuario sobre la página actual.
        /// </summary>
        public List<PermissionPageCurrentEntity> PermissionPageCurrent
        {
            get
            {
                List<PermissionPageCurrentEntity> PPC = new List<PermissionPageCurrentEntity>();
                if (!string.IsNullOrEmpty(permissionPageCurrent))
                {
                    PPC = JsonConvert.DeserializeObject<List<PermissionPageCurrentEntity>>(permissionPageCurrent).Where(x => x.IdPagina == idCurrentPage).ToList();
                }
                return PPC;
            }
        }
        /// <summary>
        /// Regresa todas las páginas a las que tiene acceso el usuario logueado. 
        /// </summary>
        public List<ModuleEntity> AllPagesByAppList
    {
            get
            {
                List<ModuleEntity> pages = new List<ModuleEntity>();
                if (!string.IsNullOrEmpty(AllPagesByApp))
                {
                    pages = JsonConvert.DeserializeObject<List<ModuleEntity>>(AllPagesByApp).OrderBy(_=>_.Orden).ToList();
                }
                return pages;
            }
        }
        /// <summary>
        /// Regresa si tiene acceso a la lista de acciones enviadas.
        /// </summary>
        /// <param name="OptionList">Lista de enteros que corresponden al enumerador de permisos.</param>
        /// <returns></returns>
        public bool HasAccessTo(int[] OptionList) {
            bool hasAccess = false;
            if (!string.IsNullOrEmpty(permissionPageCurrent))
            {
                List<PermissionPageCurrentEntity> PPC = JsonConvert.DeserializeObject<List<PermissionPageCurrentEntity>>(permissionPageCurrent).Where(x => x.IdPagina == idCurrentPage).ToList();
                hasAccess = PPC.Any(x => OptionList.Contains(x.Id));
            }
            return hasAccess;
        }        
        public bool HasRol(roles Rol) {
            return RolesAsign.Any(_ => _.Id == (int)Rol);
        }
        public bool HasBlock(int Rol)
        {
            List<CatalogEntity> blocks = new List<CatalogEntity>();
            if (!string.IsNullOrEmpty(ManzanasAsignadas))
            {
                blocks = JsonConvert.DeserializeObject<List<CatalogEntity>>(ManzanasAsignadas);
            }
            return blocks.Any(_ => _.Id == Rol);
        }
        

    }
}

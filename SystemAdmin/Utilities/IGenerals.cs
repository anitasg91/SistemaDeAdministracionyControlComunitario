
using SAyCC.Entities.SystemAdmin;
using System.Collections.Generic;

namespace SAyCC.SystemAdmin.Utilities
{
    public interface IGenerals
    {
        string DecompressFromBase64(string base64EncodedString);
        List<ProfileSesionEntity> RolesAsign { get; }
        int IdUser { get; }
        int IdApplication { get; }
        string UserName { get; }
        byte[] ImagenBytesIlustrative { get; }
        bool IsSuperAdmin { get; }
        List<PermissionPageCurrentEntity> PermissionPageCurrent { get; }
        int IdCurrentPage { get; }
        List<ModuleEntity> AllPagesByAppList { get; }
        bool HasAccessTo(int[] OptionList);
        bool HasRol(roles Rol);
        bool HasBlock(int Manzana);

    }
}

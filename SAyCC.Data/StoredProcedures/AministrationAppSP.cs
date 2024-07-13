using System;
using System.Collections.Generic;
using System.Text;
using static SAyCC.Entities.Common.Enumerador;

namespace SAyCC.Data.StoredProcedures
{
    public static class AministrationAppSP
    {
        public static string GetUsers => "[Administracion].[spGetUsers]";
        public static string GetUserDetailById => "[Administracion].[spGetUserDetailById]";
        public static string GetUserByParameter => "[Administracion].[spGetUserByParameter]";
        public static string SaveUser => "[login].[spSaveUser]";
        public static string getApplications => "[seguridad].[spObtieneAplicaciones]";
        public static string SaveAplicacion => "[Administracion].[spSaveAplicacion]";
        public static string ChangeStatusApplication => "[Administracion].[spChangeStatusApplication]";
        public static string DeleteApplication => "[Administracion].[spDeleteApplication]";
        public static string GetModules => "[Administracion].[spGetModulesAllowsByUser]";
        //public static string GetModules = "[Administracion].[spGetModules]";
        public static string GetRolesByUserAndAplication => "[Administracion].[spGetRolesByUserAndAplication]";
        public static string SaveModule => "[Administracion].[spSaveModule]";
        public static string ChangeStatusModule => "[Administracion].[spChangeStatusModule]";
        public static string DeleteModule => "[Administracion].[spDeleteModule]";
        public static string GetProfiles => "[Administracion].[spGetProfiles]"; /*Borrar*/
        public static string SaveProfiles => "[Administracion].[spSaveProfile]";
        public static string ChangeStatusProfile => "[Administracion].[spChangeStatusProfile]";
        public static string GetProfilesByAppActives => "[Administracion].[spGetProfilesByAppActives]";
        public static string DeleteProfile => "[Administracion].[spDeleteProfile]";
        public static string GetPermission => "[Administracion].[spGetPermission]";
        public static string SavePermission => "[Administracion].[spSavePermission]";
        public static string GetCatalog => "[Administracion].[GetCatalog]";        
        public static string DeleteCatalog => "[Administracion].[spDeleteCatalog]";
        public static string SaveCatalog => "[Administracion].[spSaveCatalog]";
        public static string GetAsignacionAplicacion => "[Administracion].[spGetAsignacionAplicacion]";
        public static string saveAsignacionAplicacion => "[Administracion].[spSaveAsignacionAplicacion]";
        public static string SaveWaterMeter => "[AguaPotable].[spSaveWaterMeter]";
        public static string DeleteWaterMeterById => "[AguaPotable].[spDeleteWaterMeterById]";
        public static string UpdateWaterMeterById => "[AguaPotable].[spUpdateWaterMeterById]";
        public static string GetWaterMeterByIdUser => "[Administracion].[spGetWaterMeterByIdUser]";
        public static string SavePermissionsBulk => "[Administracion].[SavePermissionsConfigurationBulk]";
        public static string GetPagesFathers => "[Administracion].[spGetPagesFathers]";
        public static string GetPagesAndPermissionAsign => "[Administracion].[spGetPagesAndPermissionAsign]";
        public static string SavePermisoPaginaPerfilBulk => "[Administracion].[SavePermisoPaginaPerfilBulk]";
        public static string GetBlockByUserAndAplication => "[Administracion].[spGetBlockByUserAndAplication]";
        public static string GetPermissionsByUsuarioAndAplication => "[Administracion].[spGetPermissionsByUsuarioAndAplication]";
        public static string GetPermissionCatalog => "[Administracion].[GetPermissionCatalog]";
        public static string GetAdministradorCatalogo => "[Administracion].[spGetAdministradorCatalogo]";
        public static string SavePermissionCatalog => "[Administracion].[spSavePermissionCatalog]";
        public static string LockCatalog => "[Administracion].[spLockCatalog]";
        public static string GetPermissionCatalogByNotExistInPage => "[Administracion].[spGetPermissionCatalogByNotInPage]";
        public static string GetPermissionCatalogByPage => "[Administracion].[spGetPermissionCatalogByPage]";
        public static string SavePermissionPage => "[Administracion].[spSavePermissionPage]";
        public static string DeletePermissionPage => "[Administracion].[spDeletePermissionPage]";
        public static string ChangeStatusUser => "[Administracion].[spChangeStatusUser]";
        public static string GetBlockAsignedByApp => "[dbo].[GetBlockAsignedByApp]";
        public static string ChangeStatusUserBySuperAdmin => "[Administracion].[spChangeStatusUserBySuperAdmin]";
        public static string DeleteUserForErrorToSave => "[Administracion].[spDeleteUserForErrorToSave]";
        
    }


    public static class ParameterName
    {
        public static string TblPermisoPaginaPerfil => "@PermisoPaginaPerfil";
        public static string IdPerfil => "IdPerfil";
        public static string Nombre => "Nombre";
        public static string Activo => "Activo";
        public static string IdUsuarioModificacion => "IdUsuarioModificacion";
        public static string IdUsuario => "IdUsuario";
        public static string IdApp => "IdApp";
        public static string IdPermiso => "IdPermiso";
        public static string IdPagina => "IdPagina";
        public static string IdPermisoPagina => "IdPermisoPagina";
        public static string IdUserLoggin => "IdUserLoggin";
        public static string Estatus => "Estatus";
        public static string Motivo => "Motivo";
        
    }

    public static class DataReaderColumn
    {
        public static string Id => "Id";
        public static string Detalle => "Detalle";
        public static string Descripcion => "Descripcion";
        public static string IdPermiso => "IdPermiso";
        public static string Permiso => "Permiso";
        public static string IdPagina => "IdPagina";
        public static string Nombre => "Nombre";
        public static string Activo => "Activo";
        public static string FechaCreacion => "FechaCreacion";
        public static string IdPermisoPagina => "IdPermisoPagina";
        

    }
    

    public static class ColumnDataTable
    {
        public static string IdPermisoPagina => "IdPermisoPagina";
        public static string IdPerfil => "IdPerfil";
        public static string IdUsuarioCreacion => "IdUsuarioCreacion";
    }
    
    public static class TypeTable
    { 
        public static string PermisoPaginaPerfil => "[Administracion].[TipoPermisoPaginaPerfil]";
    }
}

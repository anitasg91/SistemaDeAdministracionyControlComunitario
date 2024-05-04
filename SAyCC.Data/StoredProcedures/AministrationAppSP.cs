using System;
using System.Collections.Generic;
using System.Text;

namespace SAyCC.Data.StoredProcedures
{
    public static class AministrationAppSP
    {
        public static string GetUsers = "[Administracion].[spGetUsers]";
        public static string GetUserDetailById = "[Administracion].[spGetUserDetailById]";
        public static string GetUserByParameter = "[Administracion].[spGetUserByParameter]";
        public static string SaveUser = "[login].[spSaveUser]";
        public static string getApplications = "[seguridad].[spObtieneAplicaciones]";
        public static string SaveAplicacion = "[Administracion].[spSaveAplicacion]";
        public static string ChangeStatusApplication = "[Administracion].[spChangeStatusApplication]";
        public static string DeleteApplication = "[Administracion].[spDeleteApplication]";
        public static string GetModules = "[Administracion].[spGetModules]";
        public static string SaveModule = "[Administracion].[spSaveModule]";
        public static string ChangeStatusModule = "[Administracion].[spChangeStatusModule]";
        public static string DeleteModule = "[Administracion].[spDeleteModule]";
        public static string GetProfiles = "[Administracion].[spGetProfiles]";
        public static string SaveProfiles = "[Administracion].[spSaveProfile]";
        public static string ChangeStatusProfile = "[Administracion].[spChangeStatusProfile]";
        public static string GetProfilesByAppActives = "[Administracion].[spGetProfilesByAppActives]";
        public static string DeleteProfile = "[Administracion].[spDeleteProfile]";
        public static string GetPermission = "[Administracion].[spGetPermission]";
        public static string SavePermission = "[Administracion].[spSavePermission]";
        public static string GetCatalog = "[Administracion].[GetCatalog]";
        public static string DeleteCatalog = "[Administracion].[spDeleteCatalog]";
        public static string SaveCatalog = "[Administracion].[spSaveCatalog]";
        public static string GetAsignacionAplicacion = "[Administracion].[spGetAsignacionAplicacion]";
        public static string saveAsignacionAplicacion = "[Administracion].[spSaveAsignacionAplicacion]";
        public static string SaveWaterMeter = "[AguaPotable].[spSaveWaterMeter]";
        public static string DeleteWaterMeterById = "[AguaPotable].[spDeleteWaterMeterById]";
        public static string UpdateWaterMeterById = "[AguaPotable].[spUpdateWaterMeterById]";
        public static string GetWaterMeterByIdUser = "[Administracion].[spGetWaterMeterByIdUser]";
    }
}

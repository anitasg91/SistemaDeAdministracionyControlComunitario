using System;
using System.Collections.Generic;
using System.Text;

namespace SAyCC.Data.StoredProcedures
{
    public static class LoginSP
    {
        public static string GetUserForLogin = "[login].[spGetUserByLogin]";
        public static string GetUser = "[login].[spGetUserById]";
        public static string UpdateInfoUser = "[login].[spUpdateInfoUser]";
        public static string GetApplicationsAllowed = "[login].[spGetApplicationsAllowed]";
        public static string ChangePassword = "[seguridad].[spChangePassword]";
        public static string GetNoticeByIdUser = "[Common].[spGetNoticeByIdUser]";
        public static string GetTipoAviso = "[Common].[spGetTipoAviso]";
        public static string GetPrioridad = "[Common].[spGetPrioridad]";
        public static string SaveNotice = "[Common].[spSaveUpdateNotice]";

        
    }
}

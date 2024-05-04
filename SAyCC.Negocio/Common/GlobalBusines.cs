using SAyCC.Data.Repository;
using SAyCC.Entities.Common;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace SAyCC.Bussiness.Common
{
    public static class GlobalBusines
    {
        public static string GetSHA256(string password)
        {
            SHA256 sha256 = SHA256Managed.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = sha256.ComputeHash(encoding.GetBytes(password));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }

        public static List<ConfiguracionGlobalEntity> GetConfiguracionGlobal()
        {
            using (ConfiguracionGlobalRepository objDBD = new ConfiguracionGlobalRepository())
            {
                var Result = objDBD.GetConfiguracionGlobal(DBSet.DBcnn);
                objDBD.Dispose();
                return Result;
            }

        }

    }
}

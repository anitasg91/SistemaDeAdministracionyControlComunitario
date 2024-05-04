using SAyCC.Data.StoredProcedures;
using SAyCC.Entities.Common;
using SAyCC.Entities.Login;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace SAyCC.Data.Repository
{
    public class ConfiguracionGlobalRepository : IDisposable
    {
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        SqlParameter[] _Parametros;
        List<ConfiguracionGlobalEntity> Lista = new List<ConfiguracionGlobalEntity>();

        public List<ConfiguracionGlobalEntity> GetConfiguracionGlobal(string DBCnn)
        {
            SqlDataReader DatosReader;
            using (ContextoDB DataObj = new ContextoDB(DBCnn))
            {

                DatosReader = DataObj.EjecutaSP(ConfiguracionGlobalSP.getConfiguracionGlobal);
                try
                {
                    while (DatosReader.Read())
                    {
                        ConfiguracionGlobalEntity Config = new ConfiguracionGlobalEntity();
                        Config.ID = Convert.ToInt32(DatosReader["ID"].ToString());
                        Config.Nombre = DatosReader["Nombre"].ToString();
                        Config.Valor = DatosReader["Valor"].ToString();
                        Config.Descripcion = DatosReader["Descripcion"].ToString();
                        Config.Activo = Convert.ToBoolean(DatosReader["Activo"].ToString());
                        Lista.Add(Config);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            return Lista;
        }
    }
}

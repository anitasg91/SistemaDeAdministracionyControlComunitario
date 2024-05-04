using SAyCC.Data.StoredProcedures;
using SAyCC.Entities.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace SAyCC.Data.Repository
{
    public class CommonRepository : IDisposable
    {
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        public List<ConfiguracionGlobalEntity> getConfiguracionGlobal(string DBCnn, string Nombre)
        {
            SqlDataReader DatosReader;
            using (ContextoDB DataObj = new ContextoDB(DBCnn))
            {
                SqlParameter[] _Parametros = new SqlParameter[]
                {
                        new SqlParameter("Nombre",SqlDbType.VarChar){Value = Nombre },
                };
                DatosReader = DataObj.EjecutaSP(CommonSP.GetConfiguracionGlobal, _Parametros);
                List<ConfiguracionGlobalEntity> Lista = new List<ConfiguracionGlobalEntity>();
                while (DatosReader.Read())
                {
                    ConfiguracionGlobalEntity ent = new ConfiguracionGlobalEntity();
                    ent.ID = Convert.ToInt32(DatosReader["Id"].ToString());
                    ent.Nombre = DatosReader["Nombre"].ToString();
                    ent.Valor = DatosReader["Valor"].ToString();
                    ent.Descripcion = DatosReader["Descripcion"].ToString();
                    ent.Activo = Convert.ToBoolean(DatosReader["Activo"].ToString());

                    Lista.Add(ent);
                }
                return Lista;
            }
        }

    }
}

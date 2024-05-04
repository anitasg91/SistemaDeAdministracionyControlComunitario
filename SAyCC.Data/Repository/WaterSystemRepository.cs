using SAyCC.Data.StoredProcedures;
using SAyCC.Entities.Common;
using SAyCC.Entities.WaterSystem;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace SAyCC.Data.Repository
{
   public class WaterSystemRepository:IDisposable
    {
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        public List<WaterMeterEntity> GetWaterMeter(string DBCnn, int? Id, int? Numero)
        {
            SqlDataReader DatosReader;
            using (ContextoDB DataObj = new ContextoDB(DBCnn))
            {
                SqlParameter[] _Parametros = new SqlParameter[]
                {
                        new SqlParameter("Id",SqlDbType.Int){Value = Id },
                        new SqlParameter("Numero",SqlDbType.Int){Value = Numero },
                };
                DatosReader = DataObj.EjecutaSP(WaterSystemSP.GetWaterMeter, _Parametros);
                List<WaterMeterEntity> Lista = new List<WaterMeterEntity>();
                while (DatosReader.Read())
                {
                    WaterMeterEntity ent = new WaterMeterEntity();
                    ent.Id = Convert.ToInt32(DatosReader["Id"].ToString());
                    ent.Numero = DatosReader["Numero"].ToString();
                    ent.FechaAlta = Convert.ToDateTime(DatosReader["FechaAlta"].ToString());
                    ent.FechaBaja = string.IsNullOrEmpty(DatosReader["FechaBaja"].ToString()) ? new DateTime?() : Convert.ToDateTime(DatosReader["FechaBaja"].ToString());
                    ent.LecturaActual = Convert.ToDecimal(DatosReader["LecturaActual"].ToString());
                    ent.LecturaAnterior = Convert.ToDecimal(DatosReader["LecturaAnterior"].ToString());
                    ent.IdTitular = string.IsNullOrEmpty(DatosReader["IdTitular"].ToString()) ? new int?(): Convert.ToInt32(DatosReader["IdTitular"].ToString());
                    ent.NombreTitular = DatosReader["NombreTitular"].ToString();
                    ent.Activo = Convert.ToBoolean(DatosReader["Activo"].ToString());
                    ent.IdManzana = Convert.ToInt32(DatosReader["IdManzana"].ToString());
                    ent.Ubicacion = DatosReader["Ubicacion"].ToString();
                    ent.FechaLectura = Convert.ToDateTime(DatosReader["FechaLectura"].ToString());

                    Lista.Add(ent);
                }
                return Lista;
            }
        }
        #region Catalogos
        public CatalogEntityWS GetCatalog(string DBCnn, int IDCatalog, int? Id)
        {
            SqlDataReader DatosReader;
            using (ContextoDB DataObj = new ContextoDB(DBCnn))
            {
                SqlParameter[] _Parametros = new SqlParameter[]
                {
                        new SqlParameter("IDCatalog",SqlDbType.Int){Value = IDCatalog },
                        new SqlParameter("Id",SqlDbType.Int){Value = Id },
                };
                DatosReader = DataObj.EjecutaSP(WaterSystemSP.GetCatalog, _Parametros);
                CatalogEntityWS catalogo = mapCatalog(DatosReader, IDCatalog);
                return catalogo;
            }
        }
        public CatalogEntityWS mapCatalog(SqlDataReader DatosReader, int IDCatalog)
        {
                CatalogEntityWS ent = new CatalogEntityWS();
            ent.TipoLectura = new List<TipoLectura>();
            ent.Folio = new List<Folio>();

            while (DatosReader.Read())
            {
                switch (IDCatalog)
                {
                    case (int)Enumerador.CatalogTypeWaterSystem.TipoLectura:
                        TipoLectura entity = new TipoLectura();
                        entity.Id = Convert.ToInt32(DatosReader["Id"].ToString());
                        entity.Descripcion = DatosReader["Descripcion"].ToString();
                        entity.Inicial = Convert.ToInt32(DatosReader["Inicial"].ToString());
                        entity.Limite = string.IsNullOrEmpty(DatosReader["Limite"].ToString()) ? new int?(): Convert.ToInt32(DatosReader["Limite"].ToString());
                        entity.Precio = Convert.ToInt32(DatosReader["Precio"].ToString());
                        ent.TipoLectura.Add(entity);
                        break;
                    case (int)Enumerador.CatalogTypeWaterSystem.Folio:
                        Folio folio = new Folio();
                        folio.Id = Convert.ToInt32(DatosReader["Id"].ToString());
                        folio.IdManzana = Convert.ToInt32(DatosReader["IdManzana"].ToString());
                        folio.Consecutivo = Convert.ToInt32(DatosReader["Consecutivo"].ToString());
                        folio.FechaAlta = Convert.ToDateTime(DatosReader["FechaAlta"].ToString());
                        folio.IdUserAlta = Convert.ToInt32(DatosReader["IdUserAlta"].ToString());
                        folio.UsuarioAlta = DatosReader["usuarioAlta"].ToString();
                        ent.Folio.Add(folio);
                        break;
                    default:
                        break;
                }
            }

            return ent;
        }
        //public void DeleteCatalog(string DBCnn, int IDCatalog, int Id)
        //{
        //    try
        //    {
        //        //SqlDataReader DatosReader;
        //        using (ContextoDB DataObj = new ContextoDB(DBCnn))
        //        {
        //            SqlParameter[] _Parametros = new SqlParameter[]
        //            {
        //                new SqlParameter("IDCatalog",SqlDbType.Int){Value = IDCatalog },
        //                new SqlParameter("Id",SqlDbType.Int){Value = Id },
        //            };
        //            DataObj.EjecutaSP(AministrationAppSP.DeleteCatalog, _Parametros);
        //            //while (DatosReader.Read())
        //            //{
        //            //    ent.Descripcion = DatosReader["Descripcion"].ToString();
        //            //}
        //        }
        //    }
        //    catch (SqlException ex)
        //    {
        //        throw ex;

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //public void SaveCatalog(string DBCnn, int IDCatalog, int Id, string Descripcion)
        //{
        //    try
        //    {
        //        using (ContextoDB DataObj = new ContextoDB(DBCnn))
        //        {
        //            SqlParameter[] _Parametros = new SqlParameter[]
        //            {
        //                new SqlParameter("IDCatalog",SqlDbType.Int){Value = IDCatalog },
        //                new SqlParameter("Id",SqlDbType.Int){Value = Id },
        //                new SqlParameter("Descripcion",SqlDbType.VarChar){Value = Descripcion },
        //            };
        //            DataObj.EjecutaSP(AministrationAppSP.SaveCatalog, _Parametros);
        //        }
        //    }
        //    catch (SqlException ex)
        //    {
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        #endregion

        public List<VoucherEntity> GetPaymentVoucher(string DBCnn, int? Id)
        {
            SqlDataReader DatosReader;
            using (ContextoDB DataObj = new ContextoDB(DBCnn))
            {
                SqlParameter[] _Parametros = new SqlParameter[]
                {
                        new SqlParameter("Id",SqlDbType.Int){Value = Id },
                };
                DatosReader = DataObj.EjecutaSP(WaterSystemSP.GetReceiptByParameter, _Parametros);
                List< VoucherEntity> lista = mapVoucher(DatosReader);
                
                return lista;
            }
        }
        public List<VoucherEntity> mapVoucher(SqlDataReader DatosReader)
        {
            List<VoucherEntity> lista = new List<VoucherEntity>();

            while (DatosReader.Read())
            {
                VoucherEntity entity = new VoucherEntity();
                entity.Id = Convert.ToInt32(DatosReader["Id"].ToString());
                entity.IdMedidor = Convert.ToInt32(DatosReader["IdMedidor"].ToString());
                entity.IdUsuario = Convert.ToInt32(DatosReader["IdUsuario"].ToString());
                entity.IdMes = Convert.ToInt32(DatosReader["IdMes"].ToString());
                entity.Estado = DatosReader["Estado"].ToString();
                entity.Fecha = Convert.ToDateTime(DatosReader["Fecha"].ToString());
                entity.Folio = DatosReader["Folio"].ToString();
                entity.Periodo = DatosReader["Periodo"].ToString();
                entity.LecturaAnt = Convert.ToInt32(DatosReader["LecturaAnt"].ToString());
                entity.LecturaAct = Convert.ToInt32(DatosReader["LecturaAct"].ToString());
                entity.FechaRegistro = Convert.ToDateTime(DatosReader["FechaRegistro"].ToString());
                entity.TotalImUsado = Convert.ToInt32(DatosReader["TotalImUsado"].ToString());
                entity.TotalPagado = Convert.ToDecimal(DatosReader["TotalPagado"].ToString());
                entity.Titular = DatosReader["Titular"].ToString();
                entity.ClaveTitular = DatosReader["ClaveTitular"].ToString();
                entity.DireccionMedidor = DatosReader["DireccionMedidor"].ToString();
                entity.NoMedidor = DatosReader["NoMedidor"].ToString();
                entity.Manzana = DatosReader["Manzana"].ToString();
                entity.Pagado = Convert.ToBoolean(DatosReader["Pagado"].ToString());
                
                lista.Add(entity);
            }

            return lista;
        }

        public List<BreakdownEntity> GetbreakdownByIdComprobante(string DBCnn, int IdComprobante)
        {
            SqlDataReader DatosReader;
            using (ContextoDB DataObj = new ContextoDB(DBCnn))
            {
                SqlParameter[] _Parametros = new SqlParameter[]
                {
                        new SqlParameter("@IdComprobante",SqlDbType.Int){Value = IdComprobante },
                };
                DatosReader = DataObj.EjecutaSP(WaterSystemSP.GetbreakdownByIdComprobante, _Parametros);
                List<BreakdownEntity> lista = new List<BreakdownEntity>();
                while (DatosReader.Read())
                {
                    BreakdownEntity entity = new BreakdownEntity();
                    entity.Identificador = Convert.ToInt32(DatosReader["Identificador"].ToString());
                    entity.Concepto = DatosReader["Concepto"].ToString();
                    entity.MetrosUsados = Convert.ToInt32(DatosReader["MetrosUsados"].ToString());
                    entity.TotalPagar = Convert.ToDecimal(DatosReader["TotalPagar"].ToString());
                    lista.Add(entity);
                }
                return lista;
            }
        }

        public List<GraphicDataEntity> GetGraphicData(string DBCnn, int IdComprobante)
        {
            SqlDataReader DatosReader;
            using (ContextoDB DataObj = new ContextoDB(DBCnn))
            {
                SqlParameter[] _Parametros = new SqlParameter[]
                {
                        new SqlParameter("@IdComprobante",SqlDbType.BigInt){Value = IdComprobante },
                };
                DatosReader = DataObj.EjecutaSP(WaterSystemSP.GetGraphicData, _Parametros);
                List<GraphicDataEntity> lista = new List<GraphicDataEntity>();
                while (DatosReader.Read())
                {
                    GraphicDataEntity entity = new GraphicDataEntity();
                    entity.IdComprobante = Convert.ToInt32(DatosReader["IdComprobante"].ToString());
                    entity.TotalMUsados = Convert.ToInt32(DatosReader["TotalMUsados"].ToString());
                    entity.TotalPagado  = Convert.ToDecimal(DatosReader["TotalPagado"].ToString());
                    entity.Fecha = DatosReader["Fecha"].ToString();
                    lista.Add(entity);
                }
                return lista;
            }
        }
    }
}

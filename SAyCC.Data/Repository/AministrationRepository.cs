using SAyCC.Data.StoredProcedures;
using SAyCC.Entities.Login;
using SAyCC.Entities.SystemAdmin;
using SAyCC.Entities.WaterSystem;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace SAyCC.Data.Repository
{
    public class AministrationRepository : IDisposable
    {
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #region Usuarios
        public List<UserEntity> GetUsers(string DBCnn)
        {
            SqlDataReader DatosReader;
            using (ContextoDB DataObj = new ContextoDB(DBCnn))
            {
                DatosReader = DataObj.EjecutaSP(AministrationAppSP.GetUsers);
                List<UserEntity> Lista = new List<UserEntity>();
                while (DatosReader.Read())
                {
                    Lista.Add(mapUser(DatosReader));
                }
                return Lista;
            }
        }
        public UserEntity GetUserDetailById(string DBCnn, int IdUser)
        {
            try
            {
                using (ContextoDB DataObj = new ContextoDB(DBCnn))
                {
                    SqlParameter[] _Parametros = new SqlParameter[]
                    {
                        new SqlParameter("IdUser",SqlDbType.Int){Value = IdUser },
                    };
                    var DatosReader = DataObj.EjecutaSP(AministrationAppSP.GetUserDetailById, _Parametros);
                    UserEntity user = new UserEntity();
                    while (DatosReader.Read())
                    {
                        user = mapUser(DatosReader);
                    }
                    return user;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<UserEntity> getUserByParameter(string DBCnn, string name)
        {
            try
            {
                using (ContextoDB DataObj = new ContextoDB(DBCnn))
                {
                    SqlParameter[] _Parametros = new SqlParameter[]
                    {
                        new SqlParameter("name",SqlDbType.VarChar){Value = name },
                    };
                    var DatosReader = DataObj.EjecutaSP(AministrationAppSP.GetUserByParameter, _Parametros);
                    List<UserEntity> Lista = new List<UserEntity>();
                    while (DatosReader.Read())
                    {
                        Lista.Add(mapUser(DatosReader));
                    }
                    return Lista;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public UserEntity mapUser(SqlDataReader DatosReader) {
            UserEntity user = new UserEntity();
            user.Id = Convert.ToInt32(DatosReader["Id"].ToString());
            user.Nombre = DatosReader["Nombre"].ToString();
            user.APaterno = DatosReader["APaterno"].ToString();
            user.AMaterno = DatosReader["AMaterno"].ToString();
            user.Telefono = DatosReader["Telefono"].ToString();
            user.Email = DatosReader["Email"].ToString();
            user.FechaAlta = Convert.ToDateTime(DatosReader["FechaAlta"].ToString());
            user.IdUsuarioAlta = Convert.ToInt32(DatosReader["IdUsuarioAlta"].ToString());
            user.PrimerLogin = Convert.ToBoolean(DatosReader["PrimerLogin"].ToString());
            user.InicioSesion = string.IsNullOrEmpty(DatosReader["InicioSesion"].ToString()) ? new DateTime?() : Convert.ToDateTime(DatosReader["InicioSesion"].ToString());
            user.SesionActiva = Convert.ToBoolean(DatosReader["SesionActiva"].ToString());
            user.NoIntentosFallidos = Convert.ToInt32(DatosReader["NoIntentosFallidos"].ToString());
            user.Activo = Convert.ToBoolean(DatosReader["Activo"].ToString());
            user.FechaMod = string.IsNullOrEmpty(DatosReader["FechaMod"].ToString()) ? new DateTime?() : Convert.ToDateTime(DatosReader["FechaMod"].ToString());
            user.IdUsuarioMod = string.IsNullOrEmpty(DatosReader["IdUsuarioMod"].ToString()) ? new int?() : Convert.ToInt32(DatosReader["IdUsuarioMod"].ToString());
            user.Sexo = Convert.ToBoolean(DatosReader["Sexo"].ToString());
            user.IdManzana = Convert.ToInt32(DatosReader["IdManzana"].ToString());
            user.IdPerfil = Convert.ToInt32(DatosReader["IdPerfil"].ToString());
            user.ImagenUpload = DatosReader["ImagenUpload"].ToString();
            user.FechaNacimiento = Convert.ToDateTime(DatosReader["FechaNacimiento"].ToString());
            user.Perfil = DatosReader["Perfil"].ToString();
            user.Manzana = DatosReader["Manzana"].ToString();
            user.Identificador = DatosReader["Identificador"].ToString();
            user.Usuario = DatosReader["Usuario"].ToString();
            return user;
        }
        public string saveUser(string DBCnn, UserEntity entidad, ref int Id)
        {
            string user = "";
            try
            {
                SqlDataReader DatosReader;
                using (ContextoDB DataObj = new ContextoDB(DBCnn))
                {
                    SqlParameter[] _Parametros = new SqlParameter[]
                    {
                        new SqlParameter("Id",SqlDbType.Int){Value = entidad.Id },
                        new SqlParameter("IdManzana",SqlDbType.Int){Value = entidad.IdManzana },
                        new SqlParameter("IdPerfil",SqlDbType.Int){Value = entidad.IdPerfil },
                        new SqlParameter("IdUsuarioAlta",SqlDbType.Int){Value = entidad.IdUsuarioAlta },
                        new SqlParameter("IdUsuarioMod",SqlDbType.Int){Value = entidad.IdUsuarioMod },
                        new SqlParameter("Nombre",SqlDbType.VarChar){Value = entidad.Nombre },
                        new SqlParameter("APaterno",SqlDbType.VarChar){Value = entidad.APaterno },
                        new SqlParameter("AMaterno",SqlDbType.VarChar){Value = entidad.AMaterno },
                        new SqlParameter("Telefono",SqlDbType.VarChar){Value = entidad.Telefono },
                        new SqlParameter("Email",SqlDbType.VarChar){Value = entidad.Email },
                        new SqlParameter("Passw",SqlDbType.VarChar){Value = entidad.Password },
                        new SqlParameter("Sexo",SqlDbType.Bit){Value = entidad.Genero },
                        new SqlParameter("ImagenUpload",SqlDbType.VarChar){Value = entidad.ImagenUpload },
                        new SqlParameter("FechaNacimiento",SqlDbType.DateTime){Value = entidad.FechaNacimiento },
                        new SqlParameter("CreateUser",SqlDbType.Bit){Value = entidad.CreateUser },
                    };
                    DatosReader = DataObj.EjecutaSP(AministrationAppSP.SaveUser, _Parametros);
                    while (DatosReader.Read())
                    {
                        user = DatosReader["Usuario"].ToString();
                        Id = int.Parse(DatosReader["Id"].ToString());
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return user;
        }
        #endregion

        #region Aplicación
        public List<ApplicationEntity> getApplications(string DBCnn, int? Id)
        {
            SqlDataReader DatosReader;
            using (ContextoDB DataObj = new ContextoDB(DBCnn))
            {
                SqlParameter[] _Parametros = new SqlParameter[]
                {
                        new SqlParameter("Id",SqlDbType.Int){Value = Id },
                };
                DatosReader = DataObj.EjecutaSP(AministrationAppSP.getApplications, _Parametros);
                List<ApplicationEntity> ListaAplicacion = new List<ApplicationEntity>();
                while (DatosReader.Read())
                {
                    ApplicationEntity Aplicacion = new ApplicationEntity();
                    Aplicacion.Id = Convert.ToInt32(DatosReader["Id"].ToString());
                    Aplicacion.Descripcion = DatosReader["Descripcion"].ToString();
                    Aplicacion.Activo = Convert.ToBoolean(DatosReader["Activo"].ToString());
                    Aplicacion.FechaAlta = Convert.ToDateTime(DatosReader["FechaAlta"].ToString());
                    Aplicacion.IdUsuarioAlta = Convert.ToInt32(DatosReader["IdUsuarioAlta"].ToString());
                    Aplicacion.FechaModificacion = string.IsNullOrEmpty(DatosReader["FechaModificacion"].ToString()) ? new DateTime?() : Convert.ToDateTime(DatosReader["FechaModificacion"].ToString());
                    Aplicacion.IdUsuarioModificacion = string.IsNullOrEmpty(DatosReader["IdUsuarioModificacion"].ToString()) ? new int?() : Convert.ToInt32(DatosReader["IdUsuarioModificacion"].ToString());
                    Aplicacion.Icono = DatosReader["Icono"].ToString();
                    Aplicacion.Accion = DatosReader["Accion"].ToString();
                    Aplicacion.Controlador = DatosReader["Controlador"].ToString();
                    Aplicacion.Dominio = DatosReader["Dominio"].ToString();
                    Aplicacion.NombreUsuarioAlta = DatosReader["NombreUsuarioAlta"].ToString();
                    Aplicacion.NombreUsuarioMod = DatosReader["NombreUsuarioMod"].ToString();
                    ListaAplicacion.Add(Aplicacion);
                }
                return ListaAplicacion;
            }
        }
        public void saveApplication(string DBCnn, ApplicationEntity entidad)
        {
            try
            {
                using (ContextoDB DataObj = new ContextoDB(DBCnn))
                {
                    SqlParameter[] _Parametros = new SqlParameter[]
                    {
                        new SqlParameter("Id",SqlDbType.Int){Value = entidad.Id },
                        new SqlParameter("Descripcion",SqlDbType.VarChar){Value = entidad.Descripcion },
                        new SqlParameter("IdUsuarioAlta",SqlDbType.Int){Value = entidad.IdUsuarioAlta },
                        new SqlParameter("IdUsuarioModificacion",SqlDbType.Int){Value = entidad.IdUsuarioModificacion },
                        new SqlParameter("Icono",SqlDbType.VarChar){Value = entidad.Icono },
                        new SqlParameter("Accion",SqlDbType.VarChar){Value = entidad.Accion },
                        new SqlParameter("Controlador",SqlDbType.VarChar){Value = entidad.Controlador },
                        new SqlParameter("Dominio",SqlDbType.VarChar){Value = entidad.Dominio },
                    };
                    DataObj.EjecutaSP(AministrationAppSP.SaveAplicacion, _Parametros);
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public void changeStatusApplication(string DBCnn, int Id, bool Status, int IdUsuarioModificacion)
        {
            try
            {
                using (ContextoDB DataObj = new ContextoDB(DBCnn))
                {
                    SqlParameter[] _Parametros = new SqlParameter[]
                    {
                        new SqlParameter("Id",SqlDbType.Int){Value = Id },
                        new SqlParameter("Status",SqlDbType.VarChar){Value = Status },
                        new SqlParameter("IdUsuarioModificacion",SqlDbType.VarChar){Value = IdUsuarioModificacion },
                    };
                    DataObj.EjecutaSP(AministrationAppSP.ChangeStatusApplication, _Parametros);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public void DeleteApplication(string DBCnn, int Id)
        {
            try
            {
                using (ContextoDB DataObj = new ContextoDB(DBCnn))
                {
                    SqlParameter[] _Parametros = new SqlParameter[]
                    {
                        new SqlParameter("IdApp",SqlDbType.Int){Value = Id },
                    };
                    DataObj.EjecutaSP(AministrationAppSP.DeleteApplication, _Parametros);
                }
            }
            catch (SqlException ex)
            {
                throw ex;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Modulos
        public List<ModuleEntity> GetModulesAllowed(string DBCnn, int? IdPerfil, int? IdApp)
        {
            SqlDataReader DatosReader;
            using (ContextoDB DataObj = new ContextoDB(DBCnn))
            {
                SqlParameter[] _Parametros = new SqlParameter[]
                {
                        new SqlParameter("IdApp",SqlDbType.Int){Value = IdApp },
                        new SqlParameter("IdPerfil",SqlDbType.Int){Value = IdPerfil },
                };
                DatosReader = DataObj.EjecutaSP(AministrationAppSP.GetModules, _Parametros);
                List<ModuleEntity> ListaAplicacion = new List<ModuleEntity>();
                while (DatosReader.Read())
                {
                    ModuleEntity Mod = new ModuleEntity();
                    Mod.Id = Convert.ToInt32(DatosReader["Id"].ToString());
                    Mod.Titulo = DatosReader["Titulo"].ToString();
                    Mod.Descripcion = DatosReader["Descripcion"].ToString();
                    Mod.Controlador = DatosReader["Controlador"].ToString();
                    Mod.Accion = DatosReader["Accion"].ToString();
                    Mod.Activo = Convert.ToBoolean(DatosReader["Activo"].ToString());
                    Mod.FechaAlta = Convert.ToDateTime(DatosReader["FechaAlta"].ToString());
                    Mod.IdUsuarioAlta = Convert.ToInt32(DatosReader["IdUsuarioAlta"].ToString());
                    Mod.FechaModificacion = string.IsNullOrEmpty(DatosReader["FechaModificacion"].ToString()) ? new DateTime?() : Convert.ToDateTime(DatosReader["FechaModificacion"].ToString());
                    Mod.IdUsuarioModificacion = string.IsNullOrEmpty(DatosReader["IdUsuarioModificacion"].ToString()) ? new int?() : Convert.ToInt32(DatosReader["IdUsuarioModificacion"].ToString());
                    Mod.i_class = DatosReader["i_class"].ToString();
                    Mod.Orden = int.Parse(DatosReader["Orden"].ToString());
                    Mod.IdAplicacion = int.Parse(DatosReader["IdAplicacion"].ToString());
                    Mod.Aplicacion = DatosReader["Aplicacion"].ToString();
                    Mod.NombreUsuarioAlta = DatosReader["NombreUsuarioAlta"].ToString();
                    Mod.NombreUsuarioMod = DatosReader["NombreUsuarioMod"].ToString();
                    Mod.Alta = Convert.ToBoolean(DatosReader["Alta"].ToString());
                    Mod.Modificacion = Convert.ToBoolean(DatosReader["Modificacion"].ToString());
                    Mod.Consulta = Convert.ToBoolean(DatosReader["Consulta"].ToString());
                    ListaAplicacion.Add(Mod);
                }
                return ListaAplicacion;
            }
        }
        public void saveModules(string DBCnn, int id, int IdApp, string Titulo, string Descripcion, string Icono, string Controlador, string Accion, int Orden, int? IdUsuarioAlta, int? IdUsuarioModificacion)
        {
            try
            {
                using (ContextoDB DataObj = new ContextoDB(DBCnn))
                {
                    SqlParameter[] _Parametros = new SqlParameter[]
                    {
                        new SqlParameter("IdModulo",SqlDbType.Int){Value = id },
                        new SqlParameter("IdApp",SqlDbType.Int){Value = IdApp },
                        new SqlParameter("Titulo",SqlDbType.VarChar){Value = Titulo },
                        new SqlParameter("Descripcion",SqlDbType.VarChar){Value = Descripcion },
                        new SqlParameter("Icono",SqlDbType.VarChar){Value = Icono },
                        new SqlParameter("Controlador",SqlDbType.VarChar){Value = Controlador },
                        new SqlParameter("Accion",SqlDbType.VarChar){Value = Accion },
                        new SqlParameter("Orden",SqlDbType.Int){Value = Orden },
                        new SqlParameter("IdUsuarioAlta",SqlDbType.Int){Value = IdUsuarioAlta },
                        new SqlParameter("IdUsuarioModificacion",SqlDbType.Int){Value = IdUsuarioModificacion },
                    };
                    DataObj.EjecutaSP(AministrationAppSP.SaveModule, _Parametros);
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public void changeStatusModule(string DBCnn, int Id, bool Status, int IdUsuarioModificacion)
        {
            try
            {
                using (ContextoDB DataObj = new ContextoDB(DBCnn))
                {
                    SqlParameter[] _Parametros = new SqlParameter[]
                    {
                        new SqlParameter("Id",SqlDbType.Int){Value = Id },
                        new SqlParameter("Status",SqlDbType.VarChar){Value = Status },
                        new SqlParameter("IdUsuarioModificacion",SqlDbType.VarChar){Value = IdUsuarioModificacion },
                    };
                    DataObj.EjecutaSP(AministrationAppSP.ChangeStatusModule, _Parametros);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public void DeleteModule(string DBCnn, int Id)
        {
            try
            {
                using (ContextoDB DataObj = new ContextoDB(DBCnn))
                {
                    SqlParameter[] _Parametros = new SqlParameter[]
                    {
                        new SqlParameter("IdModule",SqlDbType.Int){Value = Id },
                    };
                    DataObj.EjecutaSP(AministrationAppSP.DeleteModule, _Parametros);
                }
            }
            catch (SqlException ex)
            {
                throw ex;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Perfiles
        public List<ProfileEntity> GetProfiles(string DBCnn, int? IdPerfil, int? IdApp)
        {
            SqlDataReader DatosReader;
            using (ContextoDB DataObj = new ContextoDB(DBCnn))
            {
                SqlParameter[] _Parametros = new SqlParameter[]
                {
                        new SqlParameter("IdPerfil",SqlDbType.Int){Value = IdPerfil },
                        new SqlParameter("IdApp",SqlDbType.Int){Value = IdApp },
                };
                DatosReader = DataObj.EjecutaSP(AministrationAppSP.GetProfiles, _Parametros);
                List<ProfileEntity> Lista = new List<ProfileEntity>();
                while (DatosReader.Read())
                {
                    ProfileEntity ent = new ProfileEntity();
                    ent.Id = Convert.ToInt32(DatosReader["Id"].ToString());
                    ent.Detalle = DatosReader["Detalle"].ToString();
                    ent.Activo = Convert.ToBoolean(DatosReader["Activo"].ToString());
                    ent.FechaAlta = Convert.ToDateTime(DatosReader["FechaAlta"].ToString());
                    ent.IdUsuarioAlta = Convert.ToInt32(DatosReader["IdUsuarioAlta"].ToString());
                    ent.FechaModificacion = string.IsNullOrEmpty(DatosReader["FechaModificacion"].ToString()) ? new DateTime?() : Convert.ToDateTime(DatosReader["FechaModificacion"].ToString());
                    ent.IdUsuarioModificacion = string.IsNullOrEmpty(DatosReader["IdUsuarioModificacion"].ToString()) ? new int?() : Convert.ToInt32(DatosReader["IdUsuarioModificacion"].ToString());
                    ent.IDAplicacion = Convert.ToInt32(DatosReader["IDAplicacion"].ToString());
                    ent.Aplicacion = DatosReader["Aplicacion"].ToString();
                    ent.NombreUsuarioAlta = DatosReader["NombreUsuarioAlta"].ToString();
                    ent.NombreUsuarioMod = DatosReader["NombreUsuarioMod"].ToString();
                    Lista.Add(ent);
                }
                return Lista;
            }
        }
        public void saveProfiles(string DBCnn, int id, int IdApp, string Descripcion, int? IdUsuarioAlta, int? IdUsuarioModificacion)
        {
            try
            {
                using (ContextoDB DataObj = new ContextoDB(DBCnn))
                {
                    SqlParameter[] _Parametros = new SqlParameter[]
                    {
                        new SqlParameter("IdProfile",SqlDbType.Int){Value = id },
                        new SqlParameter("IdApp",SqlDbType.Int){Value = IdApp },
                        new SqlParameter("Descripcion",SqlDbType.VarChar){Value = Descripcion },
                        new SqlParameter("IdUsuarioAlta",SqlDbType.Int){Value = IdUsuarioAlta },
                        new SqlParameter("IdUsuarioModificacion",SqlDbType.Int){Value = IdUsuarioModificacion },
                    };
                    DataObj.EjecutaSP(AministrationAppSP.SaveProfiles, _Parametros);
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public void changeStatusProfile(string DBCnn, int Id, bool Status, int IdUsuarioModificacion)
        {
            try
            {
                using (ContextoDB DataObj = new ContextoDB(DBCnn))
                {
                    SqlParameter[] _Parametros = new SqlParameter[]
                    {
                        new SqlParameter("Id",SqlDbType.Int){Value = Id },
                        new SqlParameter("Status",SqlDbType.VarChar){Value = Status },
                        new SqlParameter("IdUsuarioModificacion",SqlDbType.VarChar){Value = IdUsuarioModificacion },
                    };
                    DataObj.EjecutaSP(AministrationAppSP.ChangeStatusProfile, _Parametros);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public void DeleteProfile(string DBCnn, int Id)
        {
            try
            {
                using (ContextoDB DataObj = new ContextoDB(DBCnn))
                {
                    SqlParameter[] _Parametros = new SqlParameter[]
                    {
                        new SqlParameter("IdProfile",SqlDbType.Int){Value = Id },
                    };
                    DataObj.EjecutaSP(AministrationAppSP.DeleteProfile, _Parametros);
                }
            }
            catch (SqlException ex)
            {
                throw ex;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<ProfileEntity> GetProfilesByAppActives(string DBCnn, int IdApp)
        {
            SqlDataReader DatosReader;
            using (ContextoDB DataObj = new ContextoDB(DBCnn))
            {
                SqlParameter[] _Parametros = new SqlParameter[]
                {
                        new SqlParameter("IdApp",SqlDbType.Int){Value = IdApp },
                };
                DatosReader = DataObj.EjecutaSP(AministrationAppSP.GetProfilesByAppActives, _Parametros);
                List<ProfileEntity> Lista = new List<ProfileEntity>();
                while (DatosReader.Read())
                {
                    ProfileEntity ent = new ProfileEntity();
                    ent.Id = Convert.ToInt32(DatosReader["Id"].ToString());
                    ent.Detalle = DatosReader["Detalle"].ToString();
                    Lista.Add(ent);
                }
                return Lista;
            }
        }
        #endregion

        #region Permisos
        public List<PermissionEntity> getPermission(string DBCnn, int? IdPerfil, int? IdApp)
        {
            SqlDataReader DatosReader;
            using (ContextoDB DataObj = new ContextoDB(DBCnn))
            {
                SqlParameter[] _Parametros = new SqlParameter[]
                {
                        new SqlParameter("IdApp",SqlDbType.Int){Value = IdApp },
                        new SqlParameter("IdPerfil",SqlDbType.Int){Value = IdPerfil },
                };
                DatosReader = DataObj.EjecutaSP(AministrationAppSP.GetPermission, _Parametros);
                List<PermissionEntity> ListaAplicacion = new List<PermissionEntity>();
                while (DatosReader.Read())
                {
                    PermissionEntity ent = new PermissionEntity();

                    ent.Id = Convert.ToInt32(DatosReader["Id"].ToString());
                    ent.IdPerfil = int.Parse(DatosReader["IdPerfil"].ToString());
                    ent.IdModulo = int.Parse(DatosReader["IdModulo"].ToString());
                    ent.Alta = Convert.ToBoolean(DatosReader["Alta"].ToString());
                    ent.Modificacion = Convert.ToBoolean(DatosReader["Modificacion"].ToString());
                    ent.Consulta = Convert.ToBoolean(DatosReader["Consulta"].ToString());
                    ent.Modulo = DatosReader["Modulo"].ToString();
                    ent.Aplicacion = DatosReader["Aplicacion"].ToString();
                    ent.Descripcion = DatosReader["Descripcion"].ToString();
                    ent.Perfil = DatosReader["Perfil"].ToString();
                    //ent.FechaAlta = Convert.ToDateTime(DatosReader["FechaAlta"].ToString());
                    // ent.IdUsuarioAlta = Convert.ToInt32(DatosReader["IdUsuarioAlta"].ToString());
                    // ent.FechaModificacion = string.IsNullOrEmpty(DatosReader["FechaModificacion"].ToString()) ? new DateTime?() : Convert.ToDateTime(DatosReader["FechaModificacion"].ToString());
                    // ent.IdUsuarioModificacion = string.IsNullOrEmpty(DatosReader["IdUsuarioModificacion"].ToString()) ? new int?() : Convert.ToInt32(DatosReader["IdUsuarioModificacion"].ToString());

                    ListaAplicacion.Add(ent);
                }
                return ListaAplicacion;
            }
        }
        public void savePermission(string DBCnn, List<PermissionEntity> Lista)
        {
            foreach (var entidad in Lista)
            {
                try
                {
                    if (!string.IsNullOrEmpty(entidad.strAlta) || !string.IsNullOrEmpty(entidad.strModificacion) || !string.IsNullOrEmpty(entidad.strConsulta) || entidad.Id != 0)
                    {
                        using (ContextoDB DataObj = new ContextoDB(DBCnn))
                        {
                            SqlParameter[] _Parametros = new SqlParameter[]{
                        new SqlParameter("Id",SqlDbType.Int){Value = entidad.Id },
                        new SqlParameter("IdPerfil",SqlDbType.Int){Value = entidad.IdPerfil },
                        new SqlParameter("IdModulo",SqlDbType.Int){Value = entidad.IdModulo },
                        new SqlParameter("Alta",SqlDbType.Bit){Value = entidad.strAlta == "on" },
                        new SqlParameter("Modificacion",SqlDbType.Bit){Value = entidad.strModificacion == "on" },
                        new SqlParameter("Consulta",SqlDbType.Bit){Value = entidad.strConsulta == "on" }
                      };
                            DataObj.EjecutaSP(AministrationAppSP.SavePermission, _Parametros);
                        }
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

        }
        #endregion

        #region Catalogos
        public List<Entities.SystemAdmin.CatalogEntity> GetCatalog(string DBCnn, int IDCatalog, int? Id)
        {
            SqlDataReader DatosReader;
            using (ContextoDB DataObj = new ContextoDB(DBCnn))
            {
                SqlParameter[] _Parametros = new SqlParameter[]
                {
                        new SqlParameter("IDCatalog",SqlDbType.Int){Value = IDCatalog },
                        new SqlParameter("Id",SqlDbType.Int){Value = Id },
                };
                DatosReader = DataObj.EjecutaSP(AministrationAppSP.GetCatalog, _Parametros);
                List<CatalogEntity> Lista = new List<CatalogEntity>();
                while (DatosReader.Read())
                {
                    CatalogEntity ent = new CatalogEntity();
                    ent.Id = Convert.ToInt32(DatosReader["Id"].ToString());
                    ent.Descripcion = DatosReader["Descripcion"].ToString();
                    Lista.Add(ent);
                }
                return Lista;
            }
        }
        public void DeleteCatalog(string DBCnn, int IDCatalog, int Id)
        {
            try
            {
                //SqlDataReader DatosReader;
                using (ContextoDB DataObj = new ContextoDB(DBCnn))
                {
                    SqlParameter[] _Parametros = new SqlParameter[]
                    {
                        new SqlParameter("IDCatalog",SqlDbType.Int){Value = IDCatalog },
                        new SqlParameter("Id",SqlDbType.Int){Value = Id },
                    };
                    DataObj.EjecutaSP(AministrationAppSP.DeleteCatalog, _Parametros);
                    //while (DatosReader.Read())
                    //{
                    //    ent.Descripcion = DatosReader["Descripcion"].ToString();
                    //}
                }
            }
            catch (SqlException ex)
            {
                throw ex;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void SaveCatalog(string DBCnn, int IDCatalog, int Id, string Descripcion)
        {
            try
            {
                using (ContextoDB DataObj = new ContextoDB(DBCnn))
                {
                    SqlParameter[] _Parametros = new SqlParameter[]
                    {
                        new SqlParameter("IDCatalog",SqlDbType.Int){Value = IDCatalog },
                        new SqlParameter("Id",SqlDbType.Int){Value = Id },
                        new SqlParameter("Descripcion",SqlDbType.VarChar){Value = Descripcion },
                    };
                    DataObj.EjecutaSP(AministrationAppSP.SaveCatalog, _Parametros);
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Asignación
        public List<AsignacionAplicacionEntity> getAsignacionAplicacion(string DBCnn, int IdPerfil)
        {
            SqlDataReader DatosReader;
            using (ContextoDB DataObj = new ContextoDB(DBCnn))
            {
                SqlParameter[] _Parametros = new SqlParameter[]
                {
                        new SqlParameter("IdPerfil",SqlDbType.Int){Value = IdPerfil },
                };
                DatosReader = DataObj.EjecutaSP(AministrationAppSP.GetAsignacionAplicacion, _Parametros);
                List<AsignacionAplicacionEntity> ListaAplicacion = new List<AsignacionAplicacionEntity>();
                while (DatosReader.Read())
                {
                    AsignacionAplicacionEntity Aplicacion = new AsignacionAplicacionEntity();
                    Aplicacion.Id = Convert.ToInt32(DatosReader["Id"].ToString());
                    Aplicacion.IdPerfil = Convert.ToInt32(DatosReader["IdPerfil"].ToString());
                    Aplicacion.IdAplicacion = Convert.ToInt32(DatosReader["IdAplicacion"].ToString());
                    Aplicacion.NombreApp = DatosReader["NombreApp"].ToString();
                    Aplicacion.Activo = Convert.ToBoolean(DatosReader["Activo"].ToString());
                    ListaAplicacion.Add(Aplicacion);
                }
                return ListaAplicacion;
            }
        }
        public void saveAsignacionAplicacion(string DBCnn, int Id, int IdProfile, int IdApp)
        {
            try
            {
                using (ContextoDB DataObj = new ContextoDB(DBCnn))
                {
                    SqlParameter[] _Parametros = new SqlParameter[]
                    {
                        new SqlParameter("Id",SqlDbType.Int){Value = Id },
                        new SqlParameter("IdProfile",SqlDbType.Int){Value = IdProfile },
                        new SqlParameter("IdApp",SqlDbType.Int){Value = IdApp },
                    };
                    DataObj.EjecutaSP(AministrationAppSP.saveAsignacionAplicacion, _Parametros);
                }
            }
            catch (SqlException ex)
            {
                throw ex;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        public void SaveWaterMeter(string DBCnn, WaterMeterEntity entity)
        {
            try
            {
                using (ContextoDB DataObj = new ContextoDB(DBCnn))
                {
                    SqlParameter[] _Parametros = new SqlParameter[]
                    {
                        new SqlParameter("Id",SqlDbType.Int){Value = entity.Id },
                        new SqlParameter("Numero",SqlDbType.Int){Value = entity.Numero },
                        new SqlParameter("LecturaActual", SqlDbType.Decimal){Value = entity.LecturaActual },
                        new SqlParameter("LecturaAnterior", SqlDbType.Decimal){Value = entity.LecturaAnterior },
                        new SqlParameter("IdTitular",SqlDbType.Int){Value = entity.IdTitular },
                        new SqlParameter("IdManzana",SqlDbType.Int){Value = entity.IdManzana },
                    };
                    DataObj.EjecutaSP(AministrationAppSP.SaveWaterMeter, _Parametros);
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteWaterMeterById(string DBCnn, int IdWaterMeter)
        {
            try
            {
                using (ContextoDB DataObj = new ContextoDB(DBCnn))
                {
                    SqlParameter[] _Parametros = new SqlParameter[]
                    {
                        new SqlParameter("IdWaterMeter",SqlDbType.Int){Value = IdWaterMeter },
                    };
                    DataObj.EjecutaSP(AministrationAppSP.DeleteWaterMeterById, _Parametros);
                }
            }
            catch (SqlException ex)
            {
                throw ex;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateWaterMeterById(string DBCnn, int IdWaterMeter, int ModifyTypeId, int? IdTitular)
        {
            try
            {
                using (ContextoDB DataObj = new ContextoDB(DBCnn))
                {
                    SqlParameter[] _Parametros = new SqlParameter[]
                    {
                        new SqlParameter("IdWaterMeter",SqlDbType.Int){Value = IdWaterMeter },
                        new SqlParameter("ModifyTypeId",SqlDbType.Int){Value = ModifyTypeId },
                        new SqlParameter("IdTitular",SqlDbType.Int){Value = IdTitular },
                    };
                    DataObj.EjecutaSP(AministrationAppSP.UpdateWaterMeterById, _Parametros);
                }
            }
            catch (SqlException ex)
            {
                throw ex;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<WaterMeterEntity> GetWaterMeterByIdUser(string DBCnn, int IdUser)
        {
            try
            {
                using (ContextoDB DataObj = new ContextoDB(DBCnn))
                {
                    SqlParameter[] _Parametros = new SqlParameter[]
                    {
                        new SqlParameter("IdUser",SqlDbType.Int){Value = IdUser },
                    };
                    var DatosReader = DataObj.EjecutaSP(AministrationAppSP.GetWaterMeterByIdUser, _Parametros);
                    List<WaterMeterEntity> lista = new List<WaterMeterEntity>();
                    while (DatosReader.Read())
                    {
                     WaterMeterEntity meter = new WaterMeterEntity();
                        meter.Id = Convert.ToInt32(DatosReader["Id"].ToString());
                        meter.Numero = DatosReader["Numero"].ToString();
                        meter.FechaAlta = Convert.ToDateTime(DatosReader["FechaAlta"].ToString());
                        meter.FechaBaja = string.IsNullOrEmpty(DatosReader["FechaBaja"].ToString()) ? new DateTime?(): Convert.ToDateTime(DatosReader["FechaBaja"].ToString());
                        meter.LecturaActual = Convert.ToDecimal(DatosReader["LecturaActual"].ToString());
                        meter.LecturaAnterior = Convert.ToDecimal(DatosReader["LecturaAnterior"].ToString());
                        meter.IdTitular = Convert.ToInt32(DatosReader["IdTitular"].ToString());
                        meter.Activo = Convert.ToBoolean(DatosReader["Activo"].ToString());
                        meter.FechaLectura = Convert.ToDateTime(DatosReader["FechaLectura"].ToString());
                        lista.Add(meter);
                    }
                    return lista;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

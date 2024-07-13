using Newtonsoft.Json;
using SAyCC.Data.StoredProcedures;
using SAyCC.Entities;
using SAyCC.Entities.Login;
using SAyCC.Entities.SystemAdmin;
using SAyCC.Entities.WaterSystem;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static SAyCC.Entities.Common.Enumerador;

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
            //user.IdPerfil = Convert.ToInt32(DatosReader["IdPerfil"].ToString());
            user.ImagenUpload = DatosReader["ImagenUpload"].ToString();
            user.FechaNacimiento = Convert.ToDateTime(DatosReader["FechaNacimiento"].ToString());
            user.Perfil = DatosReader["Perfil"].ToString();
            user.Manzana = DatosReader["Manzana"].ToString();
            user.Identificador = DatosReader["Identificador"].ToString();
            user.Usuario = DatosReader["Usuario"].ToString();
            user.IdEstatus = Convert.ToInt32(DatosReader["IdEstatus"].ToString());
            user.NombreEstatus = DatosReader["NombreEstatus"].ToString();
            user.HasApprovalSubdelegate = Convert.ToBoolean(DatosReader["HasApprovalSubdelegate"].ToString());
            user.HasApprovalJudge = Convert.ToBoolean(DatosReader["HasApprovalJudge"].ToString());
            user.HasApprovalTreasurer = Convert.ToBoolean(DatosReader["HasApprovalTreasurer"].ToString());


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
                        //new SqlParameter("IdPerfil",SqlDbType.Int){Value = entidad.IdPerfil },
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

        public void ChangeStatusUser(string DBCnn, int IdUsuario, int Estatus, string Motivo, int IdUsuarioModificacion)
        {
            try
            {
                using (ContextoDB DataObj = new ContextoDB(DBCnn))
                {
                    SqlParameter[] _Parametros = new SqlParameter[]
                    {
                        new SqlParameter(ParameterName.IdUsuario,SqlDbType.Int){Value = IdUsuario },
                        new SqlParameter(ParameterName.Estatus,SqlDbType.Int){Value = Estatus },
                        new SqlParameter(ParameterName.Motivo,SqlDbType.VarChar){Value = Motivo },
                        new SqlParameter(ParameterName.IdUsuarioModificacion,SqlDbType.VarChar){Value = IdUsuarioModificacion },
                    };
                    DataObj.EjecutaSP(AministrationAppSP.ChangeStatusUser, _Parametros);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public void ChangeStatusUserBySuperAdmin(string DBCnn, int IdUsuario, int Estatus, string Motivo, int IdUsuarioModificacion)
        {
            try
            {
                using (ContextoDB DataObj = new ContextoDB(DBCnn))
                {
                    SqlParameter[] _Parametros = new SqlParameter[]
                    {
                        new SqlParameter(ParameterName.IdUsuario,SqlDbType.Int){Value = IdUsuario },
                        new SqlParameter(ParameterName.Estatus,SqlDbType.Int){Value = Estatus },
                        new SqlParameter(ParameterName.Motivo,SqlDbType.VarChar){Value = Motivo },
                        new SqlParameter(ParameterName.IdUsuarioModificacion,SqlDbType.VarChar){Value = IdUsuarioModificacion },
                    };
                    DataObj.EjecutaSP(AministrationAppSP.ChangeStatusUserBySuperAdmin, _Parametros);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool DeleteUserForErrorToSave(string DBCnn, int Id)
        {
            bool eliminado = false;
            try
            {
                SqlDataReader DatosReader;
                using (ContextoDB DataObj = new ContextoDB(DBCnn))
                {
                    SqlParameter[] _Parametros = new SqlParameter[]
                    {
                        new SqlParameter("IdUsuario",SqlDbType.Int){Value = Id },
                    };
                    DatosReader = DataObj.EjecutaSP(AministrationAppSP.DeleteUserForErrorToSave, _Parametros);
                    while (DatosReader.Read())
                    {
                        eliminado = Convert.ToBoolean(DatosReader["eliminado"].ToString());
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return eliminado;
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
        public List<ModuleEntity> GetModulesAllowed(string DBCnn, int? IdUsuario, int? IdApp)
        {
            SqlDataReader DatosReader;
            using (ContextoDB DataObj = new ContextoDB(DBCnn))
            {
                SqlParameter[] _Parametros = new SqlParameter[]
                {
                        /*new SqlParameter("IdApp",SqlDbType.Int){Value = IdApp },
                        new SqlParameter("IdPerfil",SqlDbType.Int){Value = IdPerfil },*/
                        new SqlParameter("IdUsuario",SqlDbType.Int){Value = IdUsuario },
                        new SqlParameter("IdApp",SqlDbType.Int){Value = IdApp },
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
                    Mod.IDPadre = string.IsNullOrEmpty(DatosReader["IdPadre"].ToString()) ? new int?() : Convert.ToInt32(DatosReader["IdPadre"].ToString());
                    
                    Mod.Alta = true;
                    Mod.Modificacion = true;
                    Mod.Consulta = true;
                    /*Mod.Alta = Convert.ToBoolean(DatosReader["Alta"].ToString());
                    Mod.Modificacion = Convert.ToBoolean(DatosReader["Modificacion"].ToString());
                    Mod.Consulta = Convert.ToBoolean(DatosReader["Consulta"].ToString());*/
                    ListaAplicacion.Add(Mod);
                }
                return ListaAplicacion;
            }
        }
        public void saveModules(string DBCnn, int id, int IdApp, string Titulo, string Descripcion, string Icono, string Controlador, string Accion, int Orden, int? IdUsuarioAlta, int? IdUsuarioModificacion, int? IdPadre)
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
                        new SqlParameter("IdPadre",SqlDbType.Int){Value = IdPadre },
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
        public List<ModuleEntity> GetPagesFathers(string DBCnn, int IdApp)
        {
            SqlDataReader DatosReader;
            using (ContextoDB DataObj = new ContextoDB(DBCnn))
            {
                SqlParameter[] _Parametros = new SqlParameter[]
                {
                        new SqlParameter("IdApp",SqlDbType.Int){Value = IdApp },
                };
                DatosReader = DataObj.EjecutaSP(AministrationAppSP.GetPagesFathers, _Parametros);
                List<ModuleEntity> ListaAplicacion = new List<ModuleEntity>();
                while (DatosReader.Read())
                {
                    ModuleEntity Mod = new ModuleEntity();
                    Mod.Id = Convert.ToInt32(DatosReader["Id"].ToString());
                    Mod.Titulo = DatosReader["Titulo"].ToString();
                    ListaAplicacion.Add(Mod);
                }
                return ListaAplicacion;
            }
        }

        #endregion

        #region Perfiles
        public List<ProfileSesionEntity> GetRolesByUserAndAplication(string DBCnn, int IdUsuario, int IdApp)
        {
            SqlDataReader DatosReader;
            using (ContextoDB DataObj = new ContextoDB(DBCnn))
            {
                SqlParameter[] _Parametros = new SqlParameter[]
                {
                    new SqlParameter(ParameterName.IdUsuario,SqlDbType.Int){Value = IdUsuario },
                    new SqlParameter(ParameterName.IdApp,SqlDbType.Int){Value = IdApp },
                };
                DatosReader = DataObj.EjecutaSP(AministrationAppSP.GetRolesByUserAndAplication, _Parametros);
                List<ProfileSesionEntity> list = new List<ProfileSesionEntity>();
                while (DatosReader.Read())
                {
                    ProfileSesionEntity ent = new ProfileSesionEntity();
                    ent.Id = Convert.ToInt32(DatosReader[DataReaderColumn.Id].ToString());
                    ent.Detalle = DatosReader[DataReaderColumn.Detalle].ToString();
                    list.Add(ent);
                }
                return list;
            }
        }
        public List<ProfileEntity> GetProfiles(string DBCnn, int? IdPerfil, int? IdApp) /*Borrar*/
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

        public List<UserUtilityEntity> GetUsersListForPermissions(string DBCnn)
        {
            SqlDataReader DatosReader;
            using (ContextoDB DataObj = new ContextoDB(DBCnn))
            {
                DatosReader = DataObj.EjecutaSP(AministrationAppSP.GetUsers);
                List<UserUtilityEntity> Lista = new List<UserUtilityEntity>();
                while (DatosReader.Read())
                {
                    UserUtilityEntity user = new UserUtilityEntity();
                    user.Id = Convert.ToInt32(DatosReader["Id"].ToString());
                    user.Nombre = string.Concat(
                        DatosReader["Nombre"].ToString(), " ",
                        DatosReader["APaterno"].ToString(), " ",
                        DatosReader["AMaterno"].ToString());
                    user.Activo = Convert.ToBoolean(DatosReader["Activo"].ToString());

                    Lista.Add(user);
                }
                return Lista.OrderBy(_=>_.Nombre).Where(_=>_.Activo).ToList();
            }
        }

        public void SaveConfigPermissionsByUser(string DBCnn, PermissionsConfigRequest request)
        {
            try
            {
                DataTable rolesTable = ConvertirListaATabla(request, Relacion.UsuarioPerfil);
                DataTable blockTable = ConvertirListaATabla(request, Relacion.UsuarioManzana);

                using (ContextoDB DataObj = new ContextoDB(DBCnn))
                {
                    SqlParameter[] _Parametros = new SqlParameter[]{
                new SqlParameter("@RolesTable", SqlDbType.Structured) { Value = rolesTable, TypeName = "Administracion.TipoTablaPerfiles" },
                new SqlParameter("@BlockTable", SqlDbType.Structured) { Value = blockTable, TypeName = "Administracion.TipoTablaManzana" }
                };

                    DataObj.EjecutaSP(AministrationAppSP.SavePermissionsBulk, _Parametros);
                }
            }
            catch (Exception e)
            {

                throw  e;
            }

        }
        private DataTable ConvertirListaATabla(PermissionsConfigRequest request, Relacion opcion)
        {
            DataTable tabla = new DataTable();

            switch (opcion)
            {
                case Relacion.UsuarioPerfil:
                    tabla.Columns.Add("IdPerfil", typeof(int));
                    tabla.Columns.Add("IdUsuario", typeof(int));
                    tabla.Columns.Add("IdAplicacion", typeof(int));
                    tabla.Columns.Add("FechaCreacion", typeof(DateTime));

                    foreach (var item in request.Roles)
                    {
                        DataRow fila = tabla.NewRow();
                        fila["IdPerfil"] = item.IdPerfil;
                        fila["IdUsuario"] = item.IdUsuario;
                        fila["IdAplicacion"] = item.IdAplicacion;
                        fila["FechaCreacion"] = DateTime.Now;
                        tabla.Rows.Add(fila);
                    }
                    break;
                case Relacion.UsuarioManzana:
                    tabla.Columns.Add("IdManzana", typeof(int));
                    tabla.Columns.Add("IdUsuario", typeof(int));
                    tabla.Columns.Add("IdAplicacion", typeof(int));
                    tabla.Columns.Add("FechaCreacion", typeof(DateTime));
                    foreach (var item in request.Block)
                    {
                        DataRow fila = tabla.NewRow();
                        fila["IdManzana"] = item.IdManzana;
                        fila["IdUsuario"] = item.IdUsuario;
                        fila["IdAplicacion"] = item.IdAplicacion;
                        fila["FechaCreacion"] = DateTime.Now;
                        tabla.Rows.Add(fila);
                    }
                    break;
                case Relacion.PermisoPagina:
                    tabla.Columns.Add("IdPagina", typeof(int));
                    tabla.Columns.Add("IdPermiso", typeof(int));
                    tabla.Columns.Add("FechaCreacion", typeof(DateTime));
                    break;

                default:
                    break;
            }

            return tabla;
        }

        public List<PermissionPageCurrentEntity> GetPermissionPageByUserApp(string DBCnn, int IdUsuario, int IdApp)
        {
            SqlDataReader DatosReader;
            using (ContextoDB DataObj = new ContextoDB(DBCnn))
            {
                SqlParameter[] _Parametros = new SqlParameter[]
                {
                        new SqlParameter(ParameterName.IdUsuario,SqlDbType.Int){Value = IdUsuario },
                        new SqlParameter(ParameterName.IdApp,SqlDbType.Int){Value = IdApp },
                };
                DatosReader = DataObj.EjecutaSP(AministrationAppSP.GetPermissionsByUsuarioAndAplication, _Parametros);
                List<PermissionPageCurrentEntity> ListaAplicacion = new List<PermissionPageCurrentEntity>();
                while (DatosReader.Read())
                {
                    PermissionPageCurrentEntity Mod = new PermissionPageCurrentEntity();
                    Mod.Id = Convert.ToInt32(DatosReader[DataReaderColumn.IdPermiso].ToString());
                    Mod.Permiso = DatosReader[DataReaderColumn.Permiso].ToString(); 
                    Mod.IdPagina = Convert.ToInt32(DatosReader[DataReaderColumn.IdPagina].ToString());
                    ListaAplicacion.Add(Mod);
                }
                return ListaAplicacion;
            }
        }

        public List<PermissionCatalogEntity> GetPermissionCatalog(string DBCnn, int? IdPermiso = new int?())
        {
            SqlDataReader DatosReader;
            using (ContextoDB DataObj = new ContextoDB(DBCnn))
            {
                SqlParameter[] _Parametros = new SqlParameter[]
                {
                        new SqlParameter(ParameterName.IdPermiso,SqlDbType.Int){Value = IdPermiso },
                };
                DatosReader = DataObj.EjecutaSP(AministrationAppSP.GetPermissionCatalog, _Parametros);
                List<PermissionCatalogEntity> Lista = new List<PermissionCatalogEntity>();
                while (DatosReader.Read())
                {
                    PermissionCatalogEntity ent = new PermissionCatalogEntity();
                    ent.Id = Convert.ToInt32(DatosReader["Id"].ToString());
                    ent.Nombre = DatosReader[DataReaderColumn.Nombre].ToString();
                    ent.Descripcion = DatosReader[DataReaderColumn.Descripcion].ToString();
                    ent.Activo = Convert.ToBoolean(DatosReader[DataReaderColumn.Activo].ToString());
                    ent.FechaCreacion = Convert.ToDateTime(DatosReader[DataReaderColumn.FechaCreacion].ToString());
                   
                    Lista.Add(ent);
                }
                return Lista;
            }
        }

        public List<PermissionCatalogEntity> GetPermissionCatalogByNotExistInPage(string DBCnn, int IdPagina)
        {
            SqlDataReader DatosReader;
            using (ContextoDB DataObj = new ContextoDB(DBCnn))
            {
                SqlParameter[] _Parametros = new SqlParameter[]
                {
                   new SqlParameter(ParameterName.IdPagina,SqlDbType.Int){Value = IdPagina },
                };
                DatosReader = DataObj.EjecutaSP(AministrationAppSP.GetPermissionCatalogByNotExistInPage, _Parametros);
                List<PermissionCatalogEntity> Lista = new List<PermissionCatalogEntity>();
                while (DatosReader.Read())
                {
                    PermissionCatalogEntity ent = new PermissionCatalogEntity();
                    ent.Id = Convert.ToInt32(DatosReader["Id"].ToString());
                    ent.Nombre = DatosReader[DataReaderColumn.Nombre].ToString();
                    ent.Descripcion = DatosReader[DataReaderColumn.Descripcion].ToString();
                    ent.Activo = Convert.ToBoolean(DatosReader[DataReaderColumn.Activo].ToString());
                    ent.FechaCreacion = Convert.ToDateTime(DatosReader[DataReaderColumn.FechaCreacion].ToString());

                    Lista.Add(ent);
                }
                return Lista;
            }
        }

        public List<PermissionCatalogEntity> GetPermissionCatalogByPage(string DBCnn, int IdPagina)
        {
            SqlDataReader DatosReader;
            using (ContextoDB DataObj = new ContextoDB(DBCnn))
            {
                SqlParameter[] _Parametros = new SqlParameter[]
                {
                   new SqlParameter(ParameterName.IdPagina,SqlDbType.Int){Value = IdPagina },
                };
                DatosReader = DataObj.EjecutaSP(AministrationAppSP.GetPermissionCatalogByPage, _Parametros);
                List<PermissionCatalogEntity> Lista = new List<PermissionCatalogEntity>();
                while (DatosReader.Read())
                {
                    PermissionCatalogEntity ent = new PermissionCatalogEntity();
                    ent.Id = Convert.ToInt32(DatosReader[DataReaderColumn.Id].ToString());
                    ent.Nombre = DatosReader[DataReaderColumn.Nombre].ToString();
                    ent.Descripcion = DatosReader[DataReaderColumn.Descripcion].ToString();
                    ent.Activo = Convert.ToBoolean(DatosReader[DataReaderColumn.Activo].ToString());
                    ent.FechaCreacion = Convert.ToDateTime(DatosReader[DataReaderColumn.FechaCreacion].ToString());
                    ent.IdPermisoPagina = Convert.ToInt32(DatosReader[DataReaderColumn.IdPermisoPagina].ToString());
                    Lista.Add(ent);
                }
                return Lista;
            }
        }

        public void SavePermissionPage(string DBCnn, int IdPermiso, int IdPagina)
        {
            try
            {
                using (ContextoDB DataObj = new ContextoDB(DBCnn))
                {
                    SqlParameter[] _Parametros = new SqlParameter[]{
                        new SqlParameter(ParameterName.IdPermiso,SqlDbType.Int){Value = IdPermiso },
                        new SqlParameter(ParameterName.IdPagina,SqlDbType.Int){Value = IdPagina },
                      };
                    DataObj.EjecutaSP(AministrationAppSP.SavePermissionPage, _Parametros);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void DeletePermissionPagina(string DBCnn, int IdPermisoPagina)
        {
            try
            {
                using (ContextoDB DataObj = new ContextoDB(DBCnn))
                {
                    SqlParameter[] _Parametros = new SqlParameter[]
                    {
                        new SqlParameter(ParameterName.IdPermisoPagina,SqlDbType.Int){Value = IdPermisoPagina },
                    };
                    DataObj.EjecutaSP(AministrationAppSP.DeletePermissionPage, _Parametros);
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

        public List<CatalogEntity> GetBlockAsignedToUser(string DBCnn, int IdUsuario, int IdApp)
        {
            SqlDataReader DatosReader;
            using (ContextoDB DataObj = new ContextoDB(DBCnn))
            {
                SqlParameter[] _Parametros = new SqlParameter[]
                {
                        new SqlParameter(ParameterName.IdUsuario,SqlDbType.Int){Value = IdUsuario },
                        new SqlParameter(ParameterName.IdApp,SqlDbType.Int){Value = IdApp },
                };
                DatosReader = DataObj.EjecutaSP(AministrationAppSP.GetBlockAsignedByApp, _Parametros);
                List<CatalogEntity> ListaAplicacion = new List<CatalogEntity>();
                while (DatosReader.Read())
                {
                    CatalogEntity Mod = new CatalogEntity();
                    Mod.Id = Convert.ToInt32(DatosReader[DataReaderColumn.Id].ToString());
                    Mod.Descripcion = DatosReader[DataReaderColumn.Descripcion].ToString();
                    ListaAplicacion.Add(Mod);
                }
                return ListaAplicacion;
            }
        }

        #endregion

        #region Administrador de Catálogos
        public List<Entities.SystemAdmin.CatalogEntity> GetCatalogManager(string DBCnn, int? IdAplicacion)
        {
            SqlDataReader DatosReader;
            using (ContextoDB DataObj = new ContextoDB(DBCnn))
            {
                SqlParameter[] _Parametros = new SqlParameter[]
                {
                        new SqlParameter(ParameterName.IdApp,SqlDbType.Int){Value = IdAplicacion },
                };
                DatosReader = DataObj.EjecutaSP(AministrationAppSP.GetAdministradorCatalogo, _Parametros);
                List<CatalogEntity> Lista = new List<CatalogEntity>();
                while (DatosReader.Read())
                {
                    CatalogEntity ent = new CatalogEntity();
                    ent.Id = Convert.ToInt32(DatosReader[DataReaderColumn.Id].ToString());
                    ent.Descripcion = DatosReader[DataReaderColumn.Nombre].ToString();
                    Lista.Add(ent);
                }
                return Lista;
            }
        }

        public void LockPermission(string DBCnn, int IdLock, bool Activar)
        {
            try
            {
                using (ContextoDB DataObj = new ContextoDB(DBCnn))
                {
                    SqlParameter[] _Parametros = new SqlParameter[]
                    {
                        new SqlParameter(ParameterName.IdPermiso,SqlDbType.Int){Value = IdLock },
                        new SqlParameter(ParameterName.Activo,SqlDbType.Bit){Value = Activar },
                    };
                    DataObj.EjecutaSP(AministrationAppSP.LockCatalog, _Parametros);
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
                    ent.IdCatalogType = IDCatalog;

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
        public List<Entities.SystemAdmin.CatalogEntity> GetBlockByUserAndAplication(string DBCnn, int IdUser, int IdAplication)
        {
            SqlDataReader DatosReader;
            using (ContextoDB DataObj = new ContextoDB(DBCnn))
            {
                SqlParameter[] _Parametros = new SqlParameter[]
                {
                        new SqlParameter(ParameterName.IdUsuario,SqlDbType.Int){Value = IdUser },
                        new SqlParameter(ParameterName.IdApp,SqlDbType.Int){Value = IdAplication },
                };
                DatosReader = DataObj.EjecutaSP(AministrationAppSP.GetBlockByUserAndAplication, _Parametros);
                List<CatalogEntity> Lista = new List<CatalogEntity>();
                while (DatosReader.Read())
                {
                    CatalogEntity ent = new CatalogEntity();
                    ent.Id = Convert.ToInt32(DatosReader[DataReaderColumn.Id].ToString());
                    ent.Descripcion = DatosReader[DataReaderColumn.Descripcion].ToString();
                    Lista.Add(ent);
                }
                return Lista;
            }
        }
        public void SavePermissionCatalog(string DBCnn, PermissionCatalogEntity entity)
        {
            try
            {
                using (ContextoDB DataObj = new ContextoDB(DBCnn))
                {
                    SqlParameter[] _Parametros = new SqlParameter[]
                    {
                        new SqlParameter("Id",SqlDbType.Int){Value = entity.Id },
                        new SqlParameter("Nombre",SqlDbType.VarChar){Value = entity.Nombre },
                        new SqlParameter("Descripcion",SqlDbType.VarChar){Value = entity.Descripcion },
                        new SqlParameter("Activo",SqlDbType.VarChar){Value = entity.Activo }
                    };
                    DataObj.EjecutaSP(AministrationAppSP.SavePermissionCatalog, _Parametros);
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

        public List<PermisosByPagina> GetPermissionPageAndAsign(string DBCnn, int? IdPerfil, int? IdApp, int? IdPagina = new int?())
        {
            SqlDataReader DatosReader;
            using (ContextoDB DataObj = new ContextoDB(DBCnn))
            {
                SqlParameter[] _Parametros = new SqlParameter[]
                {
                        new SqlParameter("IdPerfil",SqlDbType.Int){Value = IdPerfil },
                        new SqlParameter("IdApp",SqlDbType.Int){Value = IdApp },
                        new SqlParameter("IdPagina",SqlDbType.Int){Value = IdPagina },
                        
                };
                DatosReader = DataObj.EjecutaSP(AministrationAppSP.GetPagesAndPermissionAsign, _Parametros);
                List<PermisosByPagina> list = new List<PermisosByPagina>();
                while (DatosReader.Read())
                {
                    PermisosByPagina dto = new PermisosByPagina();
                    dto.IdPagina = int.Parse(DatosReader["IdPagina"].ToString());
                    dto.Pagina = DatosReader["Pagina"].ToString();
                    dto.Orden = int.Parse(DatosReader["Orden"].ToString());
                    var jsonPermisos = DatosReader["PermissionsList"].ToString();
                    var PermisoPagina =  JsonConvert.DeserializeObject<List<PermisoPaginaEntity>>(jsonPermisos);
                    dto.PermisosPaginas = PermisoPagina;
                    list.Add(dto);
                }
                return list.OrderBy(x=>x.Orden).ToList();
            }
        }

        public void SaveDetailProfileAndPermissionList(string DBCnn, PermisoPaginaPerfilRequest request)
        {
            try
            {
                DataTable tabla = new DataTable();

                tabla.Columns.Add(ColumnDataTable.IdPermisoPagina, typeof(int));
                tabla.Columns.Add(ColumnDataTable.IdPerfil, typeof(int));
                tabla.Columns.Add(ColumnDataTable.IdUsuarioCreacion, typeof(int));

                foreach (var item in request.PermisoPaginaPerfil)
                {
                    DataRow fila = tabla.NewRow();
                    fila[ColumnDataTable.IdPermisoPagina] = item.IdPermisoPagina;
                    fila[ColumnDataTable.IdPerfil] = item.IdPerfil;
                    fila[ColumnDataTable.IdUsuarioCreacion] = request.IdUsuarioModificacion;
                    tabla.Rows.Add(fila);
                }

                using (ContextoDB DataObj = new ContextoDB(DBCnn))
                {
                    SqlParameter[] _Parametros = new SqlParameter[]{
                        new SqlParameter(ParameterName.IdPerfil,SqlDbType.Int){Value = request.IdPerfil },
                        new SqlParameter(ParameterName.Nombre,SqlDbType.VarChar){Value = request.Nombre },
                        new SqlParameter(ParameterName.Activo,SqlDbType.Bit){Value = request.Activo },
                        new SqlParameter(ParameterName.IdUsuarioModificacion,SqlDbType.Int){Value = request.IdUsuarioModificacion },
                        new SqlParameter(ParameterName.TblPermisoPaginaPerfil, SqlDbType.Structured) { Value = tabla, TypeName = TypeTable.PermisoPaginaPerfil},
                };

                    DataObj.EjecutaSP(AministrationAppSP.SavePermisoPaginaPerfilBulk, _Parametros);
                }
            }
            catch (Exception e)
            {

                throw e;
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

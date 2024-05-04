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
    public class LoginRepository : IDisposable
    {
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        SqlParameter[] _Parametros;
        List<UserEntity> Lista = new List<UserEntity>();
        List<ApplicationEntity> ListaApp = new List<ApplicationEntity>();

        public List<UserEntity> GetUserForLogin(string DBCnn, string User, string Password = null)
        {
            SqlDataReader DatosReader;
            using (ContextoDB DataObj = new ContextoDB(DBCnn))
            {
                _Parametros = new SqlParameter[]
                    {
                    new SqlParameter("User",SqlDbType.VarChar){Value = User },
                    new SqlParameter("Password",SqlDbType.VarChar){Value = Password }
                    };
                DatosReader = DataObj.EjecutaSP(LoginSP.GetUserForLogin, _Parametros);
                try
                {
                    while (DatosReader.Read())
                    {
                        UserEntity login = new UserEntity();
                        login.Id = Convert.ToInt32(DatosReader["Id"].ToString());
                        login.Nombre = DatosReader["Nombre"].ToString();
                        login.APaterno = DatosReader["APaterno"].ToString();
                        login.AMaterno = DatosReader["AMaterno"].ToString();
                        login.ImagenUpload = DatosReader["ImagenUpload"].ToString();
                        //login.Email = DatosReader["Email"].ToString();
                        //login.Password = DatosReader["Password"].ToString();
                        login.NoIntentosFallidos = Convert.ToInt32(DatosReader["NoIntentosFallidos"].ToString());
                        login.Activo = Convert.ToBoolean(DatosReader["Activo"].ToString());
                        login.PrimerLogin = Convert.ToBoolean(DatosReader["PrimerLogin"].ToString());
                        login.PerfilActivo = Convert.ToBoolean(DatosReader["PerfilActivo"].ToString());
                        Lista.Add(login);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            return Lista;
        }
        public List<UserEntity> GetUser(string DBCnn, int? user = new int?())
        {
            SqlDataReader DatosReader;
            using (ContextoDB DataObj = new ContextoDB(DBCnn))
            {
                _Parametros = new SqlParameter[]
                    {
                    new SqlParameter("Id",SqlDbType.VarChar){Value = user },
                    };
                DatosReader = DataObj.EjecutaSP(LoginSP.GetUser, _Parametros);
                //ListaApp = new List<ApplicationEntity>();
                while (DatosReader.Read())
                {
                    UserEntity login = new UserEntity();
                    login.Id = int.Parse(DatosReader["Id"].ToString());
                    login.Nombre = DatosReader["Nombre"].ToString();
                    login.APaterno = DatosReader["APaterno"].ToString();
                    login.AMaterno = DatosReader["AMaterno"].ToString();
                    login.Email = DatosReader["Email"].ToString();
                    login.Telefono = DatosReader["Telefono"].ToString();
                    login.ImagenUpload = DatosReader["ImagenUpload"].ToString();
                    login.Perfil = DatosReader["Perfil"].ToString();
                    login.Activo = Convert.ToBoolean(DatosReader["Activo"].ToString());
                    login.IdPerfil = int.Parse(DatosReader["IdPerfil"].ToString());

                    Lista.Add(login);
                }
                return Lista;
            }
        }

        public int UpdateInfoUser(string DBCnn, UserEntity ent)
        {
            SqlDataReader DatosReader;
            using (ContextoDB DataObj = new ContextoDB(DBCnn))
            {
                _Parametros = new SqlParameter[]
                    {
                    new SqlParameter("Email",SqlDbType.VarChar){Value = ent.Email },
                    new SqlParameter("Telefono",SqlDbType.VarChar){Value = ent.Telefono },
                    new SqlParameter("Id",SqlDbType.VarChar){Value = ent.Id },
                    new SqlParameter("ImagenUpload",SqlDbType.VarChar){Value = ent.ImagenUpload },
                    new SqlParameter("Password",SqlDbType.VarChar){Value = ent.Password }
                    };
                DatosReader = DataObj.EjecutaSP(LoginSP.UpdateInfoUser, _Parametros);
                return int.Parse(DatosReader.RecordsAffected.ToString());
            }
        }

        public List<ApplicationEntity> GetApplicationsAllowed(string DBCnn, int? IdUser)
        {
            SqlDataReader DatosReader;
            using (ContextoDB DataObj = new ContextoDB(DBCnn))
            {
                _Parametros = new SqlParameter[]
                    {
                    new SqlParameter("@IdUser",SqlDbType.VarChar){Value = IdUser },
                    };
                DatosReader = DataObj.EjecutaSP(LoginSP.GetApplicationsAllowed, _Parametros);
                ListaApp = new List<ApplicationEntity>();
                while (DatosReader.Read())
                {
                    ApplicationEntity login = new ApplicationEntity();
                    login.Id = Convert.ToInt32(DatosReader["Id"].ToString());
                    login.Descripcion = DatosReader["Descripcion"].ToString();
                    login.Activo = Convert.ToBoolean(DatosReader["Activo"].ToString());
                    login.Icono = DatosReader["Icono"].ToString();
                    login.Accion = DatosReader["Accion"].ToString();
                    login.Controlador = DatosReader["Controlador"].ToString();
                    login.Dominio = DatosReader["Dominio"].ToString();
                    ListaApp.Add(login);
                }
                return ListaApp;
            }
        }

        public void ChangePassword(string DBCnn, int? user, string password)
        {
            try
            {
                SqlDataReader DatosReader;
                using (ContextoDB DataObj = new ContextoDB(DBCnn))
                {
                    _Parametros = new SqlParameter[]
                        {
                    new SqlParameter("Id",SqlDbType.VarChar){Value = user },
                    new SqlParameter("Password",SqlDbType.VarChar){Value = password },
                   // new SqlParameter("PasswordNuevo",SqlDbType.VarChar){Value = passwordnuevo },
                   // new SqlParameter("Email",SqlDbType.VarChar){Value = correo }
                        };
                    DatosReader = DataObj.EjecutaSP(LoginSP.ChangePassword, _Parametros);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<NoticeEntity> getNoticeByIdUser(string DBCnn, int? IdUser)
        {
            SqlDataReader DatosReader;
            using (ContextoDB DataObj = new ContextoDB(DBCnn))
            {
                _Parametros = new SqlParameter[]
                    {
                    new SqlParameter("@IdUser",SqlDbType.VarChar){Value = IdUser },
                    };
                DatosReader = DataObj.EjecutaSP(LoginSP.GetNoticeByIdUser, _Parametros);
                List<NoticeEntity> ListaAviso = new List<NoticeEntity>();
                while (DatosReader.Read())
                {
                    NoticeEntity aviso = new NoticeEntity();

                    aviso.Id = Convert.ToInt32(DatosReader["Id"].ToString());
                    aviso.Titulo = DatosReader["Titulo"].ToString();
                    aviso.Descripcion = DatosReader["Descripcion"].ToString();
                    aviso.FechaCreacion = Convert.ToDateTime(DatosReader["FechaCreacion"].ToString());
                    aviso.FechaEvento = Convert.ToDateTime(DatosReader["FechaEvento"].ToString());
                    aviso.IdAplicacion = Convert.ToInt32(DatosReader["IdAplicacion"].ToString());
                    aviso.IdUsuario = Convert.ToInt32(DatosReader["IdUsuario"].ToString());
                    aviso.iconoAplicacion = DatosReader["iconoAplicacion"].ToString();
                    aviso.nombreUsuario = DatosReader["nombreUsuario"].ToString();
                    aviso.IdTipoAviso = Convert.ToInt32(DatosReader["IdTipoAviso"].ToString());
                    aviso.descripcionTipoAviso = DatosReader["descripcionTipoAviso"].ToString();
                    ListaAviso.Add(aviso);
                }
                return ListaAviso;
            }
        }

        public List<TipoAvisoEntity> getTipoAviso(string DBCnn)
        {
            SqlDataReader DatosReader;
            using (ContextoDB DataObj = new ContextoDB(DBCnn))
            {
                _Parametros = new SqlParameter[]
                    {
                    };
                DatosReader = DataObj.EjecutaSP(LoginSP.GetTipoAviso, _Parametros);
                List<TipoAvisoEntity> ListaAviso = new List<TipoAvisoEntity>();
                while (DatosReader.Read())
                {
                    TipoAvisoEntity aviso = new TipoAvisoEntity();

                    aviso.Id = Convert.ToInt32(DatosReader["Id"].ToString());
                    aviso.Descripcion = DatosReader["Descripcion"].ToString();
                   
                    ListaAviso.Add(aviso);
                }
                return ListaAviso;
            }
        }
        public List<PrioridadEntity> getPrioridad(string DBCnn)
        {
            SqlDataReader DatosReader;
            using (ContextoDB DataObj = new ContextoDB(DBCnn))
            {
                _Parametros = new SqlParameter[]
                    {
                    };
                DatosReader = DataObj.EjecutaSP(LoginSP.GetPrioridad, _Parametros);
                List<PrioridadEntity> ListaPrioridad= new List<PrioridadEntity>();
                while (DatosReader.Read())
                {
                    PrioridadEntity prioridad = new PrioridadEntity();

                    prioridad.Id = Convert.ToInt32(DatosReader["Id"].ToString());
                    prioridad.Descripcion = DatosReader["Descripcion"].ToString();
                    prioridad.ClassColor = DatosReader["ClassColor"].ToString();

                    ListaPrioridad.Add(prioridad);
                }
                return ListaPrioridad;
            }
        }

        public int SaveUpdateAviso(string DBCnn, NoticeEntity ent)
        {
            SqlDataReader DatosReader;
            using (ContextoDB DataObj = new ContextoDB(DBCnn))
            {
                _Parametros = new SqlParameter[]
                    {
                    new SqlParameter("Id",SqlDbType.Int){Value = ent.Id },
                    new SqlParameter("IdAplicacion",SqlDbType.Int){Value = ent.IdAplicacion },
                    new SqlParameter("IdTipoAviso",SqlDbType.Int){Value = ent.IdTipoAviso },
                    new SqlParameter("IdUsuario",SqlDbType.Int){Value = ent.IdUsuario },
                    new SqlParameter("Titulo",SqlDbType.VarChar){Value = ent.Titulo },
                    new SqlParameter("Descripcion",SqlDbType.VarChar){Value = ent.Descripcion },
                    new SqlParameter("FechaEvento",SqlDbType.DateTime){Value = ent.FechaEvento }
                    };
                DatosReader = DataObj.EjecutaSP(LoginSP.SaveNotice, _Parametros);
                return int.Parse(DatosReader.RecordsAffected.ToString());
            }
        }

    }
}

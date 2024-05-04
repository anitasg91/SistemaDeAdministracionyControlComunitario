/*using STC.PCG.Datos;
using STC.PCG.Entidades.AdministracionApp;
using STC.PCG.Entidades.Login;*/
using SAyCC.Bussiness.Common;
using SAyCC.Data.Repository;
using SAyCC.Entities.Common;
using SAyCC.Entities.Login;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAyCC.Bussiness.Login
{
    public class LoginBusiness : IDisposable
    {
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        /*public List<UsuarioEntidad> ObtenerDatosUsuarios(int? Id)
        {
            using (AdministracionAppsRepositorio objAdmin = new AdministracionAppsRepositorio())
            {
                return objAdmin.ObtenerDatosUsuarios(DBSet.DBcnn, Id);
            }
        }
        public List<PerfilEntidad> ObtenerDatosPerfil(int? Id)
        {
            using (AdministracionAppsRepositorio objAdmin = new AdministracionAppsRepositorio())
            {
                return objAdmin.ObtenerDatosPerfil(DBSet.DBcnn, Id);
            }
        }*/
        public List<ApplicationEntity> getApplications(int? Id)
        {
            using (AministrationRepository objAdmin = new AministrationRepository())
            {
                return objAdmin.getApplications(DBSet.DBcnn, Id);
            }
        }

        public List<UserEntity> AllowAccess(string User, string Password = null)
        {
            using (LoginRepository objDBD = new LoginRepository())
            {
                var Result = objDBD.GetUserForLogin(DBSet.DBcnn, User, Password);
                objDBD.Dispose();
                return Result;
            }
        }

        public List<UserEntity> GetUser(int? user = new int?())
        {
            using (LoginRepository objDBD = new LoginRepository())
            {
                var Result = objDBD.GetUser(DBSet.DBcnn, user);
                objDBD.Dispose();
                return Result;
            }
        }

        public int UpdateInfoUser(UserEntity ent)
        {
            using (LoginRepository objDBD = new LoginRepository())
            {
                var Result = objDBD.UpdateInfoUser(DBSet.DBcnn, ent);
                objDBD.Dispose();
                return Result;
            }

        }

        public List<ApplicationEntity> GetApplicationsAllowed(int? IdUser)
        {
            using (LoginRepository objDBD = new LoginRepository())
            {
                var Result = objDBD.GetApplicationsAllowed(DBSet.DBcnn, IdUser);
                objDBD.Dispose();
                return Result;
            }
        }
        public void ChangePassword(int? user, string password)
        {
            using (LoginRepository objDBD = new LoginRepository())
            {
               objDBD.ChangePassword(DBSet.DBcnn, user, password);
                objDBD.Dispose();
            }
        }
        public List<NoticeEntity> getNoticeByIdUser(int? IdUser)
        {
            using (LoginRepository objDBD = new LoginRepository())
            {
                var Result = objDBD.getNoticeByIdUser(DBSet.DBcnn, IdUser);
                objDBD.Dispose();
                return Result;
            }
        }
        public List<TipoAvisoEntity> getTipoAviso()
        {
            using (LoginRepository objDBD = new LoginRepository())
            {
                var Result = objDBD.getTipoAviso(DBSet.DBcnn);
                objDBD.Dispose();
                return Result;
            }
        }
        public List<PrioridadEntity> getPrioridad()
        {
            using (LoginRepository objDBD = new LoginRepository())
            {
                var Result = objDBD.getPrioridad(DBSet.DBcnn);
                objDBD.Dispose();
                return Result;
            }
        }


        public int SaveUpdateAviso(NoticeEntity ent)
        {
            using (LoginRepository objDBD = new LoginRepository())
            {
                var Result = objDBD.SaveUpdateAviso(DBSet.DBcnn, ent);
                objDBD.Dispose();
                return Result;
            }

        }

        /*public List<AplicacionEntidad> ObtenAplicacionUsuario(int? Id)
        {
            using (AdministracionAppsRepositorio objAdmin = new AdministracionAppsRepositorio())
            {
                return objAdmin.ObtenAplicacionUsuario(DBSet.DBcnn, Id);
            }
        }
        public List<TipoAplicacionEntidad> ObtenerTipodeAplicacion(int? Id)
        {
            using (AdministracionAppsRepositorio objAdmin = new AdministracionAppsRepositorio())
            {
                return objAdmin.ObtenerTipodeAplicacion(DBSet.DBcnn, Id);
            }
        }
        public Respuesta NuevoUsuario(UsuarioEntidad model, int? Id, string password)
        {
            using (AdministracionAppsRepositorio objAdmin = new AdministracionAppsRepositorio())
            {
                return objAdmin.NuevoUsuario(DBSet.DBcnn, model, Id, password);
            }
        }
        public List<ConfigGeneralEntidad> ObtieneContrasenaGenerica()
        {
            using (AdministracionAppsRepositorio objAdmin = new AdministracionAppsRepositorio())
            {
                return objAdmin.ObtieneContrasenaGenerica(DBSet.DBcnn);
            }
        }
        public Respuesta AsignaAplicacion(DropListEntidad model)
        {
            using (AdministracionAppsRepositorio objAdmin = new AdministracionAppsRepositorio())
            {
                return objAdmin.AsignaAplicacion(DBSet.DBcnn, model);
            }
        }
        public Respuesta GuardaUsuarioEditado(UsuarioEntidad model, int? Id)
        {
            using (AdministracionAppsRepositorio objAdmin = new AdministracionAppsRepositorio())
            {
                return objAdmin.GuardaUsuarioEditado(DBSet.DBcnn, model, Id);
            }
        }
        public Respuesta GuardaNuevoPerfil(PerfilEntidad model, int? Id)
        {
            using (AdministracionAppsRepositorio objAdmin = new AdministracionAppsRepositorio())
            {
                return objAdmin.GuardaNuevoPerfil(DBSet.DBcnn, model, Id);
            }
        }
        public Respuesta GuardaPerfilEditado(PerfilEntidad model, int? Id)
        {
            using (AdministracionAppsRepositorio objAdmin = new AdministracionAppsRepositorio())
            {
                return objAdmin.GuardaPerfilEditado(DBSet.DBcnn, model, Id);
            }
        }
        public Respuesta GuardaNuevaApp(AplicacionEntidad model, int? Id)
        {
            using (AdministracionAppsRepositorio objAdmin = new AdministracionAppsRepositorio())
            {
                return objAdmin.GuardaNuevaApp(DBSet.DBcnn, model, Id);
            }
        }
        public Respuesta GuardaPerfildeApp(DropListEntidad model)
        {
            using (AdministracionAppsRepositorio objAdmin = new AdministracionAppsRepositorio())
            {
                return objAdmin.GuardaPerfildeApp(DBSet.DBcnn, model);
            }
        }
        public Respuesta GuardaAppEditada(AplicacionEntidad model, int? Id)
        {
            using (AdministracionAppsRepositorio objAdmin = new AdministracionAppsRepositorio())
            {
                return objAdmin.GuardaAppEditada(DBSet.DBcnn, model, Id);
            }
        }
        public List<AplicacionEntidad> ObtieneAplicacionNoAsignadaByUsuario(int? Id)
        {
            using (AdministracionAppsRepositorio objAdmin = new AdministracionAppsRepositorio())
            {
                return objAdmin.ObtieneAplicacionNoAsignadaByUsuario(DBSet.DBcnn, Id);
            }
        }*/
    }
}

using SAyCC.Bussiness.Common;
using SAyCC.Data.Repository;
using SAyCC.Entities.Login;
using SAyCC.Entities.SystemAdmin;
using SAyCC.Entities.WaterSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAyCC.Bussiness.SystemAdmin
{
    public class ApplicationBusiness : IDisposable
    {
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #region Usuarios
        public List<UserEntity> GetUsers(bool getWaterMeter = false)
        {
            using (AministrationRepository objDBD = new AministrationRepository())
            {
                var Result = objDBD.GetUsers(DBSet.DBcnn);
                if (getWaterMeter)
                {
                    Result.ForEach(x => x.Medidor = GetWaterMeterByIdUser(x.Id));
                }

                objDBD.Dispose();
                return Result;
            }
        }
        public UserEntity GetUserDetailById(int IdUser)
        {
            using (AministrationRepository objDBD = new AministrationRepository())
            {
                var Result = objDBD.GetUserDetailById(DBSet.DBcnn, IdUser);
                objDBD.Dispose();
                return Result;
            }
        }
        public List<UserEntity> getUserByParameter(string name)
        {
            using (AministrationRepository objDBD = new AministrationRepository())
            {
                var Result = objDBD.getUserByParameter(DBSet.DBcnn, name);
                objDBD.Dispose();
                return Result;
            }
        }
        public string saveUser(UserEntity entit, ref int Id)
        {
            using (AministrationRepository objDBD = new AministrationRepository())
            {
                string usuario = objDBD.saveUser(DBSet.DBcnn, entit, ref Id);
                objDBD.Dispose();
                return usuario;
            }
        }

        public void ChangeStatusUser(int IdUsuario, int Estatus, string Motivo, int IdUsuarioModificacion)
        {
            using (AministrationRepository objDBD = new AministrationRepository())
            {
                objDBD.ChangeStatusUser(DBSet.DBcnn, IdUsuario, Estatus, Motivo, IdUsuarioModificacion);
                objDBD.Dispose();
            }
        }
        public void ChangeStatusUserBySuperAdmin(int IdUsuario, int Estatus, string Motivo, int IdUsuarioModificacion)
        {
            using (AministrationRepository objDBD = new AministrationRepository())
            {
                objDBD.ChangeStatusUserBySuperAdmin(DBSet.DBcnn, IdUsuario, Estatus, Motivo, IdUsuarioModificacion);
                objDBD.Dispose();
            }
        }

        public bool DeleteUserForErrorToSave(int Id)
        {
            using (AministrationRepository objDBD = new AministrationRepository())
            {
                bool usuario = objDBD.DeleteUserForErrorToSave(DBSet.DBcnn, Id);
                objDBD.Dispose();
                return usuario;
            }
        }

        #endregion

        #region Aplicación
        public void saveApplication(ApplicationEntity entidad)
        {
            using (AministrationRepository objDBD = new AministrationRepository())
            {
                objDBD.saveApplication(DBSet.DBcnn, entidad);
                objDBD.Dispose();
            }
        }
        public void changeStatusApplication(int Id, bool Status, int IdUsuarioModificacion)
        {
            using (AministrationRepository objDBD = new AministrationRepository())
            {
                objDBD.changeStatusApplication(DBSet.DBcnn, Id, Status, IdUsuarioModificacion);
                objDBD.Dispose();
            }
        }
        public void DeleteApplication(int Id)
        {
            using (AministrationRepository objDBD = new AministrationRepository())
            {
                objDBD.DeleteApplication(DBSet.DBcnn, Id);
                objDBD.Dispose();
            }
        }
        #endregion

        #region Modulos
        public List<ModuleEntity> GetModulesAllowed(int? IdUsuario = new int?(), int? IdApp = new int?())
        {
            using (AministrationRepository objDBD = new AministrationRepository())
            {
                var Result = objDBD.GetModulesAllowed(DBSet.DBcnn, IdUsuario, IdApp);
                objDBD.Dispose();
                return Result;
            }
        }

        public void saveModules(int id, int IdApp, string Titulo, string Descripcion, string Icono, string Controlador, string Accion, int Orden, int? IdUsuarioAlta, int? IdUsuarioModificacion, int? IdPadre)
        {
            using (AministrationRepository objDBD = new AministrationRepository())
            {
                objDBD.saveModules(DBSet.DBcnn, id, IdApp, Titulo, Descripcion, Icono, Controlador, Accion, Orden, IdUsuarioAlta, IdUsuarioModificacion, IdPadre);
                objDBD.Dispose();
            }
        }
        
        public void changeStatusModule(int Id, bool Status, int IdUsuarioModificacion)
        {
            using (AministrationRepository objDBD = new AministrationRepository())
            {
                objDBD.changeStatusModule(DBSet.DBcnn, Id, Status, IdUsuarioModificacion);
                objDBD.Dispose();
            }
        }
        
        public void DeleteModule(int Id)
        {
            using (AministrationRepository objDBD = new AministrationRepository())
            {
                objDBD.DeleteModule(DBSet.DBcnn, Id);
                objDBD.Dispose();
            }
        }

        public List<ModuleEntity> GetPagesFathers(int IdApp)
        {
            using (AministrationRepository objDBD = new AministrationRepository())
            {
                var Result = objDBD.GetPagesFathers(DBSet.DBcnn, IdApp);
                objDBD.Dispose();
                return Result;
            }
        }

        #endregion

        #region Perfiles
        public List<ProfileSesionEntity> GetRolesByUserAndAplication(int IdUsuario, int IdApp)
        {
            using (AministrationRepository objDBD = new AministrationRepository())
            {
                var Result = objDBD.GetRolesByUserAndAplication(DBSet.DBcnn, IdUsuario, IdApp);
                objDBD.Dispose();
                return Result;
            }
        }
        
        public List<ProfileEntity> GetProfiles(int? IdPerfil = new int?(), int? IdApp = new int?())/*Borrar*/
        {
            using (AministrationRepository objDBD = new AministrationRepository())
            {
                var Result = objDBD.GetProfiles(DBSet.DBcnn, IdPerfil, IdApp);
                objDBD.Dispose();
                return Result;
            }
        }
        
        public void saveProfiles(int id, int IdApp, string Descripcion, int? IdUsuarioAlta, int? IdUsuarioModificacion)
        {
            using (AministrationRepository objDBD = new AministrationRepository())
            {
                objDBD.saveProfiles(DBSet.DBcnn, id, IdApp, Descripcion, IdUsuarioAlta, IdUsuarioModificacion);
                objDBD.Dispose();
            }
        }
        
        public void changeStatusProfile(int Id, bool Status, int IdUsuarioModificacion)
        {
            using (AministrationRepository objDBD = new AministrationRepository())
            {
                objDBD.changeStatusProfile(DBSet.DBcnn, Id, Status, IdUsuarioModificacion);
                objDBD.Dispose();
            }
        }
        
        public void DeleteProfile(int Id)
        {
            using (AministrationRepository objDBD = new AministrationRepository())
            {
                objDBD.DeleteProfile(DBSet.DBcnn, Id);
                objDBD.Dispose();
            }
        }
        
        public List<ProfileEntity> GetProfilesByAppActives(int IdApp)
        {
            using (AministrationRepository objDBD = new AministrationRepository())
            {
                var Result = objDBD.GetProfilesByAppActives(DBSet.DBcnn, IdApp);
                objDBD.Dispose();
                return Result;
            }
        }
        #endregion

        #region Permisos
        public List<PermissionEntity> getPermission(int? IdPerfil = new int?(), int? IdApp = new int?())
        {
            using (AministrationRepository objDBD = new AministrationRepository())
            {
                var Result = objDBD.getPermission(DBSet.DBcnn, IdPerfil, IdApp);
                objDBD.Dispose();
                return Result;
            }
        }
        public void savePermission(List<PermissionEntity> entidad)
        {
            using (AministrationRepository objDBD = new AministrationRepository())
            {
                objDBD.savePermission(DBSet.DBcnn, entidad);
                objDBD.Dispose();
            }
        }

        public void SaveConfigPermissionsByUser(PermissionsConfigRequest request)
        {
            using (AministrationRepository objDBD = new AministrationRepository())
            {
                objDBD.SaveConfigPermissionsByUser(DBSet.DBcnn, request);
                objDBD.Dispose();
            }
        }

        public void SaveDetailProfileAndPermissionList(PermisoPaginaPerfilRequest request)
        {
            using (AministrationRepository objDBD = new AministrationRepository())
            {
                objDBD.SaveDetailProfileAndPermissionList(DBSet.DBcnn, request);
                objDBD.Dispose();
            }
        }

        public List<PermissionPageCurrentEntity> GetPermissionPageByUserApp(int IdUsuario, int IdApp)
        {
            using (AministrationRepository objDBD = new AministrationRepository())
            {
                var Result = objDBD.GetPermissionPageByUserApp(DBSet.DBcnn, IdUsuario, IdApp);
                objDBD.Dispose();
                return Result;
            }
        }

        public List<PermissionCatalogEntity> GetPermissionCatalog(int? IdPermiso = new int?())
        {
            using (AministrationRepository objDBD = new AministrationRepository())
            {
                var Result = objDBD.GetPermissionCatalog(DBSet.DBcnn, IdPermiso);
                objDBD.Dispose();
                return Result;
            }
        }

        public List<PermissionCatalogEntity> GetPermissionCatalogByNotExistInPage(int IdPagina)
        {
            using (AministrationRepository objDBD = new AministrationRepository())
            {
                var Result = objDBD.GetPermissionCatalogByNotExistInPage(DBSet.DBcnn, IdPagina);
                objDBD.Dispose();
                return Result;
            }
        }

        public List<PermissionCatalogEntity> GetPermissionCatalogByPage(int IdPagina)
        {
            using (AministrationRepository objDBD = new AministrationRepository())
            {
                var Result = objDBD.GetPermissionCatalogByPage(DBSet.DBcnn, IdPagina);
                objDBD.Dispose();
                return Result;
            }
        }

        public void SavePermissionPage(int IdPermiso, int IdPagina)
        {
            using (AministrationRepository objDBD = new AministrationRepository())
            {
                objDBD.SavePermissionPage(DBSet.DBcnn, IdPermiso, IdPagina);
                objDBD.Dispose();
            }
        }

        public void DeletePermissionPagina(int IdPermisoPagina)
        {
            using (AministrationRepository objDBD = new AministrationRepository())
            {
                objDBD.DeletePermissionPagina(DBSet.DBcnn, IdPermisoPagina);
                objDBD.Dispose();
            }
        }

        public List<CatalogEntity> GetBlockAsignedToUser(int IdUsuario, int IdApp)
        {
            using (AministrationRepository objDBD = new AministrationRepository())
            {
                var Result = objDBD.GetBlockAsignedToUser(DBSet.DBcnn, IdUsuario, IdApp);
                objDBD.Dispose();
                return Result;
            }
        }
        #endregion
        #region Administrador de Catálogos
        public List<CatalogEntity> GetCatalogManager(int? IdAplicacion = new int?())
        {
            using (AministrationRepository objDBD = new AministrationRepository())
            {
                var Result = objDBD.GetCatalogManager(DBSet.DBcnn, IdAplicacion);
                objDBD.Dispose();
                return Result;
            }
        }

        public void LockPermission(int IdLock, bool Activar)
        {
            using (AministrationRepository objDBD = new AministrationRepository())
            {
                objDBD.LockPermission(DBSet.DBcnn, IdLock, Activar);
                objDBD.Dispose();
            }
        }
        #endregion

        #region Catalogos
        public List<CatalogEntity> GetCatalog(int IDCatalog, int? Id = new int?())
        {
            using (AministrationRepository objDBD = new AministrationRepository())
            {
                var Result = objDBD.GetCatalog(DBSet.DBcnn, IDCatalog, Id);
                objDBD.Dispose();
                return Result;
            }
        }
        public void DeleteCatalog(int IDCatalog, int Id)
        {
            using (AministrationRepository objDBD = new AministrationRepository())
            {
                objDBD.DeleteCatalog(DBSet.DBcnn, IDCatalog, Id);
                objDBD.Dispose();
            }
        }
        public void SaveCatalog(int IDCatalog, int Id, string Descripcion)
        {
            using (AministrationRepository objDBD = new AministrationRepository())
            {
                objDBD.SaveCatalog(DBSet.DBcnn, IDCatalog, Id, Descripcion);
                objDBD.Dispose();
            }
        }
        public List<CatalogEntity> GetBlockByUserAndAplication(int IdUser, int IdAplication)
        {
            using (AministrationRepository objDBD = new AministrationRepository())
            {
                var Result = objDBD.GetBlockByUserAndAplication(DBSet.DBcnn, IdUser, IdAplication);
                objDBD.Dispose();
                return Result;
            }
        }

        public void SavePermissionCatalog(PermissionCatalogEntity entity)
        {
            using (AministrationRepository objDBD = new AministrationRepository())
            {
                objDBD.SavePermissionCatalog(DBSet.DBcnn, entity);
                objDBD.Dispose();
            }
        }
        #endregion

        #region Asignación
        public List<AsignacionAplicacionEntity> getAsignacionAplicacion(int IdPerfil)
        {
            using (AministrationRepository objDBD = new AministrationRepository())
            {
                var Result = objDBD.getAsignacionAplicacion(DBSet.DBcnn, IdPerfil);
                objDBD.Dispose();
                return Result;
            }
        }
        public void saveAsignacionAplicacion(int Id, int IdProfile, int IdApp)
        {
            using (AministrationRepository objDBD = new AministrationRepository())
            {
                objDBD.saveAsignacionAplicacion(DBSet.DBcnn, Id, IdProfile, IdApp);
                objDBD.Dispose();
            }
        }

        public List<PermisosByPagina> GetPermissionPageAndAsign(int IdPerfil, int? IdApp = new int?(), int? IdPagina = new int?())
        {
            using (AministrationRepository objDBD = new AministrationRepository())
            {
                var Result = objDBD.GetPermissionPageAndAsign(DBSet.DBcnn, IdPerfil, IdApp, IdPagina);
                objDBD.Dispose();
                return Result;
            }
        }
        #endregion

        public void SaveWaterMeter(WaterMeterEntity entity)
        {
            using (AministrationRepository objDBD = new AministrationRepository())
            {
                objDBD.SaveWaterMeter(DBSet.DBcnn, entity);
                objDBD.Dispose();
            }
        }
        public void DeleteWaterMeterById(int IdWaterMeter)
        {
            using (AministrationRepository objDBD = new AministrationRepository())
            {
                objDBD.DeleteWaterMeterById(DBSet.DBcnn, IdWaterMeter);
                objDBD.Dispose();
            }
        }
        public void UpdateWaterMeterById(int IdWaterMeter, int ModifyTypeId, int? IdTitular)
        {
            using (AministrationRepository objDBD = new AministrationRepository())
            {
                objDBD.UpdateWaterMeterById(DBSet.DBcnn, IdWaterMeter, ModifyTypeId, IdTitular);
                objDBD.Dispose();
            }
        }
        public List<WaterMeterEntity> GetWaterMeterByIdUser(int IdUser)
        {
            using (AministrationRepository objDBD = new AministrationRepository())
            {
                var Result = objDBD.GetWaterMeterByIdUser(DBSet.DBcnn, IdUser);
                objDBD.Dispose();
                return Result;
            }
        }

        public List<UserUtilityEntity> GetUsersListForPermissions()
        {
            using (AministrationRepository objDBD = new AministrationRepository())
            {
                var Result = objDBD.GetUsersListForPermissions(DBSet.DBcnn);

                objDBD.Dispose();
                return Result;
            }
        }
        
    }
}

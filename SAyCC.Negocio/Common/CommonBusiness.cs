using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SAyCC.Entities.Login;
//using SAyCC.Entities.Common.ViewModels;
//using SAyCC.Data.Repository;
//using SAyCC.Entities.Common;
using SAyCC.Bussiness.Common;
using SAyCC.Data.Repository;
using SAyCC.Entities.Common;

namespace SAyCC.Bussiness.Common
{
    public class CommonBusiness : IDisposable
    {
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        //#region Sesion Usuario

        public List<ConfiguracionGlobalEntity> getConfiguracionGlobal(string Nombre)
        {
            using (CommonRepository ObjComun = new CommonRepository())
            {
               return ObjComun.getConfiguracionGlobal(DBSet.DBcnn, Nombre);
            }
        }

        //#endregion Sesion Usuario


        //#region Obtener Ubicaciones
        //public List<Ubicacion> ObtenerLineasMetro()
        //{
        //    using (CommonRepositorio ObjComun = new CommonRepositorio())
        //    {
        //        return ObjComun.ObtieneUbicacionesTipo(DBSet.DBcnn, 1).OrderBy(X => X.Id).ToList();
        //    }
        //}

        //public List<Ubicacion> ObtenerUbicacionesPorTipo(int IdTipo)
        //{
        //    using (CommonRepositorio ObjComun = new CommonRepositorio())
        //    {
        //        return ObjComun.ObtieneUbicacionesTipo(DBSet.DBcnn, IdTipo).OrderBy(X => X.Id).ToList();
        //    }
        //}

        //public List<Ubicacion> ObtenerUbicacionesTodas()
        //{
        //    using (CommonRepositorio ObjComun = new CommonRepositorio())
        //    {
        //        return ObjComun.ObtieneUbicacionesTipo(DBSet.DBcnn, null).OrderBy(X => X.IdTipoUbicacion).ThenBy(Y => Y.Id).ToList();
        //    }
        //}
        //#endregion Obtener Ubicaciones


        //#region Obtener Construcciones
        //public List<Construccion> ObtenerEstacionesPorLinea(int IdLinea)
        //{
        //    using (CommonRepositorio ObjComun = new CommonRepositorio())
        //    {
        //        return ObjComun.ObtieneConstruccionesUbicacion(DBSet.DBcnn, IdLinea).OrderBy(X => X.Id).ToList();
        //    }
        //}

        //public List<Construccion> ObtenerEstacionesTodasLineas()
        //{
        //    using (CommonRepositorio ObjComun = new CommonRepositorio())
        //    {
        //        return ObjComun.ObtieneConstruccionesUbicacion(DBSet.DBcnn, null).OrderBy(X => X.IdUbicacion).ThenBy(Y => Y.IdTipoConstruccion).ToList();
        //    }
        //}

        //public List<Construccion> ObtenerConstruccionesPorTipo(int IdTipo)
        //{
        //    using (CommonRepositorio ObjComun = new CommonRepositorio())
        //    {
        //        return ObjComun.ObtieneConstruccionesTipo(DBSet.DBcnn, IdTipo).OrderBy(X => X.IdUbicacion).ToList();
        //    }
        //}

        //public List<Construccion> ObtenerConstruccionesTodas()
        //{
        //    using (CommonRepositorio ObjComun = new CommonRepositorio())
        //    {
        //        return ObjComun.ObtieneConstruccionesTipo(DBSet.DBcnn, null).OrderBy(X => X.IdTipoConstruccion).ThenBy(Y => Y.IdUbicacion).ToList();
        //    }
        //}
        //#endregion Obtener Construcciones


        //#region Obtener Sistemas
        //public List<Sistema> ObtenerSistemas()
        //{
        //    using (CommonRepositorio ObjComun = new CommonRepositorio())
        //    {
        //        return ObjComun.ObtieneSistemas(DBSet.DBcnn, null).OrderBy(X => X.IdSistema).ToList();
        //    }
        //}

        //public List<Sistema> ObtenerSistemas(int IdSistema)
        //{
        //    using (CommonRepositorio ObjComun = new CommonRepositorio())
        //    {
        //        return ObjComun.ObtieneSistemas(DBSet.DBcnn, IdSistema).OrderBy(X => X.IdSistema).ToList();
        //    }
        //}
        //#endregion Obtener Sistemas


        //#region Informacion Trenes
        //public List<DetalleTrenViewModel> ObtenerDetalleTren(int IdTren)
        //{
        //    using (CommonRepositorio ObjComun = new CommonRepositorio())
        //    {
        //        return ObjComun.ObtieneDetalleTren(DBSet.DBcnn, IdTren).OrderBy(X => X.IdTren).ToList();
        //    }
        //}

        //public List<FormacionTrenViewModel> ObtenerFormacionTren(int IdTren)
        //{
        //    using (CommonRepositorio ObjComun = new CommonRepositorio())
        //    {
        //        return ObjComun.ObtieneFormacionTren(DBSet.DBcnn, IdTren).OrderBy(X => X.IdTren).ToList();
        //    }
        //}
        //#endregion Informacion Trenes


        //#region Modulos
        //public List<ModuloEntidad> obtieneModulosPermisosByAplicacion(int IdAplicacion, int IdUsuario)
        //{
        //    using (CommonRepositorio ObjComun = new CommonRepositorio())
        //    {
        //        return ObjComun.obtieneModulosPermisosByAplicacion(DBSet.DBcnn, IdAplicacion, IdUsuario);
        //    }
        //}
        //#endregion
    }
}

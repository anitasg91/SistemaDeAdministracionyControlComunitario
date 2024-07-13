namespace SAyCC.WaterSystem.Utilities
{
   
    public static class PartialViewEnum
    {
        public static string Shared_ImgProfile => "_ImgProfile";
        public static string Shared_SideBar => "_SideBar";
        public static string PageNotAccess => "~/Views/Shared/PageNotAccess.cshtml";
        public static string Shared_Manzana => "_Manzana";
        public static string Shared_Mes => "_Mes";
        public static string Table => "_Table";
        public static string ModalActions => "_ModalActions";

    }
    public static class TempDataEnum
    {
        public static string Resultado => "resultado";
        public static string MensajeErr => "MensajeErr";

    }

    public enum roles
    {
        Superadmin = 1,
        Presidente_Agua = 7,
        Juez = 8
    }
    public enum RestrictedPages
    {
        Catalogos = 2,
        Aplicaciones = 3
    }

    public enum CatalogManager
    {
      Manzana = 1,
      Mes,
      Permisos
    }
    public enum PermisoEn
    {
        TodosLP = 1,
        Crear,
        Editar,
        Borrar,
        Eliminar_Medidor,
        Generar_Reporte,
        Bloquear,
        Activar,
        Desactivar,
        Asignar_Medidor,
        Visualizar,
        Ver_Pestaña_Medidor,
        Administrar_Manzana,
        Administrar_Mes,
        Administrar_Permisos,
        Asignar_Rol,
        Asignar_Manzana,
        AsignarPermisos,
        Ver_Pestaña_Paginas,
        Crear_Pagina,
        Editar_Pagina,
        Activar_Pagina,
        Desactivar_Pagina,
        Eliminar_Pagina,
        Ver_Pestaña_Permisos,
        Asignar_Permiso,
        Eliminar_Permiso,
        Ver_Pestaña_Perfiles,
        Crear_Perfil,
        Editar_Perfil,
        Activar_Perfil,
        Desactivar_Perfil,
        Eliminar_Perfil,
        Crear_Manzana,
        Editar_Manzana,
        Eliminar_Manzana,
        Crear_Mes,
        Editar_Mes,
        Eliminar_Mes,
        Crear_Permiso,
        Editar_Permiso,
        blbla,//Este valor puede cambiar
        Activar_Permiso,
        Desactivar_Permiso,
        AprobarMovimientos,
        Lectura_Desfase_Periodo,
        Desasignar_Medidor,
        Dar_Baja,
        Descargar
    }
    public static class ConfigModalDelete
    {
        public static string Titulo => "Eliminar {0}";
        public static string Manzana => "manzana";
        public static string Mes => "mes";
        public static string Permisos => "permiso";
        public static string Descripcion => "Si elimina{0}, éste y todos sus componentes se verán afectados. ¿Esta seguro de eliminar{1}?";
    }

    public static class Constantes
    {
        public const string CatalogNotAsigned = "No tiene catálogos asignados, contacte al administrador.";
        public const string UpdateRecord = "El registro ha sido actualizado con éxito.";
        public const string LockUser = "Si desactiva el usuario, ya no podrá entrar al sistema, ni generar historial.";
        public const string UnLockUser = "Si activa el usuario, podrá entrar al sistema y/o generar historial.";
        public const string LockProfile = "Si desactiva el perfil, los usuarios asignados perderán los privilegios asociados al perfil.";
        public const string UnLockProfile = "Si activa el perfil, los usuarios que lo tengan asignado retomarán sus privilegios.";
        public const string SendToUpdateRecord = "Se ha solicitado la {0} del usuario correctamente. En espera de las aprobaciones correspondientes.";
        public const string ApproveLockUser = "Esta aprobando la solicitud de bloqueo de usuario, ya no podrá entrar al sistema.";
        public const string ApproveUnlockUser = "Esta aprobando la solicitud de desbloqueo de usuario, podrá entrar y manipular el sistema.";
        public const string ApproveDeleteUser = "Esta aprobando la solicitud de eliminación del usuario, ya no podrá entrar al sistema y se irá a lista negra para posibles reingresos.";
        public const string ApproveMoveUser = "Si no eres administrador. cancela esta operación, tu usuario quedará registrado con este movimiento.";
        public const string MensajeCambioEstatus = "El usuario se ha {0} por solicitud del super admin {1}";
    }

    public enum EstatusUsuario
    {
        Eliminacion=0,//No esta en la BD
        Activo = 1,
        Inactivo,
        Eliminado,
        EnvíoEliminación,
        ProcesoAprobaciónEliminación,
        EnvíoInactivación,
        ProcesoAprobaciónInactivación,
        EnvíoActivación,
        ProcesoAprobaciónActivación,
        EnvíoRecuperación
        //,Recuperacion
    }
    
}
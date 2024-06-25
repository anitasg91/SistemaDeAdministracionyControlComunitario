namespace SAyCC.Entities
{
    public enum Relacion
    {
        UsuarioPerfil,
        UsuarioManzana,
        PermisoPagina
    }
    public enum EstatusUsuario
    {
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
    }
}
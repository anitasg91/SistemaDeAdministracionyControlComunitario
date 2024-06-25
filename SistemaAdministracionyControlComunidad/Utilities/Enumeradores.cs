namespace SAyCC.Login.Utilities
{
    public enum Prioridad
    {
        Bajo = 1,
        Medio = 3,
        Urgente = 4
    }
    public static class Constantes
    {
        public static string RutaLogin => "https://localhost:44336/";

    }

    public enum Relacion
    {
        UsuarioPerfil,
        UsuarioManzana,
        PermisoPagina
    }

}
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAyCC.Entities.Common
{
    public class Enumerador
    {
        public enum Perfil
        {
            Administrador = 1,
            Delegado = 2,
            Subdelegado = 3,
            Tesorero = 4,
            Usuario = 5
        }
        public enum TypeAction
        {
            InActive = 1,
            Delete = 2,
        }
        public enum CatalogType
        {
            Apple = 1,
            Month = 2,
            Permission = 3,
                Profile
        }
        /// <summary>
        ///   Disassociate = Desasocia el medidor del usuario, para poder reasignarlo a alguien más. 
        ///   Unsubscribe = Da de baja permanentemente el medidor, ya no se podrá usar, pero continuará mostrando su historial.
        ///   Deactivate = Desactiva temporalmente el medidor.
        ///   Delete = Elimina permanentemente el medidor, solo si no tiene registros ligados a él.
        ///   update = Indica su actualización.
        ///   Associate = Aasocia el medidor a un usuario.
        /// </summary>
        public enum ModifyTypeWaterMeter
        {
            Disassociate = 1,
            Unsubscribe = 2,
            Deactivate = 3,
            Delete = 4,
            Update = 5,
            associate = 6,
            activate = 7,
            Subscribe = 8
        }

        public enum CatalogTypeWaterSystem
        {
            TipoLectura = 1,
            Folio = 2
        }

        public enum Aplicacion
        {
            AdministracionDelSistema = 1,
             Delegacion = 2,
             Iglesia = 3,
             AguaPotable = 4,
             ActividadesDeportivasYCulturales = 5,
             Avisos = 6
        }

    }
}

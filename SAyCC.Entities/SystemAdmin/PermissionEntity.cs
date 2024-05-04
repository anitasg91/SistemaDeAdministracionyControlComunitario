using System;
using System.Collections.Generic;
using System.Text;

namespace SAyCC.Entities.SystemAdmin
{
    public class PermissionEntity
    {
        public int Id { get; set; }
        public int IdPerfil { get; set; }
        public int IdModulo { get; set; }
        public bool Alta { get; set; }
        public bool Modificacion { get; set; }
        public bool Consulta { get; set; }
        public string Modulo { get; set; }
        public string Perfil { get; set; }
        public string Aplicacion { get; set; }
        public string Descripcion { get; set; }
        
        //public string strAlta { get { return Alta ? "fas fa-check-circle" : "fas fa-ban"; } }
        //public string strModificacion { get { return Modificacion ? "fas fa-check-circle" : "fas fa-ban"; } }
        //public string strConsulta { get { return Consulta ? "fas fa-check-circle" : "fas fa-ban"; } }

        //public string ColorA { get { return Alta ? "green" : "red"; } }
        //public string ColorM { get { return Modificacion ? "green" : "red"; } }
        //public string ColorC { get { return Consulta ? "green" : "red"; } }

        public string chkAlta { get { return Alta ? "checked" : ""; } }
        public string chkModificacion { get { return Modificacion ? "checked" : ""; } }
        public string chkConsulta { get { return Consulta ? "checked" : ""; } }

        public string strAlta { get; set; }
        public string strModificacion { get; set; }
        public string strConsulta { get; set; }

    }
}

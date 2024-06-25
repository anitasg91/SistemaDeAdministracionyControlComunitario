using System;
using System.Collections.Generic;
using System.Text;

namespace SAyCC.Entities.SystemAdmin
{
   public class CatalogEntity
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public string Nombre { get; set; } //Auxiliar
        public int IdCatalogType { get; set; } = 0; //Auxiliar
    }

    public class CatalogResponse
    {
        public List<CatalogEntity> Catalog { get; set; } = new List<CatalogEntity>();
        public List<PermissionCatalogEntity> PermissionCatalog { get; set; } = new List<PermissionCatalogEntity>();
    }
    public class PersonalizeModalDelete
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
    }


}

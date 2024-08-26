using System;
using System.Collections.Generic;

namespace DL
{
    public partial class ArticuloAlmacen
    {
        public ArticuloAlmacen()
        {
            Registros = new HashSet<Registro>();
        }

        public int IdArticuloAlmacen { get; set; }
        public int? IdArticulo { get; set; }
        public int? IdAlmacen { get; set; }
        public int? Stock { get; set; }

        public virtual Almacen? IdAlmacenNavigation { get; set; }
        public virtual Articulo? IdArticuloNavigation { get; set; }
        public virtual ICollection<Registro> Registros { get; set; }
    }
}

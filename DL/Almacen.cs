using System;
using System.Collections.Generic;

namespace DL
{
    public partial class Almacen
    {
        public Almacen()
        {
            ArticuloAlmacens = new HashSet<ArticuloAlmacen>();
        }

        public int IdAlmacen { get; set; }
        public int? IdUbicacion { get; set; }
        public string? Descripcion { get; set; }
        public string? Direccion { get; set; }

        public virtual Ubicacion? IdUbicacionNavigation { get; set; }
        public virtual ICollection<ArticuloAlmacen> ArticuloAlmacens { get; set; }
    }
}

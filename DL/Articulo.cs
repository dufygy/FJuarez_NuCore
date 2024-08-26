using System;
using System.Collections.Generic;

namespace DL
{
    public partial class Articulo
    {
        public Articulo()
        {
            ArticuloAlmacens = new HashSet<ArticuloAlmacen>();
        }

        public int IdArticulo { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public byte[]? Imagen { get; set; }
        public int? IdMarca { get; set; }

        public virtual Marca? IdMarcaNavigation { get; set; }
        public virtual ICollection<ArticuloAlmacen> ArticuloAlmacens { get; set; }
    }
}

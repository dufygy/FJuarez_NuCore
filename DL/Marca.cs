using System;
using System.Collections.Generic;

namespace DL
{
    public partial class Marca
    {
        public Marca()
        {
            Articulos = new HashSet<Articulo>();
        }

        public int IdMarca { get; set; }
        public string Nombre { get; set; } = null!;

        public virtual ICollection<Articulo> Articulos { get; set; }
    }
}

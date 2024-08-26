using System;
using System.Collections.Generic;

namespace DL
{
    public partial class Ubicacion
    {
        public Ubicacion()
        {
            Almacens = new HashSet<Almacen>();
        }

        public int IdUbicacion { get; set; }
        public string? Descripcion { get; set; }

        public virtual ICollection<Almacen> Almacens { get; set; }
    }
}

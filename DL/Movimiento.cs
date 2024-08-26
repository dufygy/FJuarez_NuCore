using System;
using System.Collections.Generic;

namespace DL
{
    public partial class Movimiento
    {
        public Movimiento()
        {
            Registros = new HashSet<Registro>();
        }

        public int IdMovimiento { get; set; }
        public string? Descripcion { get; set; }

        public virtual ICollection<Registro> Registros { get; set; }
    }
}

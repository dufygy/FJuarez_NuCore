using System;
using System.Collections.Generic;

namespace DL
{
    public partial class Registro
    {
        public int IdRegistro { get; set; }
        public int? IdMovimiento { get; set; }
        public int? IdArticuloAlmacen { get; set; }
        public int? Cantidad { get; set; }

        public virtual ArticuloAlmacen? IdArticuloAlmacenNavigation { get; set; }
        public virtual Movimiento? IdMovimientoNavigation { get; set; }
    }
}

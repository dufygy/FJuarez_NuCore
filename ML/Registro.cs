using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Registro
    {
        public int IdRegistro { get; set; }
        public ML.ArticuloAlmacen ArticuloAlmacen { get; set; }
        public ML.Movimiento Movimiento { get; set; }
        public int Cantidad { get; set; }
    }
}

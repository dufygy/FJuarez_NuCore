using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class ArticuloAlmacen
    {
        public int IdArticuloAlmacen { get; set; }
        public ML.Articulo Articulo { get; set; }
        public ML.Almacen Almacen { get; set; }
        public int? Stock { get; set; }
        public  List<object> ArticulosAlmacenes { get; set; }
    }
}

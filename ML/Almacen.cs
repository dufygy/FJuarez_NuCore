using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Almacen
    {
        public int IdAlmacen { get; set; }
        public string Descripcion { get; set; }
        public string Direccion { get; set; }
        public List<object> Almacenes { get; set; } = new List<object>();
    }
}

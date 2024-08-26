using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Ubicacion
    {
        public int IdUbicacion { get; set; }
        public string Nombre { get; set; }
        public ML.Almacen Almacen { get; set; }
    }
}

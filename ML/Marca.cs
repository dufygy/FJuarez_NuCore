using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public partial class Marca
    {
        public int IdMarca { get; set; }
        public string Nombre { get; set; }
        public List<Marca> Marcas { get; set; }  
    }
}

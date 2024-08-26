using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Articulo
    {
        public int IdArticulo { get; set; }
        public string Nombre { get; set; }
        public byte[] Imagen { get; set; }
        public string Descripcion { get; set; }
        public ML.Marca Marca { get; set; }
        public List<object> Articulos { get; set; }
        public List<object> Marcas { get; set; }

    }
}

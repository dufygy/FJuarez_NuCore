namespace ML
{
    public class Inventario
    {
        public int IdInventario { get; set; }
        public ML.Ubicacion Ubicacion { get; set; }
        public ML.Articulo Articulo { get; set; }
        public ML.Almacen Almacen { get; set; }
        public int Stock { get; set; }

    }
}
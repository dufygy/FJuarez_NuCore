namespace BL
{
    public class Articulo
    {
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.FJuarezInventarioContext context = new DL.FJuarezInventarioContext())
                {
                    var articulos = (from u in context.Articulos
                                     select u).ToList();

                    if (articulos != null && articulos.Count > 0)
                    {
                        result.Objects = new List<object>();
                        foreach (var articulo in articulos)
                        {
                            var linqMarca = context.Marcas.FirstOrDefault(m => m.IdMarca == articulo.IdMarca);
                            ML.Articulo tempArticulo = new ML.Articulo
                            {
                                IdArticulo = articulo.IdArticulo,
                                Nombre = articulo.Nombre,
                                Imagen = articulo.Imagen,
                                Descripcion = articulo.Descripcion,
                                Marca = new ML.Marca
                                {
                                    IdMarca = Convert.ToInt32(articulo.IdMarca),
                                    Nombre = "Marca por defecto"
                                }
                            };
                            if (tempArticulo.Marca.IdMarca > 0)
                            {
                                tempArticulo.Marca.Nombre = linqMarca.Nombre;
                            }
                            result.Objects.Add(tempArticulo);
                        }
                        List<ML.Marca> marcas = context.Marcas
                                            .Select(m => new ML.Marca
                                            {
                                                IdMarca = m.IdMarca,
                                                Nombre = m.Nombre
                                            }).ToList();

                        result.Object = new ML.Marca
                        {
                            Marcas = marcas
                        };



                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.Message = "No se encontraron registros de artículos.";
                    }
                }




            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Message = "Ha ocurrido un error al cargar los registros de artículos. ERROR: " + ex.Message;
            }

            return result;
        }

        public static ML.Result GetById(int idArticulo)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.FJuarezInventarioContext context = new DL.FJuarezInventarioContext())
                {
                    var linqArticulo = context.Articulos.FirstOrDefault(a => a.IdArticulo == idArticulo);

                    if (linqArticulo != null)
                    {
                        // Obtener la lista completa de marcas para el dropdown
                        List<ML.Marca> marcas = context.Marcas
                                           .Select(m => new ML.Marca
                                           {
                                               IdMarca = m.IdMarca,
                                               Nombre = m.Nombre
                                           }).ToList();

                        // Obtener el nombre de la marca correspondiente al artículo
                        var linqMarca = context.Marcas.FirstOrDefault(m => m.IdMarca == linqArticulo.IdMarca);

                        // Verificar si linqMarca es null
                        string nombreMarca = linqMarca != null ? linqMarca.Nombre : "No tiene asignada una marca";

                        ML.Articulo articulo = new ML.Articulo
                        {
                            IdArticulo = linqArticulo.IdArticulo,
                            Nombre = linqArticulo.Nombre,
                            Imagen = linqArticulo.Imagen,
                            Descripcion = linqArticulo.Descripcion,

                            Marca = new ML.Marca
                            {
                                IdMarca = Convert.ToInt32(linqArticulo.IdMarca),
                                Nombre = nombreMarca,  // Asignar el nombre de la marca o el texto alternativo
                                Marcas = marcas
                            }
                        };

                        result.Object = articulo;
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.Message = "Artículo no encontrado.";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Message = ex.Message;
            }

            return result;
        }

        public static ML.Result Add(ML.Articulo articulo)
        {
            ML.Result result = new ML.Result();

            try
            {
                if (articulo != null)
                {
                    using (DL.FJuarezInventarioContext context = new DL.FJuarezInventarioContext())
                    {
                        var linqArticulo = new DL.Articulo
                        {
                            Nombre = articulo.Nombre,
                            Imagen = articulo.Imagen,
                            Descripcion = articulo.Descripcion,
                            IdMarca = articulo.Marca.IdMarca
                        };

                        context.Articulos.Add(linqArticulo);
                        int rowsAffected = context.SaveChanges();

                        result.Correct = rowsAffected > 0;
                        if (!result.Correct)
                        {
                            result.Message = "No se pudo insertar el artículo.";
                        }
                    }
                }
                else
                {
                    result.Correct = false;
                    result.Message = "El artículo proporcionado es nulo.";
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Message = ex.Message;
            }

            return result;
        }

        public static ML.Result Update(ML.Articulo articulo)
        {
            ML.Result result = new ML.Result();

            try
            {
                if (articulo != null)
                {
                    using (DL.FJuarezInventarioContext context = new DL.FJuarezInventarioContext())
                    {
                        var linqArticulo = context.Articulos.FirstOrDefault(a => a.IdArticulo == articulo.IdArticulo);


                        if (linqArticulo != null)
                        {
                            linqArticulo.Nombre = articulo.Nombre;
                            linqArticulo.Imagen = articulo.Imagen;
                            linqArticulo.Descripcion = articulo.Descripcion;
                            linqArticulo.IdMarca = articulo.Marca.IdMarca;

                            int rowsAffected = context.SaveChanges();

                            result.Correct = rowsAffected > 0;
                            if (!result.Correct)
                            {
                                result.Message = "El registro no surtio efectos porque es igual al de la base.";
                            }
                        }
                        else
                        {
                            result.Correct = false;
                            result.Message = "Artículo no encontrado.";
                        }
                    }
                }
                else
                {
                    result.Correct = false;
                    result.Message = "El artículo proporcionado es nulo.";
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Message = ex.Message;
            }

            return result;
        }

        public static ML.Result Delete(int idArticulo)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.FJuarezInventarioContext context = new DL.FJuarezInventarioContext())
                {
                    var linqArticulo = context.Articulos.FirstOrDefault(a => a.IdArticulo == idArticulo);

                    if (linqArticulo != null)
                    {
                        context.Articulos.Remove(linqArticulo);
                        int rowsAffected = context.SaveChanges();

                        result.Correct = rowsAffected > 0;
                        if (!result.Correct)
                        {
                            result.Message = "No se pudo eliminar el artículo.";
                        }
                    }
                    else
                    {
                        result.Correct = false;
                        result.Message = "Artículo no encontrado.";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Message = ex.Message;
            }

            return result;
        }
    }
}

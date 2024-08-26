using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Inventario
    {
        public static ML.Result GetAllArticuloAlmacen()
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.FJuarezInventarioContext context = new DL.FJuarezInventarioContext())
                {
                    var articulosAlmacen = (from aa in context.ArticuloAlmacens
                                            join a in context.Articulos on aa.IdArticulo equals a.IdArticulo
                                            join al in context.Almacens on aa.IdAlmacen equals al.IdAlmacen
                                            join m in context.Marcas on a.IdMarca equals m.IdMarca
                                            select new ML.ArticuloAlmacen
                                            {
                                                IdArticuloAlmacen = aa.IdArticuloAlmacen,
                                                Articulo = new ML.Articulo
                                                {
                                                    IdArticulo = a.IdArticulo,
                                                    Nombre = a.Nombre,
                                                    Descripcion = a.Descripcion,
                                                    Imagen = a.Imagen,
                                                    Marca = new ML.Marca
                                                    {
                                                        IdMarca = m.IdMarca,
                                                        Nombre = m.Nombre
                                                    }
                                                },
                                                Almacen = new ML.Almacen
                                                {
                                                    IdAlmacen = al.IdAlmacen,
                                                    Descripcion = al.Descripcion,
                                                    Direccion = al.Direccion
                                                },
                                                Stock = aa.Stock
                                            }).ToList();

                    if (articulosAlmacen.Count > 0)
                    {
                        result.Objects = articulosAlmacen.Cast<object>().ToList();
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.Message = "No se encontraron registros en ArticuloAlmacen.";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Message = "Ocurrió un error al obtener los registros de ArticuloAlmacen: " + ex.Message;
            }

            return result;
        }

        //DDL
        public static ML.Result AlmacenGetAll()
        {
            ML.Result result = new ML.Result
            {
                Objects = new List<object>() 
            };
            try
            {
                using (DL.FJuarezInventarioContext con = new DL.FJuarezInventarioContext())
                {
                    var almacenes = (from u in con.Almacens
                                     select u).ToList();
                    foreach (var tempAlmacen in almacenes)
                    {
                        ML.Almacen alm = new ML.Almacen
                        {
                            IdAlmacen = tempAlmacen.IdAlmacen,
                            Descripcion = tempAlmacen.Descripcion,
                            Direccion = tempAlmacen.Direccion
                        };
                        result.Objects.Add(alm);
                    }
                    result.Correct = true;
                }
            }
            catch (Exception ex) 
            {
                result.Correct = false;
                result.Message = ex.Message;
            }
            
            
            return result;
        }

        public static ML.Result GetArticulosByAlmacen(int idAlmacen)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.FJuarezInventarioContext context = new DL.FJuarezInventarioContext())
                {
                    var articulosAlmacen = (from aa in context.ArticuloAlmacens
                                            join a in context.Articulos on aa.IdArticulo equals a.IdArticulo
                                            join al in context.Almacens on aa.IdAlmacen equals al.IdAlmacen
                                            where aa.IdAlmacen == idAlmacen
                                            select new ML.ArticuloAlmacen
                                            {
                                                IdArticuloAlmacen = aa.IdArticuloAlmacen,
                                                Articulo = new ML.Articulo
                                                {
                                                    IdArticulo = a.IdArticulo,
                                                    Nombre = a.Nombre,
                                                    Descripcion = a.Descripcion
                                                },
                                                Almacen = new ML.Almacen
                                                {
                                                    IdAlmacen = al.IdAlmacen,
                                                    Descripcion = al.Descripcion,
                                                    Direccion = al.Direccion
                                                },
                                                Stock = aa.Stock
                                            }).ToList();

                    result.Objects = articulosAlmacen.Cast<object>().ToList();
                    result.Correct = true;
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Message = ex.Message;
            }

            return result;
        }

        public static ML.Result AddArticuloAlmacen(ML.ArticuloAlmacen articuloAlmacen)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.FJuarezInventarioContext context = new DL.FJuarezInventarioContext())
                {
                    // Verificar que IdArticulo y IdAlmacen no sean nulos o inválidos
                    if (articuloAlmacen.Articulo?.IdArticulo == null || articuloAlmacen.Almacen?.IdAlmacen == null)
                    {
                        result.Correct = false;
                        result.Message = "El ID del Artículo o el Almacén es nulo o inválido.";
                        return result;
                    }
                    DL.ArticuloAlmacen nuevoArticuloAlmacen = new DL.ArticuloAlmacen
                    {
                        IdArticulo = articuloAlmacen.Articulo.IdArticulo,
                        IdAlmacen = articuloAlmacen.Almacen.IdAlmacen,
                        Stock = articuloAlmacen.Stock
                    };

                    context.ArticuloAlmacens.Add(nuevoArticuloAlmacen);
                    int rowsAffected = context.SaveChanges();

                    if (rowsAffected > 0)
                    {
                        result.Object = nuevoArticuloAlmacen.IdArticuloAlmacen;
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.Message = "No se pudo agregar el artículo al almacén.";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Message = "Ocurrió un error al agregar el artículo al almacén: " + ex.Message;
            }

            return result;
        }

        public static ML.Result DeleteArticuloAlmacen(int idArticuloAlmacen)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.FJuarezInventarioContext context = new DL.FJuarezInventarioContext())
                {
                    // Primero eliminar los registros relacionados en la tabla Registro
                    var registrosRelacionados = context.Registros.Where(r => r.IdArticuloAlmacen == idArticuloAlmacen).ToList();
                    context.Registros.RemoveRange(registrosRelacionados);

                    // Guardar los cambios después de eliminar los registros
                    context.SaveChanges();

                    // Ahora eliminar el registro de ArticuloAlmacen
                    var articuloAlmacen = context.ArticuloAlmacens.FirstOrDefault(aa => aa.IdArticuloAlmacen == idArticuloAlmacen);
                    if (articuloAlmacen != null)
                    {
                        context.ArticuloAlmacens.Remove(articuloAlmacen);
                        int rowsAffected = context.SaveChanges();

                        if (rowsAffected > 0)
                        {
                            result.Correct = true;
                        }
                        else
                        {
                            result.Correct = false;
                            result.Message = "No se pudo eliminar el registro de ArticuloAlmacen.";
                        }
                    }
                    else
                    {
                        result.Correct = false;
                        result.Message = "El registro de ArticuloAlmacen no fue encontrado.";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Message = "Ocurrió un error al eliminar el registro de ArticuloAlmacen: " + ex.Message;
            }

            return result;
        }


        public static ML.Result UpdateArticuloAlmacen(ML.ArticuloAlmacen articuloAlmacen)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.FJuarezInventarioContext context = new DL.FJuarezInventarioContext())
                {
                    var articuloAlmacenExistente = context.ArticuloAlmacens.FirstOrDefault(aa => aa.IdArticuloAlmacen == articuloAlmacen.IdArticuloAlmacen);

                    if (articuloAlmacenExistente != null)
                    {
                        articuloAlmacenExistente.IdArticulo = articuloAlmacen.Articulo.IdArticulo;
                        articuloAlmacenExistente.IdAlmacen = articuloAlmacen.Almacen.IdAlmacen;
                        articuloAlmacenExistente.Stock = articuloAlmacen.Stock;

                        int rowsAffected = context.SaveChanges();

                        if (rowsAffected > 0)
                        {
                            result.Correct = true;
                        }
                        else
                        {
                            result.Correct = false;
                            result.Message = "No se pudo actualizar el registro de ArticuloAlmacen.";
                        }
                    }
                    else
                    {
                        result.Correct = false;
                        result.Message = "El registro de ArticuloAlmacen no fue encontrado.";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Message = "Ocurrió un error al actualizar el registro de ArticuloAlmacen: " + ex.Message;
            }

            return result;
        }

        public static ML.Result GetByIdArticuloAlmacen(int idArticuloAlmacen)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.FJuarezInventarioContext context = new DL.FJuarezInventarioContext())
                {
                    var articuloAlmacen = (from aa in context.ArticuloAlmacens
                                           join a in context.Articulos on aa.IdArticulo equals a.IdArticulo
                                           join al in context.Almacens on aa.IdAlmacen equals al.IdAlmacen
                                           where aa.IdArticuloAlmacen == idArticuloAlmacen
                                           select new ML.ArticuloAlmacen
                                           {
                                               IdArticuloAlmacen = aa.IdArticuloAlmacen,
                                               Articulo = new ML.Articulo
                                               {
                                                   IdArticulo = a.IdArticulo,
                                                   Nombre = a.Nombre,
                                                   Descripcion = a.Descripcion,
                                                   Imagen = a.Imagen
                                               },
                                               Almacen = new ML.Almacen
                                               {
                                                   IdAlmacen = al.IdAlmacen,
                                                   Descripcion = al.Descripcion
                                               },
                                               Stock = aa.Stock
                                           }).FirstOrDefault();

                    if (articuloAlmacen != null)
                    {
                        result.Object = articuloAlmacen;
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.Message = "No se encontró el registro de ArticuloAlmacen.";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Message = "Ocurrió un error al obtener el registro de ArticuloAlmacen: " + ex.Message;
            }

            return result;
        }


    }
}

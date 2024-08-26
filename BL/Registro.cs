using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Registro
    {
        public static ML.Result RegistrarMovimiento(int idMovimiento, ML.ArticuloAlmacen articuloAlmacen, int cantidad)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.FJuarezInventarioContext context = new DL.FJuarezInventarioContext())
                {
                    // Verificar si el ArticuloAlmacen existe
                    var articuloAlmacenExistente = context.ArticuloAlmacens
                        .FirstOrDefault(aa => aa.IdArticuloAlmacen == articuloAlmacen.IdArticuloAlmacen);

                    if (articuloAlmacenExistente == null)
                    {
                        result.Correct = false;
                        result.Message = "El ArticuloAlmacen especificado no existe.";
                        return result;
                    }

                    // Si existe, registrar el movimiento
                    DL.Registro nuevoRegistro = new DL.Registro
                    {
                        IdMovimiento = idMovimiento,
                        IdArticuloAlmacen = articuloAlmacen.IdArticuloAlmacen,
                        Cantidad = cantidad
                    };

                    context.Registros.Add(nuevoRegistro);
                    int rowsAffected = context.SaveChanges();

                    if (rowsAffected > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.Message = "No se pudo agregar el registro histórico.";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Message = "Ocurrió un error al agregar el registro histórico: " + ex.Message;
            }

            return result;
        }

        public static ML.Result RegistrarMovimientoBasico(ML.ArticuloAlmacen articuloAlmacen, string tipoMovimiento)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.FJuarezInventarioContext context = new DL.FJuarezInventarioContext())
                {
                    int idMovimiento = 0;

                    // Determinar el tipo de movimiento basado en la operación
                    switch (tipoMovimiento.ToLower())
                    {
                        case "traslado": // Update
                            idMovimiento = 4; // 4 representa "Transferencia"
                            break;
                        case "nuevoingreso": // Add
                            idMovimiento = 3; // 3 representa "nuevo ingreso de un producto"
                                              
                            break;
                        case "salida": // Delete
                            idMovimiento = 2; // 2 representa "Salida"
                            break;
                        default:
                            result.Correct = false;
                            result.Message = "Tipo de movimiento no reconocido.";
                            return result;
                    }

                    
                    DL.Registro nuevoRegistro = new DL.Registro
                    {
                        IdMovimiento = idMovimiento,
                        IdArticuloAlmacen = articuloAlmacen.IdArticuloAlmacen,
                        Cantidad = articuloAlmacen.Stock
                    };

                    
                    context.Registros.Add(nuevoRegistro);
                    int rowsAffected = context.SaveChanges();

                    if (rowsAffected > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.Message = "No se pudo agregar el registro histórico.";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Message = "Ocurrió un error al agregar el registro histórico: " + ex.Message;
            }

            return result;
        }


        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.FJuarezInventarioContext context = new DL.FJuarezInventarioContext())
                {
                    var registros = (from r in context.Registros
                                     join aa in context.ArticuloAlmacens on r.IdArticuloAlmacen equals aa.IdArticuloAlmacen
                                     join a in context.Articulos on aa.IdArticulo equals a.IdArticulo
                                     join al in context.Almacens on aa.IdAlmacen equals al.IdAlmacen
                                     join m in context.Movimientos on r.IdMovimiento equals m.IdMovimiento
                                     select new ML.Registro
                                     {
                                         IdRegistro = r.IdRegistro,
                                         ArticuloAlmacen = new ML.ArticuloAlmacen
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
                                         },
                                         Movimiento = new ML.Movimiento
                                         {
                                             IdMovimiento = m.IdMovimiento,
                                             Descripcion = m.Descripcion
                                         },
                                         Cantidad = Convert.ToInt32(r.Cantidad)
                                     }).ToList();

                    result.Objects = registros.Cast<object>().ToList();
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

        public static ML.Result GetByIdArticuloAlmacen(int idArticuloAlmacen)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.FJuarezInventarioContext context = new DL.FJuarezInventarioContext())
                {
                    var registros = (from r in context.Registros
                                     join aa in context.ArticuloAlmacens on r.IdArticuloAlmacen equals aa.IdArticuloAlmacen
                                     join a in context.Articulos on aa.IdArticulo equals a.IdArticulo
                                     join al in context.Almacens on aa.IdAlmacen equals al.IdAlmacen
                                     join m in context.Movimientos on r.IdMovimiento equals m.IdMovimiento
                                     where aa.IdArticuloAlmacen == idArticuloAlmacen
                                     select new ML.Registro
                                     {
                                         IdRegistro = r.IdRegistro,
                                         ArticuloAlmacen = new ML.ArticuloAlmacen
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
                                         },
                                         Movimiento = new ML.Movimiento
                                         {
                                             IdMovimiento = m.IdMovimiento,
                                             Descripcion = m.Descripcion
                                         },
                                         Cantidad = Convert.ToInt32(r.Cantidad)
                                     }).ToList();

                    result.Objects = registros.Cast<object>().ToList();
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
    }
}

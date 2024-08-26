using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace PL.Controllers
{
    public class InventarioController : Controller
    {
        public IActionResult GetAll()
        {
            // Crear un nuevo objeto de tipo ML.ArticuloAlmacen
            ML.ArticuloAlmacen articuloAlmacen = new ML.ArticuloAlmacen();
            ML.Result resultAll = BL.Inventario.GetAllArticuloAlmacen();
            articuloAlmacen.Articulo = new ML.Articulo();
            articuloAlmacen.Articulo.Articulos = resultAll.Objects;

            // Obtener la lista de todos los almacenes
            ML.Result resultAlmacen = BL.Inventario.AlmacenGetAll();

            if (resultAlmacen.Correct)
            {
                // Inicializar la propiedad Almacen en articuloAlmacen y asignar la lista de almacenes
                articuloAlmacen.Almacen = new ML.Almacen
                {
                    Almacenes = resultAlmacen.Objects
                };
                
            }
            else
            {
                ViewBag.Message = "Ocurrió un error al obtener los almacenes: " + resultAlmacen.Message;
                return View("Error");
            }

            // Retornar el objeto ML.ArticuloAlmacen a la vista
            return View(articuloAlmacen);
        }

        [HttpPost]
        public IActionResult GetAll(ML.ArticuloAlmacen articuloAlmacen)
        {
            // Aquí el articuloAlmacen.Almacen.IdAlmacen debería tener el valor seleccionado
            if (articuloAlmacen.Almacen != null && articuloAlmacen.Almacen.IdAlmacen > 0)
            {
                // Obtener los artículos por almacén
                ML.Result resultArticulos = BL.Inventario.GetArticulosByAlmacen(articuloAlmacen.Almacen.IdAlmacen);

                if (resultArticulos.Correct)
                {
                    articuloAlmacen.Articulo = new ML.Articulo
                    {
                        Articulos = resultArticulos.Objects.Cast<object>().ToList()
                    };
                }
                else
                {
                    ViewBag.Message = "Ocurrió un error al obtener los artículos: " + resultArticulos.Message;
                }
            }

            // Re-cargar la lista de almacenes para el DropDownList
            ML.Result resultAlmacen = BL.Inventario.AlmacenGetAll();
            if (resultAlmacen.Correct)
            {
                articuloAlmacen.Almacen.Almacenes = resultAlmacen.Objects;
            }

            return View(articuloAlmacen);
        }

        [HttpGet]
        public IActionResult Form(int? idArticuloAlmacen)
        {
            ML.ArticuloAlmacen articuloAlmacen = new ML.ArticuloAlmacen();
            ML.Result resultArticulo = BL.Articulo.GetAll();
            ML.Result resultAlmacen = BL.Inventario.AlmacenGetAll();

            articuloAlmacen.Articulo = new ML.Articulo();
            articuloAlmacen.Almacen = new ML.Almacen();

            if (resultArticulo.Correct)
            {
                articuloAlmacen.Articulo.Articulos = resultArticulo.Objects;
            }

            if (resultAlmacen.Correct)
            {
                articuloAlmacen.Almacen.Almacenes = resultAlmacen.Objects;
            }


            if (idArticuloAlmacen != null && idArticuloAlmacen > 0)//update
            {
                ML.Result resultArticuloAlmacen = BL.Inventario.GetByIdArticuloAlmacen(idArticuloAlmacen.Value);

                if (resultArticuloAlmacen.Correct)
                {
                    articuloAlmacen = (ML.ArticuloAlmacen)resultArticuloAlmacen.Object;
                    articuloAlmacen.Articulo.Articulos = resultArticulo.Objects;
                    articuloAlmacen.Almacen.Almacenes = resultAlmacen.Objects;
                }
                else
                {
                    ViewBag.Message = "Ocurrió un error al obtener la información del artículo en el almacén: " + resultArticuloAlmacen.Message;
                    return PartialView("Modal");
                }
               
            }//add

            return View(articuloAlmacen);
        }

        [HttpPost]
        public IActionResult Form(ML.ArticuloAlmacen articuloAlmacen, string tipoMovimiento)
        {
            ML.Result result;

            if (articuloAlmacen.IdArticuloAlmacen > 0)
            {
                BL.Registro.RegistrarMovimiento(4, articuloAlmacen, articuloAlmacen.Stock ?? 0);

                result = BL.Inventario.UpdateArticuloAlmacen(articuloAlmacen);
                if (result.Correct)
                {
                    if (tipoMovimiento == "Transferencia")
                    {
                        BL.Registro.RegistrarMovimientoBasico(articuloAlmacen, "traslado");
                        ViewBag.Message = "El artículo fue transferido correctamente.";
                    }
                    else
                    {
                        ViewBag.Message = "El registro se actualizó correctamente.";
                    }
                }
                else
                {
                    ViewBag.Message = "Ocurrió un error al actualizar el registro: " + result.Message;
                }
            }
            else
            {
                result = BL.Inventario.AddArticuloAlmacen(articuloAlmacen);


                if (result.Correct)
                {
                    articuloAlmacen.IdArticuloAlmacen = (int)result.Object;
                    BL.Registro.RegistrarMovimientoBasico(articuloAlmacen, "NuevoIngreso");
                    ViewBag.Message = "El artículo se agregó correctamente.";
                }
                else
                {
                    ViewBag.Message = "Ocurrió un error al agregar el artículo: " + result.Message;
                }
            }

            return PartialView("Modal");
        }

        
        public IActionResult DescargarKardex(int idArticuloAlmacen)
        {
            ML.Result result = BL.Registro.GetByIdArticuloAlmacen(idArticuloAlmacen);

            if (result.Correct)
            {
                StringBuilder sb = new StringBuilder();

                foreach (ML.Registro registro in result.Objects)
                {
                    sb.AppendLine($"IdRegistro: {registro.IdRegistro}, IdArticuloAlmacen: {registro.ArticuloAlmacen.IdArticuloAlmacen}, " +
                                  $"Movimiento: {registro.Movimiento.Descripcion}, Cantidad: {registro.Cantidad}");
                }

                byte[] fileBytes = Encoding.UTF8.GetBytes(sb.ToString());
                return File(fileBytes, "text/plain", "Registros-"+ DateTime.Now.ToShortDateString()+".txt");
            }
            else
            {
                ViewBag.Message = "Ocurrió un error al obtener los registros para la descarga.";
                return View("Error");
            }
        }


        [HttpGet]
        public IActionResult Delete(int idArticuloAlmacen)
        {
            ML.Result result = BL.Inventario.GetByIdArticuloAlmacen(idArticuloAlmacen);

            if (result.Correct)
            {
                ML.ArticuloAlmacen articuloAlmacen = (ML.ArticuloAlmacen)result.Object;
                BL.Registro.RegistrarMovimientoBasico(articuloAlmacen, "salida");

                ML.Result deleteResult = BL.Inventario.DeleteArticuloAlmacen(idArticuloAlmacen);

                if (deleteResult.Correct)
                {
                    ViewBag.Message = "El artículo fue eliminado correctamente del almacén.";
                }
                else
                {
                    ViewBag.Message = "Ocurrió un error al eliminar el artículo: " + deleteResult.Message;
                }
            }
            else
            {
                ViewBag.Message = "Ocurrió un error al obtener la información del artículo: " + result.Message;
            }

            return RedirectToAction("GetAll", "Inventario");
        }



        [HttpGet]
        public IActionResult SumarStock(int idArticuloAlmacen)
        {
            ML.Result result = BL.Inventario.GetByIdArticuloAlmacen(idArticuloAlmacen);

            if (result.Correct)
            {
                ML.ArticuloAlmacen articuloAlmacen = (ML.ArticuloAlmacen)result.Object;

                // Guardar la información del producto en la sesión
                HttpContext.Session.SetString("ProductoNombre", articuloAlmacen.Articulo.Nombre);
                HttpContext.Session.SetString("ProductoDescripcion", articuloAlmacen.Articulo.Descripcion);
                HttpContext.Session.SetInt32("ProductoStock", articuloAlmacen.Stock ?? 0);

                // Guardar la imagen en la sesión como base64
                if (articuloAlmacen.Articulo.Imagen != null)
                {
                    string imagenBase64 = Convert.ToBase64String(articuloAlmacen.Articulo.Imagen);
                    HttpContext.Session.SetString("ProductoImagen", imagenBase64);
                }
                else
                {
                    HttpContext.Session.SetString("ProductoImagen", ""); // Usar cadena vacía en lugar de null
                }

                return View(articuloAlmacen);
            }
            else
            {
                ViewBag.Message = "Ocurrió un error al obtener la información del artículo: " + result.Message;
                return RedirectToAction("GetAll", "Inventario");
            }
        }


        [HttpPost]
        public IActionResult SumarStock(int CantidadSumar, ML.ArticuloAlmacen articuloAlmacen)
        {
            ML.Result result = BL.Inventario.GetByIdArticuloAlmacen(articuloAlmacen.IdArticuloAlmacen);

            if (result.Correct)
            {
                ML.ArticuloAlmacen articuloAlmacenExistente = (ML.ArticuloAlmacen)result.Object;

                // Sumar el nuevo stock al stock existente
                articuloAlmacenExistente.Stock += CantidadSumar;

                // Actualizar el registro en la base de datos
                ML.Result updateResult = BL.Inventario.UpdateArticuloAlmacen(articuloAlmacenExistente);

                if (updateResult.Correct)
                {
                    BL.Registro.RegistrarMovimiento(1, articuloAlmacenExistente, CantidadSumar);

                    ViewBag.Message = "El stock se sumó correctamente.";
                }
                else
                {
                    ViewBag.Message = "Ocurrió un error al sumar el stock: " + updateResult.Message;
                }
            }
            else
            {
                ViewBag.Message = "Ocurrió un error al obtener la información del artículo: " + result.Message;
            }

            return RedirectToAction("GetAll", "Inventario");
        }


        [HttpGet]
        public IActionResult RestarStock(int idArticuloAlmacen)
        {
            ML.Result result = BL.Inventario.GetByIdArticuloAlmacen(idArticuloAlmacen);

            if (result.Correct)
            {
                ML.ArticuloAlmacen articuloAlmacen = (ML.ArticuloAlmacen)result.Object;

                // Guardar la información del producto en la sesión
                HttpContext.Session.SetString("ProductoNombre", articuloAlmacen.Articulo.Nombre);
                HttpContext.Session.SetString("ProductoDescripcion", articuloAlmacen.Articulo.Descripcion);
                HttpContext.Session.SetInt32("ProductoStock", articuloAlmacen.Stock ?? 0);

                // Guardar la imagen en la sesión como base64
                if (articuloAlmacen.Articulo.Imagen != null)
                {
                    string imagenBase64 = Convert.ToBase64String(articuloAlmacen.Articulo.Imagen);
                    HttpContext.Session.SetString("ProductoImagen", imagenBase64);
                }
                else
                {
                    HttpContext.Session.SetString("ProductoImagen", ""); // Usar cadena vacía en lugar de null
                }

                return View(articuloAlmacen);
            }
            else
            {
                ViewBag.Message = "Ocurrió un error al obtener la información del artículo: " + result.Message;
                return RedirectToAction("GetAll", "Inventario");
            }
        }


        [HttpPost]
        public IActionResult RestarStock(int CantidadRestar, ML.ArticuloAlmacen articuloAlmacen)
        {
            ML.Result result = BL.Inventario.GetByIdArticuloAlmacen(articuloAlmacen.IdArticuloAlmacen);

            if (result.Correct)
            {
                ML.ArticuloAlmacen articuloAlmacenExistente = (ML.ArticuloAlmacen)result.Object;

                // Validar que la cantidad a restar no sea mayor que el stock actual
                if (CantidadRestar > articuloAlmacenExistente.Stock)
                {
                    ViewBag.Message = "La cantidad a restar excede el stock disponible.";
                    return View(articuloAlmacenExistente);
                }

                // Restar el nuevo stock al stock existente
                articuloAlmacenExistente.Stock -= CantidadRestar;

                // Actualizar el registro en la base de datos
                ML.Result updateResult = BL.Inventario.UpdateArticuloAlmacen(articuloAlmacenExistente);

                if (updateResult.Correct)
                {
                    BL.Registro.RegistrarMovimiento(2, articuloAlmacenExistente, CantidadRestar);

                    ViewBag.Message = "El stock se restó correctamente.";
                }
                else
                {
                    ViewBag.Message = "Ocurrió un error al restar el stock: " + updateResult.Message;
                }
            }
            else
            {
                ViewBag.Message = "Ocurrió un error al obtener la información del artículo: " + result.Message;
            }

            return RedirectToAction("GetAll", "Inventario");
        }




    }
}

using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class ArticuloController : Controller
    {
        public IActionResult GetAll()
        {
            ML.Result result = BL.Articulo.GetAll();
            ML.Articulo articulo = new ML.Articulo
            {
                Articulos = result.Objects


            };

            return View(articulo);
        }

        [HttpGet]
        public IActionResult Form(int? idArticulo)
        {
            ML.Articulo articulo = new ML.Articulo();
            ML.Result marcaResult = BL.Marca.GetAll();

            // Inicializar la propiedad Marca para evitar NullReferenceException
            articulo.Marca = new ML.Marca
            {
                Marcas = marcaResult.Objects.Cast<ML.Marca>().ToList()
            };

            if (idArticulo != null && idArticulo != 0)
            {
                ML.Result resultArticulo = BL.Articulo.GetById(idArticulo.Value);
                if (resultArticulo.Correct)
                {
                    articulo = (ML.Articulo)resultArticulo.Object;
                    // Asegúrate de que la lista de marcas se mantenga al editar un artículo
                    articulo.Marca.Marcas = marcaResult.Objects.Cast<ML.Marca>().ToList();
                }
                else
                {
                    ViewBag.Mensaje = "Ocurrió un error al traer la información del artículo: " + resultArticulo.Message;
                    return PartialView("Modal");
                }
            }
            return View(articulo);
        }



        [HttpPost]
        public ActionResult Form(ML.Articulo articulo, IFormFile imagen)
        {
            //if (ModelState.IsValid)
            //{

            if (imagen != null && imagen.Length > 0)
            {
                articulo.Imagen = ConvertirABytes(imagen);
            }

            ML.Result result;

            if (articulo.IdArticulo > 0)
            {
                result = BL.Articulo.Update(articulo);
                ViewBag.Message = result.Correct ? "El artículo se actualizó correctamente." : "No se pudo actualizar el artículo: " + result.Message;
            }
            else
            {
                result = BL.Articulo.Add(articulo);
                ViewBag.Message = result.Correct ? "El artículo se insertó correctamente." : "No se pudo insertar el artículo: " + result.Message;
            }

            return PartialView("Modal");
            //}
            //else
            //{
            //return View(articulo);
            //}
        }

        [HttpGet]
        public ActionResult Delete(int idArticulo)
        {
            if (idArticulo > 0)
            {
                var result = BL.Articulo.Delete(idArticulo);
                ViewBag.Mensaje = result.Correct ? "El artículo se eliminó correctamente." : "No se pudo eliminar el artículo: " + result.Message;
            }
            else
            {
                ViewBag.Mensaje = "No se pudo eliminar el artículo.";
            }

            return PartialView("Modal");
        }

        private byte[] ConvertirABytes(IFormFile foto)
        {
            using (var ms = new MemoryStream())
            {
                foto.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}

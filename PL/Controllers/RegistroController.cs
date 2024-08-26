using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text;

namespace PL.Controllers
{
    public class RegistroController : Controller
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            ML.Result result = BL.Registro.GetAll();

            if (result.Correct)
            {
                // Convertir el resultado en una lista de ML.Registro antes de pasarlo a la vista
                var registros = result.Objects.Cast<ML.Registro>().ToList();
                return View(registros);
            }
            else
            {
                ViewBag.Message = "Ocurrió un error al obtener el historial de registros: " + result.Message;
                return View(new List<ML.Registro>());
            }
        }

        [HttpGet]
        public IActionResult DescargarRegistros()
        {
            ML.Result result = BL.Registro.GetAll();

            if (result.Correct)
            {
                StringBuilder sb = new StringBuilder();

                foreach (ML.Registro registro in result.Objects)
                {
                    sb.AppendLine($"IdRegistro: {registro.IdRegistro}, IdArticuloAlmacen: {registro.ArticuloAlmacen.IdArticuloAlmacen}, " +
                                  $"Movimiento: {registro.Movimiento.Descripcion}, Cantidad: {registro.Cantidad}");
                }

                byte[] fileBytes = Encoding.UTF8.GetBytes(sb.ToString());
                return File(fileBytes, "text/plain", "Registros.txt");
            }
            else
            {
                ViewBag.Message = "Ocurrió un error al obtener los registros para la descarga.";
                return View("Error");
            }
        }


    }
}

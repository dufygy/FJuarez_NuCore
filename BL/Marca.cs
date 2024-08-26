using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Marca
    {
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            result.Objects = new List<object>();

            try
            {
                using (DL.FJuarezInventarioContext context = new DL.FJuarezInventarioContext())
                {
                    var marcas = (from u in context.Marcas
                                  select u).ToList();
                    foreach (var tempMarca in marcas)
                    {
                        ML.Marca mar = new ML.Marca
                        {
                            IdMarca = tempMarca.IdMarca,
                            Nombre = tempMarca.Nombre
                        };
                        result.Objects.Add(mar);
                    }
                    result.Correct = true;
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
            }

            return result;
        }
    }
}

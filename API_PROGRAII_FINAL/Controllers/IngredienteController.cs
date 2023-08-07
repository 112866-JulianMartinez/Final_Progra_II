using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.Xml;
using System;
using RecetaCliente.Datos;
using RecetaCliente.Dominio;

namespace API_PROGRAII_FINAL.Controllers
{
    public class IngredienteController : Controller
    {
        private AccesoDB accesoDB = AccesoDB.ObtenerInstancia();

        [HttpGet]
        [Route("Api/Ingredientes/ObtenerIngredientes")]
        public IActionResult GetIngredientes()
        {
            try
            {
                List<Ingrediente> lts = accesoDB.Consulta("SP_CONSULTAR_INGREDIENTES");
                return Ok(lts);
            }
            catch(Exception)
            {
                return StatusCode(500, "Error interrno. Intente Luego");
            }
        }
    }
}

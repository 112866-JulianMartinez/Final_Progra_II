using Microsoft.AspNetCore.Mvc;
using RecetaCliente.Datos;
using RecetaCliente.Dominio;

namespace API_PROGRAII_FINAL.Controllers
{
    public class RecetaController : Controller
    {
        private AccesoDB accesoDB = AccesoDB.ObtenerInstancia();

        [HttpPost]
        [Route("Api/Receta/SaveReceta")]
        public IActionResult SaveReceta(Receta receta)
        {
            try
            {
                if(receta == null)
                {
                    return BadRequest("Datos de cliente incorrectos");
                }

                return Ok(accesoDB.AgregarDetalle(receta));
            }
            catch(Exception ex)
            {
                return StatusCode(500, "Error interno! Intente luego");
            }
        }
    }
}

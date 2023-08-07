using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecetaCliente.Dominio
{
    public class Ingrediente
    {
        public int ingredienteId { get; set; }
        public string nombre { get; set; }
        public string unidad { get; set; }

        public Ingrediente()
        {

        }
        public Ingrediente(int IngredienteId, string Nombre, string Unidad)
        {
            ingredienteId = IngredienteId;
            nombre = Nombre;
            unidad = Unidad;
        }
        public Ingrediente(int IngredienteId, string Nombre)
        {
            ingredienteId = IngredienteId;
            nombre = Nombre;
        }
    }
}

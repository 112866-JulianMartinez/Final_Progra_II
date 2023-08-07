using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecetaCliente.Dominio
{
    public class DetalleReceta
    {
        public Ingrediente ingrediente { get; set; }
        public int cantidad { get; set; }

        public DetalleReceta()
        {

        }
        public DetalleReceta(Ingrediente Ingrediente, int Cant)
        {
            ingrediente = Ingrediente;
            cantidad = Cant;
        }
    }
}

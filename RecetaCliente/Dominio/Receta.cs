using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecetaCliente.Dominio
{
    public class Receta
    {
        public int recetaNro { get; set; }
        public string nombre { get; set; }
        public int tipoReceta { get; set; }
        public string cheff { get; set; }
        public List<DetalleReceta> detalleRecetas { get; set; }

        public Receta()
        {
            detalleRecetas = new List<DetalleReceta>();
        }

        public void AgregarDetalle(DetalleReceta detalle)
        {
            detalleRecetas.Add(detalle);
        }

        public void QuitarDetalle(int index)
        {
            detalleRecetas.RemoveAt(index);
        }
    }
}

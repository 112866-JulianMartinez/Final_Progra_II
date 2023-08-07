using RecetaCliente.Dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace RecetasSLN.presentación
{
    public partial class FrmAltaReceta : Form
    {
        
        private static Receta receta = new Receta();
        private static HttpClient client = new HttpClient();

        public FrmAltaReceta()
        {
            InitializeComponent();
            //lblRecetaNro.Text = "Receta #: " + acceso.ProximaReceta().ToString();
            CargarIngredientes("https://localhost:7180/Api/Ingredientes/ObtenerIngredientes");
        }
        
        

        private async void CargarIngredientes(String url)
        {
            var result = await client.GetAsync(url);
            var content = await result.Content.ReadAsStringAsync();
            List<Ingrediente> lst =JsonConvert.DeserializeObject<List<Ingrediente>>(content);
            cboIngredientes.DataSource = lst;
            cboIngredientes.ValueMember = "ingredienteId";
            cboIngredientes.DisplayMember= "nombre";
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (cboIngredientes.SelectedIndex!=-1)
            {
                if (!existe(cboIngredientes.Text))
                {
                    Ingrediente oIngrediente = (Ingrediente)cboIngredientes.SelectedItem;
                    int cant = (int)numCantidad.Value;
                    DetalleReceta detalle = new DetalleReceta(oIngrediente, cant);
                    receta.AgregarDetalle(detalle);
                    dgvDetalle.Rows.Add(new object[] { detalle.ingrediente.ingredienteId, detalle.ingrediente.nombre, detalle.cantidad });
                    lblTotalIngredientes.Text = "Total de ingredientes: " + dgvDetalle.Rows.Count.ToString();
                }
            }
        }

        private bool existe(string SelectedItem)
        {
            bool aux = false;
            foreach(DataGridViewRow item in dgvDetalle.Rows)
            {
                if (item.Cells["Ingrediente"].Value.ToString().Equals(SelectedItem))
                {
                    aux = true;
                    break;
                }
            }
            return aux;
        }

        private async void btnAceptar_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtNombre.Text))
            {
                MessageBox.Show("Debe ingresar un nombre valido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else if (cboTipo.SelectedIndex == -1)
            {
                MessageBox.Show("Debe seleccionar un tipo de receta válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else if (dgvDetalle.Rows.Count < 3)
            {
                MessageBox.Show("Debe insertar al menos tres ingredientes...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            receta.cheff = txtCheff.Text;
            receta.nombre = txtNombre.Text;
            receta.tipoReceta = cboTipo.SelectedIndex + 1;

            string url = "https://localhost:7180/Api/Receta/SaveReceta";

            var json = JsonConvert.SerializeObject(receta);
            var result = await client.PostAsync(url, new StringContent(json,Encoding.UTF8,"aplication/json"));
            if (result.Equals("True"))
            {
                MessageBox.Show("Receta registrada con exito");
                this.Dispose();
            }
            else
            {
                MessageBox.Show("Error al cargar cliente. Intente mas tarde");
            }

        }



        private void LimpiarCampos()
        {
            //txtCheff.Text = string.Empty;
            //txtNombre.Text = string.Empty;
            //cboTipo.SelectedIndex = -1;
            //cboIngredientes.SelectedIndex = -1;
            //numCantidad.Value = 1;
            //dgvDetalle.Rows.Clear();
            //lblRecetaNro.Text = "Receta #: " + acceso.ProximaReceta().ToString();
            //lblTotalIngredientes.Text = "Total de ingredientes:";
        }
        private void FrmAltaReceta_Load(object sender, EventArgs e)
        {
            
        }

        private void dgvDetalle_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("¿Está seguro que desea cancelar?", "Salir", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Dispose();
            }
            else
            {
                return;
            }
        }

        private void dgvDetalle_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvDetalle.CurrentCell.ColumnIndex == 3)
            {
                receta.QuitarDetalle(dgvDetalle.CurrentRow.Index);
                //
                dgvDetalle.Rows.Remove(dgvDetalle.CurrentRow);

                lblTotalIngredientes.Text="Total de ingredientes: " + dgvDetalle.Rows.Count.ToString();
            }
        }
    }
}

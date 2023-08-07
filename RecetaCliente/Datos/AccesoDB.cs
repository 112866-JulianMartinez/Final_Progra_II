using RecetaCliente.Dominio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace RecetaCliente.Datos
{
    public class AccesoDB
    {
        private SqlConnection cnn;
        private static AccesoDB instancia;
        private SqlDataReader reader;

        public AccesoDB()
        {
            cnn = new SqlConnection("Data Source=PC-JULIAN-MARTI\\SQLEXPRESS;Initial Catalog=recetas_db;Integrated Security=True");
        }

        public static AccesoDB ObtenerInstancia()
        {
            if (instancia == null) instancia = new AccesoDB();
            return instancia;
        }

        public List<Ingrediente> Consulta(String SpNombre)
        {
            DataTable table = new DataTable();

            cnn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = SpNombre;
            reader = cmd.ExecuteReader();
            List<Ingrediente> ltsGenerica = new List<Ingrediente>();
            while (reader.Read())
            {
                ltsGenerica.Add(new Ingrediente
                {
                    ingredienteId = reader.GetInt32(0),
                    nombre = reader.GetString(1),
                    unidad = reader.GetString(2)
                });
            }
            reader.Close();
            cnn.Close();
            return ltsGenerica;
        }

        public DataTable Consulta(String SpNombre, List<Parametro> parametros)
        {
            DataTable table = new DataTable();

            cnn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = SpNombre;
            foreach (Parametro param in parametros)
            {
                cmd.Parameters.Add(param.nombre, (SqlDbType)param.valor);
            }
            table.Load(cmd.ExecuteReader());
            cnn.Close();

            return table;
        }

        public bool AgregarDetalle(Receta receta)
        {
            bool ok = false;
            SqlTransaction t = null;

            try
            {
                cnn.Open();
                t = cnn.BeginTransaction();

                SqlCommand cmd = new SqlCommand("SP_INSERTAR_RECETA", cnn, t);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tipo_receta", receta.tipoReceta);
                cmd.Parameters.AddWithValue("@nombre", receta.nombre);
                cmd.Parameters.AddWithValue("@cheff", receta.cheff);

                SqlParameter pOut = new SqlParameter();
                pOut.ParameterName = "@id";
                pOut.DbType = DbType.Int32;
                pOut.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(pOut);

                cmd.ExecuteNonQuery();
                int RecetaNro = (int)pOut.Value;

                SqlCommand cmdDetalle = null;

                foreach (DetalleReceta det in receta.detalleRecetas)
                {
                    cmdDetalle = new SqlCommand("SP_INSERTAR_DETALLES", cnn, t);
                    cmdDetalle.CommandType = CommandType.StoredProcedure;
                    cmdDetalle.Parameters.AddWithValue("@id_receta", RecetaNro);
                    cmdDetalle.Parameters.AddWithValue("@id_ingrediente", det.ingrediente.ingredienteId);
                    cmdDetalle.Parameters.AddWithValue("@cantidad", det.cantidad);
                    cmdDetalle.ExecuteNonQuery();
                }
                t.Commit();
                ok = true;
            }
            catch (Exception ex)
            {
                if (t != null)
                {
                    t.Rollback();
                }
            }
            finally
            {
                if (cnn != null && cnn.State == ConnectionState.Open)
                {
                    cnn.Close();
                }
            }
            return ok;
        }

        public int ProximaReceta()
        {
            int aux;
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter pOut = new SqlParameter();
                pOut.ParameterName = "@next";
                pOut.DbType = DbType.Int32;
                pOut.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(pOut);
                cmd.ExecuteNonQuery();
                aux = (int)pOut.Value;
            }
            catch (Exception ex)
            {
                aux = 1;
            }

            finally
            {
                if (cnn != null && cnn.State == ConnectionState.Open)
                {
                    cnn.Close();
                }
            }

            return aux;
        }
    }
}

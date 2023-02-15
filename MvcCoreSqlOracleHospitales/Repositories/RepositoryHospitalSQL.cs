using MvcCoreSqlOracleHospitales.Models;
using System.Data;
using System.Data.SqlClient;
using System.Numerics;

namespace MvcCoreSqlOracleHospitales.Repositories
{
    public class RepositoryHospitalSQL : IRepositoryHospital
    {
        private DataTable tablahospitales;
        private SqlConnection cn;
        private SqlCommand com;
        private SqlDataAdapter adapter;

        public RepositoryHospitalSQL()
        {
            string connectionString = @"Data Source=LOCALHOST\DESARROLLO;Initial Catalog=HOSPITAL;User ID=sa;Password=";
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
            this.adapter = new SqlDataAdapter("SP_HOSPITALES", connectionString);
            this.adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            this.tablahospitales = new DataTable();
            adapter.Fill(this.tablahospitales);
        }

        public List<Hospital> GetHospitales()
        {
            var consulta = from datos in this.tablahospitales.AsEnumerable()
                           select new Hospital
                           {
                               HOSPITAL_COD = datos.Field<Int32>("HOSPITAL_COD"),
                               NOMBRE = datos.Field<string>("NOMBRE"),
                               DIRECCION = datos.Field<string>("DIRECCION"),
                               TELEFONO = datos.Field<string>("TELEFONO"),
                               NUM_CAMA = datos.Field<Int32>("NUM_CAMA"),
                           };
            /*List<Hospital> hospitales = new List<Hospital>();
            foreach (var row in consulta)
            {
                Hospital hospital = new Hospital
                {
                    HOSPITAL_COD = row.Field<Int32>("HOSPITAL_COD"),
                    NOMBRE = row.Field<string>("NOMBRE"),
                    DIRECCION = row.Field<string>("DIRECCION"),
                    TELEFONO = row.Field<string>("TELEFONO"),
                    NUM_CAMA = row.Field<Int32>("NUM_CAMA"),
                };
                hospitales.Add(hospital);
            }*/
            return consulta.ToList();
        }

        public Hospital DetailsHospital(int id)
        {
            var consulta = from datos in this.tablahospitales.AsEnumerable()
                           where datos.Field<int>("HOSPITAL_COD") == id
                           select new Hospital
                           {
                               HOSPITAL_COD = datos.Field<int>("HOSPITAL_COD"),
                               NOMBRE = datos.Field<string>("NOMBRE"),
                               DIRECCION = datos.Field<string>("DIRECCION"),
                               TELEFONO = datos.Field<string>("TELEFONO"),
                               NUM_CAMA = datos.Field<int>("NUM_CAMA"),
                           };
            return consulta.First();
        }

        private int GetMaximoIdhospital()
        {
            var consulta = from datos in this.tablahospitales.AsEnumerable()
                           select datos;
            if (consulta.Count() == 0)
            {
                return 1;
            }
            else
            {
                return consulta.Max(z => z.Field<int>("HOSPITAL_COD")) + 1;
            }
        }

        public void InsertHospital(string nombre, string direccion, string telefono, int numcama)
        {
            SqlParameter pamid = new SqlParameter("@ID", GetMaximoIdhospital());
            this.com.Parameters.Add(pamid);
            SqlParameter pamnombre = new SqlParameter("@NOMBRE", nombre);
            this.com.Parameters.Add(pamnombre);
            SqlParameter pamdireccion = new SqlParameter("@DIRECCION", direccion);
            this.com.Parameters.Add(pamdireccion);
            SqlParameter pamtelefono = new SqlParameter("@TELEFONO", telefono);
            this.com.Parameters.Add(pamtelefono);
            SqlParameter pamnumcama = new SqlParameter("@NUM_CAMA", numcama);
            this.com.Parameters.Add(pamnumcama);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_INSERT_HOSPITAL";
            this.cn.Open();
            this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }

        public void UpdateHospital(int id, string nombre, string direccion, string telefono, int num_cama)
        {
            SqlParameter pamid = new SqlParameter("@ID", id);
            this.com.Parameters.Add(pamid);
            SqlParameter pamnombre = new SqlParameter("@NOMBRE", nombre);
            this.com.Parameters.Add(pamnombre);
            SqlParameter pamdireccion = new SqlParameter("@DIRECCION", direccion);
            this.com.Parameters.Add(pamdireccion);
            SqlParameter pamtelefono = new SqlParameter("@TELEFONO", telefono);
            this.com.Parameters.Add(pamtelefono);
            SqlParameter pamnumcama = new SqlParameter("@NUM_CAMA", num_cama);
            this.com.Parameters.Add(pamnumcama);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_UPDATE_HOSPITAL";
            this.cn.Open();
            this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }

        public void DeleteHospital(int id)
        {
            SqlParameter pamid = new SqlParameter("@ID", id);
            this.com.Parameters.Add(pamid);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_DELETE_HOSPITAL";
            this.cn.Open();
            this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }
    }
}

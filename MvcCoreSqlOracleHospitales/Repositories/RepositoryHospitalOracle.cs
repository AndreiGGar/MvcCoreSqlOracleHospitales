using MvcCoreSqlOracleHospitales.Models;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using System.Data.SqlClient;

namespace MvcCoreSqlOracleHospitales.Repositories
{
    public class RepositoryHospitalOracle : IRepositoryHospital
    {
        private OracleConnection cn;
        private OracleCommand com;
        private OracleDataAdapter adapter;
        private DataTable tablahospitales;

        public RepositoryHospitalOracle()
        {
            string connectionString = @"Data Source=LOCALHOST:1521/XE;Persist Security Info=True;User ID=SYSTEM;Password=oracle";
            this.cn = new OracleConnection(connectionString);
            this.com = new OracleCommand();
            this.com.Connection = this.cn;
            string oracle = "SELECT * FROM HOSPITAL";
            this.adapter = new OracleDataAdapter(oracle, connectionString);
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
                           where datos.Field<Int32>("HOSPITAL_COD") == id
                           select new Hospital
                           {
                               HOSPITAL_COD = datos.Field<Int32>("HOSPITAL_COD"),
                               NOMBRE = datos.Field<string>("NOMBRE"),
                               DIRECCION = datos.Field<string>("DIRECCION"),
                               TELEFONO = datos.Field<string>("TELEFONO"),
                               NUM_CAMA = datos.Field<Int32>("NUM_CAMA"),
                           };
            return consulta.FirstOrDefault();
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
                return consulta.Max(z => z.Field<Int32>("HOSPITAL_COD")) + 1;
            }
        }

        public void InsertHospital(string nombre, string direccion, string telefono, int numcama)
        {
            int maximo = GetMaximoIdhospital();
            Convert.ToInt32(maximo);
            OracleParameter pamid = new OracleParameter(":ID", maximo);
            this.com.Parameters.Add(pamid);
            OracleParameter pamnombre = new OracleParameter(":NOMBRE", nombre);
            this.com.Parameters.Add(pamnombre);
            OracleParameter pamdireccion = new OracleParameter(":DIRECCION", direccion);
            this.com.Parameters.Add(pamdireccion);
            OracleParameter pamtelefono = new OracleParameter(":TELEFONO", telefono);
            this.com.Parameters.Add(pamtelefono);
            OracleParameter pamnumcama = new OracleParameter(":NUM_CAMA", numcama);
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
            OracleParameter pamid = new OracleParameter(":ID", id);
            this.com.Parameters.Add(pamid);
            OracleParameter pamnombre = new OracleParameter(":NOMBRE", nombre);
            this.com.Parameters.Add(pamnombre);
            OracleParameter pamdireccion = new OracleParameter(":DIRECCION", direccion);
            this.com.Parameters.Add(pamdireccion);
            OracleParameter pamtelefono = new OracleParameter(":TELEFONO", telefono);
            this.com.Parameters.Add(pamtelefono);
            OracleParameter pamnumcama = new OracleParameter(":NUM_CAMA", num_cama);
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
            OracleParameter pamid = new OracleParameter(":ID", id);
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

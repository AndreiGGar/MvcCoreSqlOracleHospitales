namespace MvcCoreSqlOracleHospitales.Models
{
    public class Hospital
    {
        public int HOSPITAL_COD { get; set; }
        public string NOMBRE { get; set; }
        public string DIRECCION { get; set; }
        public string TELEFONO { get; set; }
        public int NUM_CAMA { get; set; }
    }
}

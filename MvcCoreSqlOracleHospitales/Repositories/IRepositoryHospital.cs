using MvcCoreSqlOracleHospitales.Models;

namespace MvcCoreSqlOracleHospitales.Repositories
{
    public interface IRepositoryHospital
    {
        List<Hospital> GetHospitales();
        Hospital DetailsHospital(int id);
        void InsertHospital(string nombre, string direccion, string telefono, int num_camas);
        void UpdateHospital(int id, string nombre, string direccion, string telefono, int num_camas);
        void DeleteHospital(int id);
    }
}

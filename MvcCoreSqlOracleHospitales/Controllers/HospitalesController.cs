using Microsoft.AspNetCore.Mvc;
using MvcCoreSqlOracleHospitales.Models;
using MvcCoreSqlOracleHospitales.Repositories;
using System.Numerics;

namespace MvcCoreSqlOracleHospitales.Controllers
{
    public class HospitalesController : Controller
    {
        RepositoryHospitalOracle repo;

        public HospitalesController()
        {
            this.repo = new RepositoryHospitalOracle();
        }

        public IActionResult Index()
        {
            List<Hospital> hospitales = this.repo.GetHospitales();
            return View(hospitales);
        }

        public IActionResult Details(int id)
        {
            Hospital hospital = this.repo.DetailsHospital(id);
            return View(hospital);
        }

        public IActionResult Insert()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Insert(Hospital hospital)
        {
            this.repo.InsertHospital(hospital.NOMBRE, hospital.DIRECCION, hospital.TELEFONO, hospital.NUM_CAMA);
            /*return View("Index");*/
            return RedirectToAction("Index");
        }

        public IActionResult Update(int id)
        {
            Hospital hospital = this.repo.DetailsHospital(id);
            return View(hospital);
        }

        [HttpPost]
        public IActionResult Update(Hospital hospital)
        {
            this.repo.UpdateHospital(hospital.HOSPITAL_COD, hospital.NOMBRE, hospital.DIRECCION, hospital.TELEFONO, hospital.NUM_CAMA);
            /*return View("Index");*/
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            this.repo.DeleteHospital(id);
            return RedirectToAction("Index");
        }
    }
}

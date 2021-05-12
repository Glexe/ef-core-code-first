using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using cw8.Contexts;
using cw8.Models;
using Microsoft.AspNetCore.Authorization;

namespace cw8.Controllers
{
    [Authorize]
    [Route("api/prescriptions")]
    [ApiController]
    public class PrescriptionsController : ControllerBase
    {
        private readonly HospitalContext _context;

        public PrescriptionsController(HospitalContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Prescription>> GetPrescription([FromBody]PrescriptionInfoDTO prescriptionInfo)
        {
            try
            {
                var result = _context.Prescriptions.Where(prescription =>
                prescription.IdDoctorNavigation.FirstName == prescriptionInfo.DoctorFirstName &&
                prescription.IdDoctorNavigation.LastName == prescriptionInfo.DoctorLastName &&
                prescription.IdPatientNavigation.FirstName == prescriptionInfo.PatientFirstName &&
                prescription.IdPatientNavigation.LastName == prescriptionInfo.PatientLastName &&
                prescription.Prescription_Medicaments.Equals(prescriptionInfo.Medicaments));
                return Ok(result);
            }
            catch { }
            return BadRequest();
        }
    }
}

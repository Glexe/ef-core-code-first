using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace cw8.Models
{
    public partial class Doctor
    {
        public Doctor()
        {
            Prescriptions = new HashSet<Prescription>();
        }

        public int DoctorID { get; set; }
        public string FirstName { get;set; }
        public string LastName { get;set; }
        public string Email { get;set; }

        public virtual ICollection<Prescription> Prescriptions { get; set; }
    }
}

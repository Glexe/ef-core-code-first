using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace cw8.Models
{
    public partial class Medicament
    {
        public Medicament()
        {
            Prescription_Medicaments = new HashSet<Prescription_Medicament>();
        }

        public int MedicamentID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }

        public virtual ICollection<Prescription_Medicament> Prescription_Medicaments { get; set; }
    }
}

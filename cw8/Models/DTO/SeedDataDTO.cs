using System;

namespace cw8.Models
{
    public class SeedDataDTO
    {
        public string[] Names { get; set; }
        public string[] LastNames { get; set; }
        public string[] MedicamentNames { get; set; }
        public string[] MedicamentDescriptions { get; set; }
        public string[] MedicamentTypes { get; set; }
        public string[] Emails { get; set; }
        public string[] MedicamentDetails { get; set; }
        public int[] MedicamentDoses { get; set; }
        public DateTime[] Dates { get; set; }
    }
}

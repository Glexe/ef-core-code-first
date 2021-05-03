using cw8.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cw8.EfConfigurations
{
    public class Prescription_MedicamentConfiguration : IEntityTypeConfiguration<Prescription_Medicament>
    {
        public void Configure(EntityTypeBuilder<Prescription_Medicament> builder)
        {
            builder.ToTable("Prescription_Medicament");
            builder.HasKey(e => new { e.IdMedicament, e.IdPrescription }).HasName("Prescription_Medicament_PK");
            builder.Property(e => e.Dose);
            builder.Property(e => e.Details).IsRequired().HasMaxLength(100);

            builder.HasOne(e => e.IdMedicamentNavigation)
                    .WithMany(m => m.Prescription_Medicaments)
                    .HasForeignKey(p => p.IdMedicament)
                    .HasConstraintName("Prescription_Medicament_Medicament");

            builder.HasOne(e => e.IdPrescriptionNavigation)
                    .WithMany(p => p.Prescription_Medicaments)
                    .HasForeignKey(p => p.IdPrescription)
                    .HasConstraintName("Prescription_Medicament_Prescription");
        }
    }
}

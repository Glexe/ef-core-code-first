using cw8.EfConfigurations;
using cw8.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace cw8.Contexts
{
    public class HospitalContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Medicament> Medicaments { get; set; }
        public DbSet<Prescription_Medicament> Prescription_Medicaments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder); 
            optionsBuilder.UseSqlServer("Data Source=db-mssql;Initial Catalog=s21057;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new PrescriptionConfiguration());
            modelBuilder.ApplyConfiguration(new DoctorConfiguration());
            modelBuilder.ApplyConfiguration(new PatientConfiguration());
            modelBuilder.ApplyConfiguration(new Prescription_MedicamentConfiguration());
            modelBuilder.ApplyConfiguration(new MedicamentConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }

        public void SeedDb(uint numOfRecordsToAdd)
        {
            var data = SeedDataManager.GetData();
            if (data is null) return;
            Random random = new();

            T rand<T>(T[] data)
            {
                return data[random.Next(0, data.Length)];
            }

            for (int i = 0; i < numOfRecordsToAdd; i++)
            {
                var birthdate = rand(data.Dates);
                var doctor = new Doctor
                {
                    FirstName = rand(data.Names),
                    LastName = rand(data.LastNames),
                    Email = rand(data.Emails)
                };
                var patient = new Patient
                {
                    FirstName = rand(data.Names),
                    LastName = rand(data.LastNames),
                    BirthDate = birthdate
                };

                var medicament = new Medicament
                {
                    Name = rand(data.MedicamentNames),
                    Description = rand(data.MedicamentDescriptions),
                    Type = rand(data.MedicamentTypes)
                };

                var date = birthdate.AddYears(random.Next(17, 45));
                var prescription = new Prescription
                {
                    Date = date,
                    DueDate = date.AddDays(random.Next(3, 25)),
                    IdPatientNavigation = patient,
                    IdDoctorNavigation = doctor
                };

                Set<Prescription_Medicament>().Add(new Prescription_Medicament
                {
                    Dose = rand(data.MedicamentDoses),
                    Details = rand(data.MedicamentDetails),
                    IdMedicamentNavigation = medicament,
                    IdPrescriptionNavigation = prescription
                });
            }

            SaveChanges();
        }
    }
}

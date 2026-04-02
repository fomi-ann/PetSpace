using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PetSpace.Core.Domain;


namespace PetSpace.Data
{
    public class PetSpaceDbContext : IdentityDbContext<User, UserRole, Guid>
    {
        public PetSpaceDbContext(DbContextOptions<PetSpaceDbContext> options) : base(options)
        {
        }

        public DbSet<Pet> Pets { get; set; }
        public DbSet<Clinic> Clinics { get; set; }
        public DbSet<Vet> Vets { get; set; }
        public DbSet<PetOwner> PetOwners { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<AppointmentStatusCode> AppointmentStatusCodes { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<PrescriptionStatusCode> PrescriptionStatusCodes { get; set; }
        public DbSet<RegisteredPatient> RegisteredPatients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Обязательно для Identity!

            // User to Pet
            modelBuilder.Entity<PetOwner>()
                .HasOne(po => po.User)
                .WithMany(u => u.PetOwners)
                .HasForeignKey(po => po.UserId);

            modelBuilder.Entity<PetOwner>()
                .HasOne(po => po.Pet)
                .WithMany(p => p.PetOwners)
                .HasForeignKey(po => po.PetId);

            // User to Clinic
            modelBuilder.Entity<RegisteredPatient>()
                .HasOne(rp => rp.User)
                .WithMany(u => u.RegisteredPatients)
                .HasForeignKey(rp => rp.UserId);

            modelBuilder.Entity<RegisteredPatient>()
                .HasOne(rp => rp.Clinic)
                .WithMany()
                .HasForeignKey(rp => rp.ClinicId);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Clinic)
                .WithMany(c => c.Appointments)
                .HasForeignKey(a => a.ClinicId)
                .OnDelete(DeleteBehavior.Restrict);

            // MedicalRecord to Appointment
            modelBuilder.Entity<MedicalRecord>()
                .HasOne(mr => mr.Appointment)
                .WithMany()
                .HasForeignKey(mr => mr.AppId)
                .OnDelete(DeleteBehavior.NoAction);
        }

    }
}

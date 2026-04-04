using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PetSpace.Core.Domain;


namespace PetSpace.Data
{
    public class PetSpaceDbContext : IdentityDbContext<
        User,
        UserRole,
        Guid,
        IdentityUserClaim<Guid>,
        IdentityUserRole<Guid>,
        IdentityUserLogin<Guid>,
        IdentityRoleClaim<Guid>,
        IdentityUserToken<Guid>>
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
                .HasForeignKey(rp => rp.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<RegisteredPatient>()
                .HasOne(rp => rp.Clinic)
                .WithMany(c => c.RegisteredPatients)
                .HasForeignKey(rp => rp.ClinicId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Clinic)
                .WithMany(c => c.Appointments)
                .HasForeignKey(a => a.ClinicId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Vet)
                .WithMany(v => v.Appointments)
                .HasForeignKey(a => a.VetId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Pet)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PetId)
                .OnDelete(DeleteBehavior.NoAction);

            // MedicalRecord to Appointment
            modelBuilder.Entity<MedicalRecord>()
                .HasOne(mr => mr.Appointment)
                .WithMany()
                .HasForeignKey(mr => mr.AppId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Vet)
                .WithOne(v => v.User)
                .HasForeignKey<Vet>(v => v.UserId);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Clinic)
                .WithOne(c => c.User)
                .HasForeignKey<Clinic>(c => c.UserId);

            modelBuilder.Entity<Vet>()
                .HasOne(v => v.Clinic)
                .WithMany(c => c.Vets)
                .HasForeignKey(v => v.ClinicId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Pet>()
                .Property(p => p.PetWeight)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<MedicalRecord>()
                .Property(mr => mr.PetWeight)
                .HasColumnType("decimal(18,2)");
        }

    }
}

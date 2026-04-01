using System.ComponentModel.DataAnnotations;

namespace PetSpace.Core.Domain
{
    public class Vet
    {
        [Key]
        public Guid VetId { get; set; }
        public string VetSpecialization { get; set; }
        public string VetLicence { get; set; }

        public Guid UserId { get; set; }
        public virtual User User { get; set; }

        public Guid ClinicId { get; set; }
        public virtual Clinic Clinic { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        
        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace PetSpace.Core.Domain
{
    public class RegisteredPatient
    {
        [Key]
        public Guid RegPatientId { get; set; }
        public bool IsActive { get; set; }

        public Guid UserId { get; set; }
        public virtual User? User { get; set; }

        public Guid ClinicId { get; set; }
        public virtual Clinic? Clinic { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}

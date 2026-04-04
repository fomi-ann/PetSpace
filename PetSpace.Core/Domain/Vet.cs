using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetSpace.Core.Domain
{
    public class Vet
    {
        [Key]
        public Guid VetId { get; set; }
        public string VetSpecialization { get; set; } = string.Empty;
        public string VetLicence { get; set; } = string.Empty;

        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        public Guid ClinicId { get; set; }
        [ForeignKey("ClinicId")]
        public virtual Clinic? Clinic { get; set; }

        public bool IsVerified { get; set; } = false;
        public DateTime? VerifiedAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        
        public virtual ICollection<Appointment>? Appointments { get; set; }
    }
}

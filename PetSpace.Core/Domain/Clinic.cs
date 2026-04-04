using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetSpace.Core.Domain
{
    public class Clinic
    {
        [Key]
        public Guid ClinicId { get; set; }
        public string ClinicName { get; set; } = string.Empty;
        public string ClinicRegCode { get; set; } = string.Empty;
        public string ClinicAddress { get; set; } = string.Empty;
        public string ClinicPhone { get; set; } = string.Empty;
        public string ClinicEmail { get; set; } = string.Empty;

        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        public bool IsVerified { get; set; } = false;
        public DateTime? VerifiedAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public virtual ICollection<Vet>? Vets { get; set; } 
        public virtual ICollection<Appointment>? Appointments { get; set; }
        public virtual ICollection<RegisteredPatient>? RegisteredPatients { get; set; }

    }
}

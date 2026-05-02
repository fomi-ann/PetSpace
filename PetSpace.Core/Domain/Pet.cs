using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetSpace.Core.Domain
{
    public class Pet
    {
        [Key]
        public Guid PetId { get; set; }
        public string PetName { get; set; } = string.Empty;
        public string PetSpecies { get; set; } = string.Empty;
        public string PetBreed { get; set; } = string.Empty;
        public string PetGender { get; set; } = string.Empty;
        public DateTime? PetBirthDate { get; set; }
        public string PetMicrochipNr { get; set; } = string.Empty;
        public decimal? PetWeight { get; set; }
        
        public Guid OwnerId { get; set; }

        [ForeignKey("OwnerId")]
        public virtual User? Owner { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        
        public virtual ICollection<PetOwner>? PetOwners { get; set; }
        public virtual ICollection<Appointment>? Appointments { get; set; }
        public virtual ICollection<MedicalRecord>? MedicalRecords { get; set; }
    }
}

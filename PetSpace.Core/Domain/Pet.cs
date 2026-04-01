using System.ComponentModel.DataAnnotations;

namespace PetSpace.Core.Domain
{
    public class Pet
    {
        [Key]
        public Guid PetId { get; set; }
        public string PetName { get; set; }
        public string PetSpecies { get; set; }
        public string PetBreed { get; set; }
        public string PetGender { get; set; }
        public DateTime PetBirthDate { get; set; }
        public string PetMicrochipNr { get; set; }
        public decimal PetWeight { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        
        public virtual ICollection<PetOwner> PetOwners { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<MedicalRecord> MedicalRecords { get; set; }
    }
}

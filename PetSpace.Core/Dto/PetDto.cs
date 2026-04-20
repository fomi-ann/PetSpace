
namespace PetSpace.Core.Dto
{
    public class PetDto
    {
        public string PetName { get; set; } = string.Empty;
        public string PetSpecies { get; set; } = string.Empty;
        public string PetBreed { get; set; } = string.Empty;
        public string PetGender { get; set; } = string.Empty;
        public DateTime? PetBirthDate { get; set; }
        public string PetMicrochipNr { get; set; } = string.Empty;
        public decimal? PetWeight { get; set; }
    }
}

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetSpace.Core.Domain
{
    public class User : IdentityUser<Guid>
    {
        public string UserFirstName { get; set; } = string.Empty;
        public string UserLastName { get; set; } = string.Empty;
        // Email
        // PasswordHash
        // PhoneNumber
        // UserName
        // EmailConfirmed
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        

        public ICollection<PetOwner>? PetOwners { get; set; }
        public ICollection<Vet>? Vets { get; set; }
        public ICollection<RegisteredPatient>? RegisteredPatients { get; set; }
    }
}

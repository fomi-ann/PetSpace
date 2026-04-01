using System.ComponentModel.DataAnnotations;


namespace PetSpace.Core.Domain
{
    public class PetOwner
    {
        [Key]
        public Guid PetOwnerId { get; set; }
        public bool IsPrimaryOwner { get; set; }

        public Guid UserId { get; set; }
        public virtual User User { get; set; }

        public Guid PetId { get; set; }
        public virtual Pet Pet { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace PetSpace.Core.Domain
{
    public class PetOwner
    {
        [Key]
        public Guid PetOwnerId { get; set; }
        public bool IsPrimaryOwner { get; set; }

        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        public Guid? PetId { get; set; }

        [ForeignKey("PetId")]
        public virtual Pet? Pet { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}

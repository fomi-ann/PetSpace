using System.ComponentModel.DataAnnotations;

namespace PetSpace.Core.Domain
{
    public class PrescriptionStatusCode
    {
        [Key]
        public int PrescStatusCodeId { get; set; }
        public string PrescStatusName { get; set; }
    }
}

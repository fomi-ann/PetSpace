using System.ComponentModel.DataAnnotations;

namespace PetSpace.Core.Domain
{
    public class AppointmentStatusCode
    {
        [Key]
        public int AppStatusCodeId { get; set; }
        public string AppStatusCode { get; set; }
    }
}

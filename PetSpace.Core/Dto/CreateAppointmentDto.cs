using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpace.Core.Dto
{
    public class CreateAppointmentDto
    {
        public Guid PetId { get; set; }
        public Guid VetId { get; set; }
        public Guid ClinicId { get; set; }
        public DateTime AppDateTime { get; set; }
        public string AppReason { get; set; } = string.Empty;
    }
}

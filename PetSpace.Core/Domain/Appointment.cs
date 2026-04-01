using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpace.Core.Domain
{
    public class Appointment
    {
        [Key]
        public Guid AppId { get; set; }
        public DateTime AppDateTime { get; set; }
        public string AppReason { get; set; }

        public int AppStatusCodeId { get; set; }
        public virtual AppointmentStatusCode Status { get; set; }

        public Guid PetId { get; set; }
        public virtual Pet Pet { get; set; }

        public Guid VetId { get; set; }
        public virtual Vet Vet { get; set; }

        public Guid ClinicId { get; set; }
        public virtual Clinic Clinic { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}

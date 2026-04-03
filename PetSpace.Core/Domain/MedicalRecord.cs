using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpace.Core.Domain
{
    public class MedicalRecord
    {
        [Key]
        public Guid MedicalRecordId { get; set; }
        public string Diagnosis { get; set; } = string.Empty;
        public string TreatmentPlan { get; set; } = string.Empty;
        public decimal PetWeight { get; set; }
        public string Comment { get; set; } = string.Empty;

        public Guid AppId { get; set; }
        public virtual Appointment? Appointment { get; set; }

        public Guid PetId { get; set; }
        public virtual Pet? Pet { get; set; }

        public Guid VetId { get; set; }
        public virtual Vet? Vet { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;


        public virtual ICollection<Prescription>? Prescriptions { get; set; }
    }
}

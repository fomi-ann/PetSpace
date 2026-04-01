using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpace.Core.Domain
{
    public class Prescription
    {
        [Key]
        public Guid PrescriptionId { get; set; }
        public string MedicationName { get; set; }
        public string Dosage { get; set; }
        public string Duration { get; set; }
        public string Comment { get; set; }
        public DateTime ExpiryDate { get; set; }

        public int PrescriptionStatusCodeId { get; set; }
        public virtual PrescriptionStatusCode Status { get; set; }

        public Guid MedicalRecordId { get; set; }
        public virtual MedicalRecord MedicalRecord { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}

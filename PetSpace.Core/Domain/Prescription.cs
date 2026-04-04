using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpace.Core.Domain
{
    public class Prescription
    {
        [Key]
        public Guid PrescriptionId { get; set; }
        public string MedicationName { get; set; } = string.Empty;
        public string Dosage { get; set; } = string.Empty;
        public string Duration { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public DateTime ExpiryDate { get; set; }

        public int PrescStatusCodeId { get; set; }

        [ForeignKey("PrescStatusCodeId")]
        public virtual PrescriptionStatusCode? Status { get; set; }

        public Guid MedicalRecordId { get; set; }

        [ForeignKey("MedicalRecordId")]
        public virtual MedicalRecord? MedicalRecord { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}

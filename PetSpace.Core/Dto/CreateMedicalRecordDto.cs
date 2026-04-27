using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpace.Core.Dto
{
    public class CreateMedicalRecordDto
    {
        public Guid AppId { get; set; }
        public string Diagnosis { get; set; } = string.Empty;
        public string TreatmentPlan { get; set; } = string.Empty;
        public decimal PetWeight { get; set; }
        public string Comment { get; set; } = string.Empty;
    }
}

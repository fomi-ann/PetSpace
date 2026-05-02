using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpace.Core.Dto
{
    public class SharePetDto
    {
        public Guid PetId { get; set; }
        public string Email { get; set; } = string.Empty;
    }
}

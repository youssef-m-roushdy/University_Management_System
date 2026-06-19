using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University_Management_System.Domain.Entities
{
    public class BaseEntities <TKey>
    {
        public TKey Id { get; set; }
        // All entities must has created and updated at for tracking and auditing purposes
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow; 
    }
}

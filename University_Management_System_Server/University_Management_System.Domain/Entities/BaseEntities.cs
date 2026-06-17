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
    }
}

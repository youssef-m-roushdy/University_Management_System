using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Domain.Entities.Identity;

namespace University_Management_System.Domain.Contracts
{
    public interface IAssistantRepository : IGenericRepository<Assistant, string>
    {
        
    }
}
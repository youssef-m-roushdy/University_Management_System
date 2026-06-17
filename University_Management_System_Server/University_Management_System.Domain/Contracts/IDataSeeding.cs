using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace University_Management_System.Domain.Contracts
{
    public interface IDataSeeding
    {
        Task SeedDataInfoAsync();
        Task SeedIdentityDataAsync();
    }
}

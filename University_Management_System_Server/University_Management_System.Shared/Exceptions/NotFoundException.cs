using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University_Management_System.Shared.Exceptions
{
    /// <summary>
    /// Exception thrown when a requested resource is not found
    /// </summary>
    public class NotFoundException : BaseException
    {
        public NotFoundException(string message)
            : base(message, "NOT_FOUND", 404)
        {
        }

        public NotFoundException(string message, Exception innerException)
            : base(message, "NOT_FOUND", 404, innerException)
        {
        }
    }
}
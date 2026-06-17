using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University_Management_System.Shared.Exceptions
{
    /// <summary>
    /// Exception thrown when there is a conflict with the current state of the resource
    /// </summary>
    public class ConflictException : BaseException
    {
        public ConflictException(string message)
            : base(message, "CONFLICT", 409)
        {
        }

        public ConflictException(string message, Exception innerException)
            : base(message, "CONFLICT", 409, innerException)
        {
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University_Management_System.Shared.Exceptions
{
    /// <summary>
    /// Exception thrown when authentication is required but not provided
    /// </summary>
    public class UnauthorizedException : BaseException
    {
        public UnauthorizedException(string message = "Authentication required")
            : base(message, "UNAUTHORIZED", 401)
        {
        }

        public UnauthorizedException(string message, Exception innerException)
            : base(message, "UNAUTHORIZED", 401, innerException)
        {
        }
    }
}
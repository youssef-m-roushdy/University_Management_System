using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University_Management_System.Shared.Exceptions
{
    /// <summary>
    /// Exception thrown when access to a resource is forbidden
    /// </summary>
    public class ForbiddenException : BaseException
    {
        public ForbiddenException(string message = "Access forbidden")
            : base(message, "FORBIDDEN", 403)
        {
        }

        public ForbiddenException(string message, Exception innerException)
            : base(message, "FORBIDDEN", 403, innerException)
        {
        }
    }
}
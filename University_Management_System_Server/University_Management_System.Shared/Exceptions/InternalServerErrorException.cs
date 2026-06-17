using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University_Management_System.Shared.Exceptions
{
    /// <summary>
    /// Exception thrown when an internal server error occurs
    /// </summary>
    public class InternalServerErrorException : BaseException
    {
        public InternalServerErrorException(string message = "Internal server error")
            : base(message, "INTERNAL_SERVER_ERROR", 500)
        {
        }

        public InternalServerErrorException(string message, Exception innerException)
            : base(message, "INTERNAL_SERVER_ERROR", 500, innerException)
        {
        }
    }
}
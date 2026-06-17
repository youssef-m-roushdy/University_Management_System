using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University_Management_System.Shared.Exceptions
{
    /// <summary>
    /// Exception thrown when a bad request is made
    /// </summary>
    public class BadRequestException : BaseException
    {
        public BadRequestException(string message)
            : base(message, "BAD_REQUEST", 400)
        {
        }

        public BadRequestException(string message, Exception innerException)
            : base(message, "BAD_REQUEST", 400, innerException)
        {
        }
    }
}
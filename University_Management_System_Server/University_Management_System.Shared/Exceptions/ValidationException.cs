using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University_Management_System.Shared.Exceptions
{
    /// <summary>
    /// Exception thrown when validation fails
    /// </summary>
    public class ValidationException : BaseException
    {
        public IEnumerable<string> Errors { get; set; }

        public ValidationException(IEnumerable<string> errors)
            : base("Validation failed", "VALIDATION_ERROR", 400)
        {
            Errors = errors;
        }

        public ValidationException(string message, IEnumerable<string> errors)
            : base(message, "VALIDATION_ERROR", 400)
        {
            Errors = errors;
        }

        public ValidationException(string message, IEnumerable<string> errors, Exception innerException)
            : base(message, "VALIDATION_ERROR", 400, innerException)
        {
            Errors = errors;
        }
    }
}
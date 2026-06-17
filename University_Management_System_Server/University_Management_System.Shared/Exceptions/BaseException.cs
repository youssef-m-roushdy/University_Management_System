using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University_Management_System.Shared.Exceptions
{
    /// <summary>
    /// Base exception class for all application-specific exceptions
    /// </summary>
    public abstract class BaseException : Exception
    {
        public string ErrorCode { get; protected set; }
        public int StatusCode { get; protected set; }

        protected BaseException(string message, string errorCode, int statusCode)
            : base(message)
        {
            ErrorCode = errorCode;
            StatusCode = statusCode;
        }

        protected BaseException(string message, string errorCode, int statusCode, Exception innerException)
            : base(message, innerException)
        {
            ErrorCode = errorCode;
            StatusCode = statusCode;
        }
    }
}
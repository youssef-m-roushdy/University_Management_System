using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace University_Management_System.Shared.Respones
{
    public class Response<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public string? Errors { get; set; }
        public string? Message { get; set; }

        // Success response
        public static Response<T> SuccessResponse(T data)
        {
            return new Response<T>
            {
                Success = true,
                Data = data,
                Errors = null,
                Message = "Operation completed successfully"
            };
        }

        // Error response
        public static Response<T> ErrorResponse(string error)
        {
            return new Response<T>
            {
                Success = false,
                Data = default(T),
                Errors = error,
                Message = "Operation failed"
            };
        }
    }
}
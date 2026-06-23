using System;
using System.Collections.Generic;
using System.Linq;

namespace University_Management_System.Shared.Responses
{
    // 1. BASE API RESPONSE
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public List<ApiError> Errors { get; set; }
        public DateTime Timestamp { get; set; }

        public ApiResponse()
        {
            Timestamp = DateTime.UtcNow;
            Errors = new List<ApiError>();
        }

        // Factory Methods
        public static ApiResponse<T> SuccessResponse(T data, string message = "Operation completed successfully", int statusCode = 200)
        {
            return new ApiResponse<T>
            {
                Success = true,
                StatusCode = statusCode,
                Message = message,
                Data = data
            };
        }

        public static ApiResponse<T> ErrorResponse(string message, int statusCode = 400, List<ApiError> errors = null)
        {
            return new ApiResponse<T>
            {
                Success = false,
                StatusCode = statusCode,
                Message = message,
                Data = default,
                Errors = errors ?? new List<ApiError>()
            };
        }

        public static ApiResponse<T> UnauthorizedResponse(string message = "Authentication failed")
        {
            return ErrorResponse(message, 401);
        }

        public static ApiResponse<T> NotFoundResponse(string message = "Resource not found")
        {
            return ErrorResponse(message, 404);
        }

        public static ApiResponse<T> ValidationErrorResponse(List<ApiError> errors, string message = "Validation failed")
        {
            return ErrorResponse(message, 400, errors);
        }

        public static ApiResponse<T> ConflictErrorResponse(string message = "Conflict error")
        {
            return ErrorResponse(message, 409);
        }

        public static ApiResponse<T> ServerErrorResponse(string message = "Internal server error")
        {
            return ErrorResponse(message, 500);
        }  
    }

    // 2. PAGED RESPONSE - Inherits from ApiResponse
    public class PagedResponse<T> : ApiResponse<IEnumerable<T>>
    {
        public PaginationMetadata Pagination { get; set; }

        public PagedResponse() : base()
        {
            Data = new List<T>();
        }

        // ✅ SUCCESS - With paginated data
        public static PagedResponse<T> SuccessResponse(
            IEnumerable<T> data,
            int pageNumber,
            int pageSize,
            int totalCount,
            string message = "Operation completed successfully")
        {
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            return new PagedResponse<T>
            {
                Success = true,
                StatusCode = 200,
                Message = message,
                Data = data ?? new List<T>(),
                Pagination = new PaginationMetadata
                {
                    CurrentPage = pageNumber,
                    PageSize = pageSize,
                    TotalPages = totalPages,
                    TotalCount = totalCount,
                    HasNextPage = pageNumber < totalPages,
                    HasPreviousPage = pageNumber > 1
                }
            };
        }

        // ✅ SUCCESS - Without data (for delete/empty responses)
        public static new PagedResponse<T> SuccessResponse(string message = "Operation completed successfully")
        {
            return new PagedResponse<T>
            {
                Success = true,
                StatusCode = 200,
                Message = message,
                Data = new List<T>(),
                Pagination = null
            };
        }

        // ❌ ERROR RESPONSE (Overrides base)
        public static new PagedResponse<T> ErrorResponse(
            string message,
            int statusCode = 400,
            List<ApiError> errors = null)
        {
            return new PagedResponse<T>
            {
                Success = false,
                StatusCode = statusCode,
                Message = message,
                Data = new List<T>(),
                Pagination = null,
                Errors = errors ?? new List<ApiError>()
            };
        }

        // ❌ UNAUTHORIZED
        public static new PagedResponse<T> UnauthorizedResponse(string message = "Authentication failed")
        {
            return ErrorResponse(message, 401);
        }

        // ❌ NOT FOUND
        public static new PagedResponse<T> NotFoundResponse(string message = "Resource not found")
        {
            return ErrorResponse(message, 404);
        }

        // ❌ VALIDATION ERROR
        public static new PagedResponse<T> ValidationErrorResponse(List<ApiError> errors, string message = "Validation failed")
        {
            return ErrorResponse(message, 400, errors);
        }

        // ❌ SERVER ERROR
        public static new PagedResponse<T> ServerErrorResponse(string message = "Internal server error")
        {
            return ErrorResponse(message, 500);
        }
    }

    // 3. PAGINATION METADATA
    public class PaginationMetadata
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
    }

    // 4. API ERROR
    public class ApiError
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string Field { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Application.Dtos.CourseUploadDtos;
using MediatR;
using Microsoft.AspNetCore.Http;
using University_Management_System.Shared.Responses;

namespace University_Management_System.Application.Commands.CourseUploads
{
    public class CreateCourseUploadCommand : IRequest<ApiResponse<int>>
    {
        public CreateCourseUploadDto CourseUploadDto { get; set; }
        public IFormFile File { get; set; }
        public string UserId { get; set; } = string.Empty;
    }
}
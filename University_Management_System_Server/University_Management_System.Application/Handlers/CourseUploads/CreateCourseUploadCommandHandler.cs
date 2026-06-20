using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using University_Management_System.Application.Commands.CourseUploads;
using University_Management_System.Application.Contracts;
using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Models;
using MediatR;
using University_Management_System.Shared.Responses;

namespace University_Management_System.Application.Handlers.CourseUploads
{
    public class CreateCourseUploadCommandHandler : IRequestHandler<CreateCourseUploadCommand, ApiResponse<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IMapper _mapper;
        public CreateCourseUploadCommandHandler(IUnitOfWork unitOfWork, ICloudinaryService cloudinaryService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _cloudinaryService = cloudinaryService;
            _mapper = mapper;
        }
        public async Task<ApiResponse<int>> Handle(CreateCourseUploadCommand request, CancellationToken cancellationToken)
        {
            var course = await _unitOfWork.Courses.GetByIdAsync(request.CourseUploadDto.CourseId);
            if (course is null)                
                return ApiResponse<int>.ErrorResponse("Course not found");

            var fileId = Guid.NewGuid().ToString();

    

            var courseUpload = _mapper.Map<CourseUpload>(request.CourseUploadDto);

            await _unitOfWork.CourseUploads.AddAsync(courseUpload);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<int>.SuccessResponse(courseUpload.Id);
        }
    }
}
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using University_Management_System.Application.Commands.Instructors;
using University_Management_System.Application.Dtos.InstructorDtos;
using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Identity;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.Instructors
{
    public class UpdateInstructorCommandHandler : IRequestHandler<UpdateInstructorCommand, InstructorDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public UpdateInstructorCommandHandler(IUnitOfWork unitOfWork, UserManager<User> userManager, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<InstructorDto> Handle(UpdateInstructorCommand request, CancellationToken cancellationToken)
        {
            // ─── 1. Check if Instructor exists ────────────────────────────────
            var instructor = await _unitOfWork.Instructors
                .GetInstructorByUserIdAsync(request.Id);
            
            if (instructor == null)
                throw new NotFoundException($"Instructor with ID '{request.Id}' not found.");

            // ─── 3. Validate Department (if provided) ──────────────────────────
         
            var department = await _unitOfWork.Departments
                    .GetByIdAsync(request.Dto.DepartmentId);
                
                if (department == null)
                    throw new NotFoundException($"Department with ID '{request.Dto.DepartmentId}' not found.");
                
            instructor.DepartmentId = request.Dto.DepartmentId;
            instructor.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Instructors.UpdateAsync(instructor);
            await _unitOfWork.SaveChangesAsync();

            // ─── 6. Return mapped DTO ─────────────────────────────────────────
            return _mapper.Map<InstructorDto>(instructor);
        }
    }
}
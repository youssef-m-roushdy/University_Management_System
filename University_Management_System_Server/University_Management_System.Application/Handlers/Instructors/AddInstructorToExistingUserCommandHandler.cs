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
    public class AddInstructorToExistingUserCommandHandler : IRequestHandler<AddInstructorToExistingUserCommand, InstructorDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public AddInstructorToExistingUserCommandHandler(
            IUnitOfWork unitOfWork,
            UserManager<User> userManager,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<InstructorDto> Handle(AddInstructorToExistingUserCommand request, CancellationToken cancellationToken)
        {
            // ─── 1. Find User by Email ──────────────────────────────────────
            var user = await _userManager.FindByEmailAsync(request.Dto.UserEmail);
            if (user == null)
                throw new NotFoundException($"User with email '{request.Dto.UserEmail}' not found.");

            // ─── 2. Check if User already has an Instructor profile ──────────
            var existingInstructor = await _unitOfWork.Instructors
                .GetInstructorByUserIdAsync(user.Id);

            if (existingInstructor != null)
                throw new ValidationException(new List<string> {
                    $"User '{request.Dto.UserEmail}' already has an Instructor profile."
                });

            // ─── 3. Check if User already has "Instructor" role ──────────────
            var isInstructor = await _userManager.IsInRoleAsync(user, "Instructor");
            if (isInstructor)
                throw new ValidationException(new List<string> {
                    $"User '{request.Dto.UserEmail}' already has the Instructor role."
                });

            // ─── 4. Validate Department ──────────────────────────────────────
            var department = await _unitOfWork.Departments
                .GetByIdAsync(request.Dto.DepartmentId);
            
            if (department == null)
                throw new NotFoundException($"Department with ID '{request.Dto.DepartmentId}' not found.");

            // ─── 5. Create Instructor Profile ──────────────────────────────────
            var instructor = new Instructor
            {
                Id = user.Id,
                DepartmentId = request.Dto.DepartmentId,     
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Instructors.AddAsync(instructor);
            await _unitOfWork.SaveChangesAsync();

            // ─── 6. Assign "Instructor" role to User ──────────────────────────
            var roleResult = await _userManager.AddToRoleAsync(user, "Instructor");
            if (!roleResult.Succeeded)
            {
                var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                throw new Exception($"Failed to assign Instructor role: {errors}");
            }

            // ─── 7. Return mapped DTO ─────────────────────────────────────────
            return _mapper.Map<InstructorDto>(instructor);
        }
    }
}
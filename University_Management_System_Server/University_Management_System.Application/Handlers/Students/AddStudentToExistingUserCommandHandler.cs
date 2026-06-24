using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using University_Management_System.Application.Commands.Students;
using University_Management_System.Application.Dtos.StudentDtos;
using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Identity;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.Students
{
    public class AddStudentToExistingUserCommandHandler : IRequestHandler<AddStudentToExistingUserCommand, StudentDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public AddStudentToExistingUserCommandHandler(
            IUnitOfWork unitOfWork,
            UserManager<User> userManager,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<StudentDto> Handle(AddStudentToExistingUserCommand request, CancellationToken cancellationToken)
        {
            // ─── 1. Find User by Email ──────────────────────────────────────
            var user = await _userManager.FindByEmailAsync(request.Dto.UserEmail);
            if (user == null)
                throw new NotFoundException($"User with email '{request.Dto.UserEmail}' not found.");

            // ─── 2. Check if User already has a Student profile ──────────────
            var existingStudent = await _unitOfWork.Students
                .GetStudentByUserIdAsync(user.Id);

            if (existingStudent != null)
                throw new ValidationException(new List<string> {
                    $"User '{request.Dto.UserEmail}' already has a Student profile."
                });

            // ─── 3. Check if User already has "Student" role ──────────────────
            var isStudent = await _userManager.IsInRoleAsync(user, "Student");
            if (isStudent)
                throw new ValidationException(new List<string> {
                    $"User '{request.Dto.UserEmail}' already has the Student role."
                });

            // ─── 4. Validate Department ──────────────────────────────────────
            var department = await _unitOfWork.Departments
                .GetByIdAsync(request.Dto.DepartmentId);
            if (department == null)
                throw new NotFoundException($"Department with ID '{request.Dto.DepartmentId}' not found.");

            // ─── 5. Validate Specialization (if provided) ──────────────────
            if (request.Dto.SpecializationId.HasValue)
            {
                var specialization = await _unitOfWork.Specializations
                    .GetByIdAsync(request.Dto.SpecializationId.Value);
                if (specialization == null)
                    throw new NotFoundException($"Specialization with ID '{request.Dto.SpecializationId}' not found.");
            }

            // ─── 6. Validate Academic Code is unique ────────────────────────
            var existingByCode = await _unitOfWork.Students
                .GetStudentByAcademicCodeAsync(request.Dto.AcademicCode);
            if (existingByCode != null)
                throw new ValidationException(new List<string> {
                    $"Academic code '{request.Dto.AcademicCode}' is already assigned to another student."
                });

            // ─── 7. Create Student Profile using AutoMapper ──────────────────
            var student = _mapper.Map<Student>(request.Dto);
            student.Id = user.Id;  // Shadow table: same Id as User
            student.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.Students.AddAsync(student);
            await _unitOfWork.SaveChangesAsync();

            // ─── 8. Assign "Student" role to User ────────────────────────────
            var roleResult = await _userManager.AddToRoleAsync(user, "Student");
            if (!roleResult.Succeeded)
            {
                var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                throw new Exception($"Failed to assign Student role: {errors}");
            }

            // ─── 9. Return mapped DTO ───────────────────────────────────────
            return _mapper.Map<StudentDto>(student);
        }
    }
}
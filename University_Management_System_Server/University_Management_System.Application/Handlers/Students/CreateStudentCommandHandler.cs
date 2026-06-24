using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using University_Management_System.Application.Commands.Students;
using University_Management_System.Application.Contracts;
using University_Management_System.Application.Dtos.StudentDtos;
using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Identity;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.Students
{
    public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, StudentDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public CreateStudentCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<StudentDto> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
        {
            // ─── 1. Validate Department ──────────────────────────────────────
            var department = await _unitOfWork.Departments.GetByIdAsync(request.Dto.DepartmentId);
            if (department == null)
                throw new NotFoundException($"Department with ID '{request.Dto.DepartmentId}' not found.");

            // ─── 2. Validate Specialization (if provided) ──────────────────
            if (request.Dto.SpecializationId.HasValue)
            {
                var specialization = await _unitOfWork.Specializations
                    .GetByIdAsync(request.Dto.SpecializationId.Value);
                if (specialization == null)
                    throw new NotFoundException($"Specialization with ID '{request.Dto.SpecializationId}' not found.");
            }

            // ─── 3. Create User with "Student" role ──────────────────────
            var user = await _userService.CreateUserAsync(request.Dto, "Student");

            // ─── 4. Create Student Profile using AutoMapper ────────────────
            var student = _mapper.Map<Student>(request.Dto);
            student.Id = user.Id;  // Same Id as User (shadow table)
            student.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.Students.AddAsync(student);
            await _unitOfWork.SaveChangesAsync();

            // ─── 5. Return mapped DTO ────────────────────────────────────────
            return _mapper.Map<StudentDto>(student);
        }
    }
}
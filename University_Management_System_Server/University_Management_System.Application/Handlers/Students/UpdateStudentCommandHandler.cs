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
    public class UpdateStudentCommandHandler : IRequestHandler<UpdateStudentCommand, StudentDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateStudentCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<StudentDto> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
        {
            // ─── 1. Validate Student Exists ──────────────────────────────────
            var student = await _unitOfWork.Students
                .GetByIdAsync(request.Id);

            if (student == null)
                throw new NotFoundException($"Student with ID '{request.Id}' not found.");

            // ─── 2. Validate Department ──────────────────────────────────────
            var department = await _unitOfWork.Departments
                .GetByIdAsync(request.Dto.DepartmentId);
            
            if (department == null)
                throw new NotFoundException($"Department with ID '{request.Dto.DepartmentId}' not found.");

            // ─── 3. Validate Specialization (if provided, can be null) ──────
            if (request.Dto.SpecializationId.HasValue)
            {
                var specialization = await _unitOfWork.Specializations
                    .GetByIdAsync(request.Dto.SpecializationId.Value);
                
                if (specialization == null)
                    throw new NotFoundException($"Specialization with ID '{request.Dto.SpecializationId}' not found.");
            }

            // ─── 4. Validate Academic Code (if changed) ──────────────────────
            if (!string.IsNullOrEmpty(request.Dto.AcademicCode))
            {
                var existingStudent = await _unitOfWork.Students
                    .GetStudentByAcademicCodeAsync(request.Dto.AcademicCode);
                
                if (existingStudent != null && existingStudent.Id != request.Id)
                    throw new ValidationException(new List<string> { 
                        $"Academic code '{request.Dto.AcademicCode}' is already assigned to another student." 
                    });
            }

            // ─── 5. Update Student Properties (Full Update - ALL fields) ─────
            // ✅ No null checks - all fields must be provided
            // ✅ SpecializationId CAN be null (set to null if not provided)
            student.AcademicCode = request.Dto.AcademicCode;
            student.Level = request.Dto.Level;
            student.TotalCredits = request.Dto.TotalCredits;
            student.AllowedCredits = request.Dto.AllowedCredits;
            student.TotalGPA = request.Dto.TotalGPA;
            student.DepartmentId = request.Dto.DepartmentId;
            student.SpecializationId = request.Dto.SpecializationId; // ✅ Can be null

            // ─── 6. Update Timestamp ─────────────────────────────────────────
            student.UpdatedAt = DateTime.UtcNow;

            // ─── 7. Save Changes ─────────────────────────────────────────────
            await _unitOfWork.Students.UpdateAsync(student);
            await _unitOfWork.SaveChangesAsync();

            // ─── 8. Return Updated Student ──────────────────────────────────
            return _mapper.Map<StudentDto>(student);
        }
    }
}
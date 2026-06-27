using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using University_Management_System.Application.Commands.AcademicSchedules;
using University_Management_System.Application.Contracts;
using University_Management_System.Application.Dtos.AcademicScheduleDtos;
using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Models;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.AcademicSchedules
{
    public class CreateAcademicScheduleCommandHandler : IRequestHandler<CreateAcademicScheduleCommand, AcademicScheduleDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IR2StorageService _r2StorageService;
        private readonly IMapper _mapper;

        public CreateAcademicScheduleCommandHandler(
            IUnitOfWork unitOfWork,
            IR2StorageService r2StorageService,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _r2StorageService = r2StorageService;
            _mapper = mapper;
        }

        public async Task<AcademicScheduleDto> Handle(CreateAcademicScheduleCommand request, CancellationToken cancellationToken)
        {
            // ─── 1. Validate Department ──────────────────────────────────────
            var department = await _unitOfWork.Departments.GetByIdAsync(request.Dto.DepartmentId);
            if (department == null)
                throw new NotFoundException($"Department with ID '{request.Dto.DepartmentId}' not found.");

            // ─── 2. Validate Semester ──────────────────────────────────────
            var semester = await _unitOfWork.Semesters.GetByIdAsync(request.Dto.SemesterId);
            if (semester == null)
                throw new NotFoundException($"Semester with ID '{request.Dto.SemesterId}' not found.");

            // ─── 3. Validate Study Year ──────────────────────────────────────
            var studyYear = await _unitOfWork.StudyYears.GetByIdAsync(request.Dto.StudyYearId);
            if (studyYear == null)
                throw new NotFoundException($"Study Year with ID '{request.Dto.StudyYearId}' not found.");

            // ─── 4. Check if schedule already exists ──────────────────────
            var exists = await _unitOfWork.AcademicSchedules.ExistsAsync(
                request.Dto.DepartmentId,
                request.Dto.SemesterId,
                request.Dto.Title);
            
            if (exists)
                throw new ValidationException(new List<string> {
                    $"Schedule with title '{request.Dto.Title}' already exists for this department and semester."
                });

            // ─── 5. Upload file to R2 (if provided) ──────────────────────────
            string fileKey = string.Empty;
            if (request.Dto.File != null && request.Dto.File.Length > 0)
            {
                // Generate unique key for the file
                var fileExtension = Path.GetExtension(request.Dto.File.FileName);
                var fileName = $"academic-schedule_{request.Dto.DepartmentId}_{request.Dto.SemesterId}_{Guid.NewGuid()}{fileExtension}";
                fileKey = $"academic-schedules/{fileName}";

                // Upload to R2
                await _r2StorageService.UploadAsync(request.Dto.File, fileKey, cancellationToken);
            }

            // ─── 6. Generate signed URL (if file uploaded) ──────────────────
            string fileUrl = string.Empty;
            if (!string.IsNullOrEmpty(fileKey))
            {
                // Generate a signed URL that expires in 7 days
                var expiry = TimeSpan.FromDays(7);
                fileUrl = await _r2StorageService.GetSignedUrlAsync(fileKey, expiry, cancellationToken);
            }

            // ─── 7. Create Academic Schedule ──────────────────────────────────
            var schedule = new AcademicSchedule
            {
                Title = request.Dto.Title,
                Description = request.Dto.Description,
                Url = fileUrl, // Store the signed URL
                DepartmentId = request.Dto.DepartmentId,
                SemesterId = request.Dto.SemesterId,
                StudyYearId = request.Dto.StudyYearId,
                AdminId = request.AdminId,
                ScheduleDate = request.Dto.ScheduleDate,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.AcademicSchedules.AddAsync(schedule);
            await _unitOfWork.SaveChangesAsync();

            // ─── 8. Get full details ──────────────────────────────────────────
            var created = await _unitOfWork.AcademicSchedules
                .GetByDepartmentAndSemesterWithDetailsAsync(
                    request.Dto.DepartmentId,
                    request.Dto.SemesterId);

            var result = created.FirstOrDefault(a => a.Title == request.Dto.Title);

            return _mapper.Map<AcademicScheduleDto>(result);
        }
    }
}
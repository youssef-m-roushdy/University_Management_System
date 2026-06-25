using AutoMapper;
using University_Management_System.Application.Dtos.StudentStudyYearDtos;
using University_Management_System.Application.Queries.StudentStudyYears;
using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Identity;
using University_Management_System.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.StudentStudyYears
{
    public class GetStudentStudyYearTimelineQueryHandler : IRequestHandler<GetStudentStudyYearTimelineQuery, StudentStudyYearTimelineDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetStudentStudyYearTimelineQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<StudentStudyYearTimelineDto> Handle(GetStudentStudyYearTimelineQuery request, CancellationToken cancellationToken)
        {
            // ─── 1. Get Student ──────────────────────────────────────────────
            var student = await _unitOfWork.Students
                .GetByIdAsync(request.StudentId);
            
            if (student == null)
                throw new NotFoundException($"Student with ID '{request.StudentId}' not found.");

            // ─── 2. Get all study years related to Student ──────────────────
            var studentStudyYears = await _unitOfWork.StudentStudyYears
                .GetStudyYearsByStudentIdAsync(request.StudentId);

            if (!studentStudyYears.Any())
                throw new NotFoundException($"No study years found for student with ID '{request.StudentId}'.");

            // ─── 3. Get Department ──────────────────────────────────────────────
            var department = await _unitOfWork.Departments
                .GetByIdAsync(student.DepartmentId);
            
            if (department == null)
                throw new NotFoundException($"Department with ID '{student.DepartmentId}' not found.");

            // ─── 4. Calculate completed years (non-current) ──────────────────
            var completedYears = studentStudyYears
                .Where(sy => !sy.StudyYear.IsCurrent)
                .ToList();

            // ─── 5. Build Timeline ────────────────────────────────────────────
            var timeline = new StudentStudyYearTimelineDto
            {
                StudentId = request.StudentId,
                CurrentLevel = student.Level,
                TotalYearsCompleted = completedYears.Count,
                IsGraduated = student.Level == Levels.Graduate,
                Department = department.Name,
                StudyYears = studentStudyYears
                    .Select(sy => new StudentStudyYearDetailsDto
                    {
                        StudentStudyYearId = sy.Id,
                        StartYear = sy.StudyYear.StartYear,
                        EndYear = sy.StudyYear.EndYear,
                        Level = sy.Level,
                        IsCurrent = sy.StudyYear.IsCurrent,
                        EnrolledAt = sy.EnrolledAt
                    })
                    .OrderByDescending(sy => sy.StartYear)
                    .ToList()
            };

            return timeline;
        }
    }
}
using AutoMapper;
using University_Management_System.Application.Dtos.StudentStudyYearDtos;
using University_Management_System.Application.Queries.StudentStudyYears;
using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Identity;
using University_Management_System.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using University_Management_System.Shared.Responses;

namespace University_Management_System.Application.Handlers.StudentStudyYears
{
    public class GetStudentStudyYearTimelineQueryHandler : IRequestHandler<GetStudentStudyYearTimelineQuery, ApiResponse<StudentStudyYearTimelineDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<Student> _userManager;
        private readonly IMapper _mapper;

        public GetStudentStudyYearTimelineQueryHandler(IUnitOfWork unitOfWork, UserManager<Student> userManager, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<ApiResponse<StudentStudyYearTimelineDto>> Handle(GetStudentStudyYearTimelineQuery request, CancellationToken cancellationToken)
        {
            //get Student
            var Student = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == request.StudentId);
            if(Student == null)
                return ApiResponse<StudentStudyYearTimelineDto>.ErrorResponse("Student not found");
            //get all study years related to Student ordered by start year
            var StudentStudyYears = await _unitOfWork.StudentStudyYears.GetStudyYearsByStudentIdAsync(request.StudentId);

            if (!StudentStudyYears.Any())
                return ApiResponse<StudentStudyYearTimelineDto>.ErrorResponse("No study year records found for this Student.");
            
            //then get department related to his study years
            if (Student.DepartmentId == null)
                return ApiResponse<StudentStudyYearTimelineDto>.ErrorResponse("Department not found for the Student.");

            var department = await _unitOfWork.Departments.GetByIdAsync(Student.DepartmentId);
            if(department == null)
                return ApiResponse<StudentStudyYearTimelineDto>.ErrorResponse("Department not found for the Student.");


            // get total completed years (non current)
            var completedYears = StudentStudyYears.Where(sy => !sy.StudyYear.IsCurrent).ToList();
            var timeline = new StudentStudyYearTimelineDto
            {
                StudentId = request.StudentId,
                CurrentLevel = Student.Level,
                TotalYearsCompleted = completedYears.Count,
                IsGraduated = Student.Level == Levels.Graduate,
                Department = department.Name,
                StudyYears =  StudentStudyYears.Select(sy => new StudentStudyYearDetailsDto
                {
                    StudentStudyYearId = sy.Id,
                    StartYear = sy.StudyYear.StartYear,
                    EndYear = sy.StudyYear.EndYear,
                    Level = sy.Level,
                    IsCurrent = sy.StudyYear.IsCurrent,
                    EnrolledAt = sy.EnrolledAt
                }).OrderByDescending(sy => sy.StartYear).ToList()
            };

            return ApiResponse<StudentStudyYearTimelineDto>.SuccessResponse(timeline);
        }
    }
}

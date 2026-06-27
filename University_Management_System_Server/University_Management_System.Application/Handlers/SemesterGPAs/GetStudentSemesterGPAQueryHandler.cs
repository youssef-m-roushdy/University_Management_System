using AutoMapper;
using MediatR;
using University_Management_System.Application.Dtos.SemesterGPADtos;
using University_Management_System.Application.Queries.SemesterGPAs;
using University_Management_System.Domain.Contracts;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.SemesterGPAs
{
    public class GetStudentSemesterGPAQueryHandler : IRequestHandler<GetStudentSemesterGPAQuery, SemesterGPADto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetStudentSemesterGPAQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SemesterGPADto> Handle(GetStudentSemesterGPAQuery request, CancellationToken cancellationToken)
        {
            var student = await _unitOfWork.Students.GetByIdAsync(request.UserId);
            if (student == null)
                throw new NotFoundException("Student not found");

            var semesterGPA = await _unitOfWork.SemesterGPAs.GetByIdAsync(request.SemesterGPAId);
            if (semesterGPA == null)
                throw new NotFoundException($"Semester GPA with ID '{request.SemesterGPAId}' not found");

            if (semesterGPA.StudentId != student.Id)
                throw new ForbiddenException("You can only view your own semester GPA");

            var dto = _mapper.Map<SemesterGPADto>(semesterGPA);
            
            dto.StudentName = semesterGPA.Student?.User?.Name ?? string.Empty;
            dto.AcademicCode = semesterGPA.Student?.AcademicCode ?? string.Empty;
            dto.StudyYearRange = semesterGPA.StudyYear != null 
                ? $"{semesterGPA.StudyYear.StartYear}-{semesterGPA.StudyYear.EndYear}" 
                : string.Empty;
            dto.DepartmentName = semesterGPA.Student?.Department?.Name ?? string.Empty;
            dto.DepartmentCode = semesterGPA.Student?.Department?.Code ?? string.Empty;

            return dto;
        }
    }
}
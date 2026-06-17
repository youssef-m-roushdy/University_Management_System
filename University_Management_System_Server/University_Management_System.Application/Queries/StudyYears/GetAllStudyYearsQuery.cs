using University_Management_System.Application.Dtos.StudyYearDtos;
using MediatR;

namespace University_Management_System.Application.Queries.StudyYears;

public record GetAllStudyYearsQuery : IRequest<IEnumerable<StudyYearDto>>;

using AutoMapper;
using MediatR;
using University_Management_System.Application.Dtos.StudyYearDtos;
using University_Management_System.Application.Queries.StudyYear;
using University_Management_System.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using University_Management_System.Domain.Queries;
using University_Management_System.Application.Queries.StudyYears;

namespace University_Management_System.Application.Handlers.StudyYears
{
    public class GetStudyYearStudentsQueryHandler : IRequestHandler<GetStudyYearStudentsQuery, (IEnumerable<StudentStudyYearDto> Data, int TotalCount)>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetStudyYearStudentsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<StudentStudyYearDto> Data, int TotalCount)> Handle(GetStudyYearStudentsQuery request, CancellationToken cancellationToken)
        {
            var (studentStudyYears, totalCount) = await _unitOfWork.StudentStudyYears
        .GetStudentsOfTheStudyYearByIdAsync(request.StudyYearId, request.Query, cancellationToken);

            var dtos = _mapper.Map<IEnumerable<StudentStudyYearDto>>(studentStudyYears);

            return (dtos, totalCount);
        }
    }
}
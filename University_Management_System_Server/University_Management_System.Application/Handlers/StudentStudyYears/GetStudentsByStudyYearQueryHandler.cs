using AutoMapper;
using MediatR;
using University_Management_System.Application.Dtos.StudentStudyYearDtos;
using University_Management_System.Application.Queries.StudentStudyYears;
using University_Management_System.Domain.Contracts;

namespace University_Management_System.Application.Handlers.StudentStudyYears
{
    public class GetStudentsByStudyYearQueryHandler : IRequestHandler<GetStudentsByStudyYearQuery, (IEnumerable<StudentStudyYearDto> Data, int TotalCount)>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetStudentsByStudyYearQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<StudentStudyYearDto> Data, int TotalCount)> Handle(
            GetStudentsByStudyYearQuery request, 
            CancellationToken cancellationToken)
        {
            var (studentStudyYears, totalCount) = await _unitOfWork.StudentStudyYears
                .GetStudyYearStudentsByStudyYearIdAsync(
                    request.StudyYearId,
                    request.Query,
                    cancellationToken);

            var dtos = _mapper.Map<IEnumerable<StudentStudyYearDto>>(studentStudyYears);

            return (dtos, totalCount);
        }
    }
}
using AutoMapper;
using MediatR;
using University_Management_System.Application.Dtos.StudentStudyYearDtos;
using University_Management_System.Application.Queries.StudentStudyYears;
using University_Management_System.Domain.Contracts;

namespace University_Management_System.Application.Handlers.StudentStudyYears
{
    public class GetStudentStudyYearsQueryHandler : IRequestHandler<GetStudentStudyYearsQuery, IEnumerable<StudentStudyYearDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetStudentStudyYearsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<StudentStudyYearDto>> Handle(
            GetStudentStudyYearsQuery request, 
            CancellationToken cancellationToken)
        {
            var studentStudyYears = await _unitOfWork.StudentStudyYears
                .GetByStudentIdAsync(request.StudentId);

            return _mapper.Map<IEnumerable<StudentStudyYearDto>>(studentStudyYears);
        }
    }
}
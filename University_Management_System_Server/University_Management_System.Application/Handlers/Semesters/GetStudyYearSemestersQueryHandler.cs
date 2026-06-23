using AutoMapper;
using MediatR;
using University_Management_System.Application.Dtos.StudyYearDtos;
using University_Management_System.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using University_Management_System.Domain.Queries;
using University_Management_System.Application.Dtos.SemesterDtos;
using University_Management_System.Application.Queries.Semesters;

namespace University_Management_System.Application.Handlers.Semesters
{
    public class GetStudyYearSemestersQueryHandler : IRequestHandler<GetStudyYearSemestersQuery, (IEnumerable<SemesterDto> Data, int TotalCount)>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetStudyYearSemestersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<SemesterDto> Data, int TotalCount)> Handle(GetStudyYearSemestersQuery request, CancellationToken cancellationToken)
        {
            var semesters = await _unitOfWork.Semesters.GetByStudyYearIdAsync(request.StudyYearId);
            var totalCount = semesters.Count();

            var dtos = _mapper.Map<IEnumerable<SemesterDto>>(semesters);

            return (dtos, totalCount);
        }
    }
}
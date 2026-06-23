using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using University_Management_System.Application.Dtos.SemesterDtos;
using University_Management_System.Application.Queries.Semesters;
using University_Management_System.Domain.Contracts;

namespace University_Management_System.Application.Handlers.Semesters
{
    public class GetSemestersQueryHandler : IRequestHandler<GetSemestersQuery, (IEnumerable<SemesterDto> Data, int TotalCount)>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetSemestersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<SemesterDto> Data, int TotalCount)> Handle(
            GetSemestersQuery request,
            CancellationToken cancellationToken)
        {
            var (semesters, totalCount) = await _unitOfWork.Semesters
                .GetAllFilteredAsync(request.Query);

            var semesterDtos = _mapper.Map<IEnumerable<SemesterDto>>(semesters);

            return (semesterDtos, totalCount);
        }
    }
}
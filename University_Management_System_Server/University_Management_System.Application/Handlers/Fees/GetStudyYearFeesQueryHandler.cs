using AutoMapper;
using MediatR;
using University_Management_System.Application.Dtos.StudyYearDtos;
using University_Management_System.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using University_Management_System.Application.Dtos.DepartmentDtos.FeeDtos;
using University_Management_System.Application.Queries.StudyYears;
using University_Management_System.Domain.Queries;
using University_Management_System.Application.Queries.Fees;

namespace University_Management_System.Application.Handlers.Fees
{
    public class GetStudyYearFeesQueryHandler : IRequestHandler<GetStudyYearFeesQuery, (IEnumerable<FeeDto> Data, int TotalCount)>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetStudyYearFeesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<FeeDto> Data, int TotalCount)> Handle(GetStudyYearFeesQuery request, CancellationToken cancellationToken)
        {
            var (fees, totalCount) = await _unitOfWork.Fees
                .GetByStudyYearIdAsync(request.StudyYearId, request.Query, cancellationToken);

            var dtos = _mapper.Map<IEnumerable<FeeDto>>(fees);

            return (dtos, totalCount);
        }
    }
}
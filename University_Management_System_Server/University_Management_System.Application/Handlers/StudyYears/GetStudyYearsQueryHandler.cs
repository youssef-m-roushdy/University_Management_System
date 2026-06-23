using AutoMapper;
using MediatR;
using University_Management_System.Application.Dtos.StudyYearDtos;
using University_Management_System.Application.Queries.StudyYears;
using University_Management_System.Domain.Contracts;
using Microsoft.EntityFrameworkCore;

namespace University_Management_System.Application.Handlers.StudyYears
{
    public class GetStudyYearsQueryHandler : IRequestHandler<GetStudyYearsQuery, (IEnumerable<StudyYearDto> Data, int TotalCount)>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetStudyYearsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<StudyYearDto> Data, int TotalCount)> Handle(GetStudyYearsQuery request, CancellationToken cancellationToken)
        {
            // Get filtered study years from repository
            var (data, totalCount) = await _unitOfWork.StudyYears
                .GetAllFilteredAsync(request.Query);


            // Map to DTOs
            var dtos = _mapper.Map<IEnumerable<StudyYearDto>>(data);

            return (dtos, totalCount);
        }
    }
}
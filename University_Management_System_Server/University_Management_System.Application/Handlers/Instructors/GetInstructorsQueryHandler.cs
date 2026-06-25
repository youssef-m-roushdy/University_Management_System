using AutoMapper;
using MediatR;
using University_Management_System.Application.Dtos.InstructorDtos;
using University_Management_System.Application.Queries.Instructors;
using University_Management_System.Domain.Contracts;

namespace University_Management_System.Application.Handlers.Instructors
{
    public class GetInstructorsQueryHandler : IRequestHandler<GetInstructorsQuery, (IEnumerable<InstructorDto> Data, int TotalCount)>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetInstructorsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<InstructorDto> Data, int TotalCount)> Handle(
            GetInstructorsQuery request,
            CancellationToken cancellationToken)
        {
            var (instructors, totalCount) = await _unitOfWork.Instructors
                .GetAllFilteredAsync(request.Query, cancellationToken);

            var instructorDtos = _mapper.Map<IEnumerable<InstructorDto>>(instructors);

            return (instructorDtos, totalCount);
        }
    }
}
using AutoMapper;
using MediatR;
using University_Management_System.Application.Dtos.StudentDtos;
using University_Management_System.Application.Queries.Students;
using University_Management_System.Domain.Contracts;

namespace University_Management_System.Application.Handlers.Students
{
    public class GetStudentsQueryHandler : IRequestHandler<GetStudentsQuery, (IEnumerable<StudentDto> Data, int TotalCount)>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetStudentsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<StudentDto> Data, int TotalCount)> Handle(
            GetStudentsQuery request, 
            CancellationToken cancellationToken)
        {
            var (students, totalCount) = await _unitOfWork.Students
                .GetAllFilteredAsync(request.Query, cancellationToken);

            var studentDtos = _mapper.Map<IEnumerable<StudentDto>>(students);

            return (studentDtos, totalCount);
        }
    }
}
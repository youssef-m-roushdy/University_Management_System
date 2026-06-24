using AutoMapper;
using MediatR;
using University_Management_System.Application.Dtos.StudentDtos;
using University_Management_System.Application.Queries.Students;
using University_Management_System.Domain.Contracts;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.Students
{
    public class GetStudentQueryHandler : IRequestHandler<GetStudentQuery, StudentDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetStudentQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<StudentDto?> Handle(GetStudentQuery request, CancellationToken cancellationToken)
        {
            var student = await _unitOfWork.Students
                .GetByIdAsync(request.Id);

            if (student == null)
                return null;

            return _mapper.Map<StudentDto>(student);
        }
    }
}
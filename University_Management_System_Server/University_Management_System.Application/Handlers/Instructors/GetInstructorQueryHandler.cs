using AutoMapper;
using MediatR;
using University_Management_System.Application.Dtos.InstructorDtos;
using University_Management_System.Application.Queries.Instructors;
using University_Management_System.Domain.Contracts;

namespace University_Management_System.Application.Handlers.Instructors
{
    public class GetInstructorQueryHandler : IRequestHandler<GetInstructorQuery, InstructorDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetInstructorQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<InstructorDto?> Handle(GetInstructorQuery request, CancellationToken cancellationToken)
        {
            var instructor = await _unitOfWork.Instructors
                .GetInstructorByUserIdAsync(request.Id);
            
            if (instructor == null)
                return null;

            return _mapper.Map<InstructorDto>(instructor);
        }
    }
}
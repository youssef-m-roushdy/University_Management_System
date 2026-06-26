using AutoMapper;
using MediatR;
using University_Management_System.Application.Dtos.RegistrationDtos;
using University_Management_System.Application.Queries.Registrations;
using University_Management_System.Domain.Contracts;

namespace University_Management_System.Application.Handlers.Registrations
{
    public class GetRegisteredCoursesQueryHandler : IRequestHandler<GetRegisteredCoursesQuery, IEnumerable<RegistrationDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetRegisteredCoursesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RegistrationDto>> Handle(
            GetRegisteredCoursesQuery request,
            CancellationToken cancellationToken)
        {
            var (registrations, _) = await _unitOfWork.Registrations
                .GetByStudentIdAsync(request.StudentId, null, cancellationToken);

            return _mapper.Map<IEnumerable<RegistrationDto>>(registrations);
        }
    }
}
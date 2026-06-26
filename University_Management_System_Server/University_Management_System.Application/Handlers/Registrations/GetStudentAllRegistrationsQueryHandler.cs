using AutoMapper;
using MediatR;
using University_Management_System.Application.Dtos.RegistrationDtos;
using University_Management_System.Application.Queries.Registrations;
using University_Management_System.Domain.Contracts;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.Registrations
{
    public class GetStudentAllRegistrationsQueryHandler : IRequestHandler<GetStudentAllRegistrationsQuery, (IEnumerable<RegistrationDto> Data, int TotalCount)>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetStudentAllRegistrationsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<RegistrationDto> Data, int TotalCount)> Handle(
            GetStudentAllRegistrationsQuery request,
            CancellationToken cancellationToken)
        {
            // ─── Validate Student exists ──────────────────────────────────
            var student = await _unitOfWork.Students.GetByIdAsync(request.StudentId);
            if (student == null)
                throw new NotFoundException($"Student with ID '{request.StudentId}' not found.");

            // ─── Get all registrations with filters ──────────────────────────
            var (registrations, totalCount) = await _unitOfWork.Registrations
                .GetByStudentIdAsync(request.StudentId, request.Filter, cancellationToken);

            return (_mapper.Map<IEnumerable<RegistrationDto>>(registrations), totalCount);
        }
    }
}
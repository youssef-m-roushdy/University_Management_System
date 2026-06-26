using AutoMapper;
using MediatR;
using University_Management_System.Application.Dtos.RegistrationDtos;
using University_Management_System.Application.Queries.Registrations;
using University_Management_System.Domain.Contracts;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.Registrations
{
    public class GetStudyYearRegistrationsQueryHandler : IRequestHandler<GetStudyYearRegistrationsQuery, (IEnumerable<RegistrationDto> Data, int TotalCount)>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetStudyYearRegistrationsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<RegistrationDto> Data, int TotalCount)> Handle(
            GetStudyYearRegistrationsQuery request,
            CancellationToken cancellationToken)
        {
            // ─── Validate Study Year exists ──────────────────────────────────
            var studyYear = await _unitOfWork.StudyYears.GetByIdAsync(request.StudyYearId);
            if (studyYear == null)
                throw new NotFoundException($"Study Year with ID '{request.StudyYearId}' not found.");

            // ─── Get registrations ──────────────────────────────────────────
            var (registrations, totalCount) = await _unitOfWork.Registrations
                .GetByStudyYearIdAsync(request.StudyYearId, request.Query, cancellationToken);

            return (_mapper.Map<IEnumerable<RegistrationDto>>(registrations), totalCount);
        }
    }
}
using AutoMapper;
using MediatR;
using University_Management_System.Application.Dtos.SpecializationCourseDtos;
using University_Management_System.Application.Queries.SpecializationCourses;
using University_Management_System.Domain.Contracts;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.SpecializationCourses
{
    public class GetSpecializationCoursesBySpecializationQueryHandler : IRequestHandler<GetSpecializationCoursesBySpecializationQuery, (IEnumerable<SpecializationCourseDto> Data, int TotalCount)>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetSpecializationCoursesBySpecializationQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<SpecializationCourseDto> Data, int TotalCount)> Handle(
            GetSpecializationCoursesBySpecializationQuery request,
            CancellationToken cancellationToken)
        {
            // ─── Validate Specialization exists ──────────────────────────────────
            var specialization = await _unitOfWork.Specializations.GetByIdAsync(request.SpecializationId);
            if (specialization == null)
                throw new NotFoundException($"Specialization with ID '{request.SpecializationId}' not found.");

            // ─── Get specialization courses ──────────────────────────────────────
            var (specializationCourses, totalCount) = await _unitOfWork.SpecializationCourses
                .GetBySpecializationIdAsync(
                    request.SpecializationId,
                    request.Query,
                    cancellationToken);

            var dtos = _mapper.Map<IEnumerable<SpecializationCourseDto>>(specializationCourses);

            return (dtos, totalCount);
        }
    }
}
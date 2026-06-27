using AutoMapper;
using MediatR;
using University_Management_System.Application.Dtos.AcademicScheduleDtos;
using University_Management_System.Application.Queries.AcademicSchedules;
using University_Management_System.Domain.Contracts;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.AcademicSchedules
{
    public class GetAcademicScheduleQueryHandler : IRequestHandler<GetAcademicScheduleQuery, AcademicScheduleDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAcademicScheduleQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AcademicScheduleDto> Handle(
            GetAcademicScheduleQuery request,
            CancellationToken cancellationToken)
        {
            // ─── 1. Get schedule with details ──────────────────────────────────
            var schedule = await _unitOfWork.AcademicSchedules
                .GetByIdWithDetailsAsync(request.Id);

            if (schedule == null)
                throw new NotFoundException($"Academic schedule with ID '{request.Id}' not found.");

            // ─── 2. Return mapped DTO ──────────────────────────────────────────
            return _mapper.Map<AcademicScheduleDto>(schedule);
        }
    }
}
using University_Management_System.Application.Dtos.UserStudyYearDtos;
using University_Management_System.Application.Queries.UserStudyYears;
using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Models;
using MediatR;
using University_Management_System.Shared.Respones;

namespace University_Management_System.Application.Handlers.UserStudyYears
{
    public class GetUserStudyYearsQueryHandler : IRequestHandler<GetUserStudyYearsQuery, Response<List<UserStudyYearDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUserStudyYearsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<List<UserStudyYearDto>>> Handle(GetUserStudyYearsQuery request, CancellationToken cancellationToken)
        {
            var records = await _unitOfWork.UserStudyYears.GetByUserIdAsync(request.UserId);

            var dtos = records.Select(MapToDto).ToList();

            return Response<List<UserStudyYearDto>>.SuccessResponse(dtos);
        }

        private static UserStudyYearDto MapToDto(UserStudyYear entity)
        {
            return new UserStudyYearDto
            {
                Id = entity.Id,
                UserId = entity.UserId,
                StudyYearId = entity.StudyYearId,
                StartYear = entity.StudyYear?.StartYear ?? 0,
                EndYear = entity.StudyYear?.EndYear ?? 0,
                Level = entity.Level,
                LevelName = entity.Level.ToString().Replace("_", " "),
                IsCurrent = entity.StudyYear?.IsCurrent ?? false,
                EnrolledAt = entity.EnrolledAt
            };
        }
    }
}

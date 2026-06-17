using University_Management_System.Application.Dtos.UserStudyYearDtos;
using MediatR;
using University_Management_System.Shared.Respones;

namespace University_Management_System.Application.Queries.UserStudyYears
{
    public class GetUserStudyYearTimelineQuery : IRequest<Response<UserStudyYearTimelineDto>>
    {
        public string UserId { get; set; } = string.Empty;

        public GetUserStudyYearTimelineQuery(string userId)
        {
            UserId = userId;
        }
    }
}

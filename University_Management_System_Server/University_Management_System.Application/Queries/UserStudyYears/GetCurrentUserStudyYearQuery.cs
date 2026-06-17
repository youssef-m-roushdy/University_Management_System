using University_Management_System.Application.Dtos.UserStudyYearDtos;
using MediatR;
using University_Management_System.Shared.Respones;

namespace University_Management_System.Application.Queries.UserStudyYears
{
    public class GetCurrentUserStudyYearQuery : IRequest<Response<UserStudyYearDto>>
    {
        public string UserId { get; set; } = string.Empty;

        public GetCurrentUserStudyYearQuery(string userId)
        {
            UserId = userId;
        }
    }
}

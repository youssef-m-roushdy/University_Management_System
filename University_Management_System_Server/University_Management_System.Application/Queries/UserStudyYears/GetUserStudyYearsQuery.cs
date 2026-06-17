using University_Management_System.Application.Dtos.UserStudyYearDtos;
using MediatR;
using University_Management_System.Shared.Respones;

namespace University_Management_System.Application.Queries.UserStudyYears
{
    public class GetUserStudyYearsQuery : IRequest<Response<List<UserStudyYearDto>>>
    {
        public string UserId { get; set; } = string.Empty;

        public GetUserStudyYearsQuery(string userId)
        {
            UserId = userId;
        }
    }
}

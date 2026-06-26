using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using University_Management_System.Application.Dtos.CourseDtos;
using University_Management_System.Application.Queries.Courses;
using University_Management_System.Domain.Contracts;

namespace University_Management_System.Application.Handlers.Courses
{
    public class SearchCoursesQueryHandler : IRequestHandler<SearchCoursesQuery, IEnumerable<CourseSearchResultDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SearchCoursesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CourseSearchResultDto>> Handle(
            SearchCoursesQuery request,
            CancellationToken cancellationToken)
        {
            var query = _unitOfWork.Courses.GetQueryable()
                .Include(c => c.Department)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm.ToLower().Trim();
                query = query.Where(c =>
                    c.Code.ToLower().Contains(searchTerm) ||
                    c.Name.ToLower().Contains(searchTerm));
            }

            if (request.DepartmentId.HasValue)
                query = query.Where(c => c.DepartmentId == request.DepartmentId.Value);

            var courses = await query
                .Take(request.MaxResults ?? 20)
                .OrderBy(c => c.Code)
                .ToListAsync(cancellationToken);

            return _mapper.Map<IEnumerable<CourseSearchResultDto>>(courses);
        }
    }
}
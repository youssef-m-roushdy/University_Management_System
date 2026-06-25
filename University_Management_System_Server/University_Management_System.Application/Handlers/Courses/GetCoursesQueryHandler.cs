using AutoMapper;
using MediatR;
using University_Management_System.Application.Dtos.CourseDtos;
using University_Management_System.Application.Queries.Courses;
using University_Management_System.Domain.Contracts;

namespace University_Management_System.Application.Handlers.Courses
{
    public class GetCoursesQueryHandler : IRequestHandler<GetCoursesQuery, (IEnumerable<CourseDto> Data, int TotalCount)>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetCoursesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<CourseDto> Data, int TotalCount)> Handle(
            GetCoursesQuery request,
            CancellationToken cancellationToken)
        {
            var (courses, totalCount) = await _unitOfWork.Courses
                .GetFilteredAsync(request.Query, cancellationToken);

            var courseDtos = _mapper.Map<IEnumerable<CourseDto>>(courses);

            // Get counts for each course
            foreach (var dto in courseDtos)
            {
                dto.PrerequisitesCount = await _unitOfWork.Courses
                    .HasPrerequisitesAsync(dto.Id) ? 
                    (await _unitOfWork.Courses.GetPrerequisitesAsync(dto.Id)).Count() : 0;
                
                dto.DependenciesCount = await _unitOfWork.Courses
                    .HasDependenciesAsync(dto.Id) ?
                    (await _unitOfWork.Courses.GetDependenciesAsync(dto.Id)).Count() : 0;
            }

            return (courseDtos, totalCount);
        }
    }
}
using AutoMapper;
using MediatR;
using University_Management_System.Application.Dtos.CourseDtos;
using University_Management_System.Application.Queries.Courses;
using University_Management_System.Domain.Contracts;

namespace University_Management_System.Application.Handlers.Courses
{
    public class GetCourseQueryHandler : IRequestHandler<GetCourseQuery, CourseDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetCourseQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CourseDto?> Handle(GetCourseQuery request, CancellationToken cancellationToken)
        {
            var course = await _unitOfWork.Courses
                .GetCourseWithDetailsAsync(request.Id);
            
            if (course == null)
                return null;

            var dto = _mapper.Map<CourseDto>(course);
            
            dto.PrerequisitesCount = course.PrerequisiteFor?.Count ?? 0;
            dto.DependenciesCount = course.DependentCourses?.Count ?? 0;

            return dto;
        }
    }
}
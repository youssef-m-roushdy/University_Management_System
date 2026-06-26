using AutoMapper;
using MediatR;
using University_Management_System.Application.Dtos.SpecializationCourseDtos;
using University_Management_System.Application.Queries.SpecializationCourses;
using University_Management_System.Domain.Contracts;

namespace University_Management_System.Application.Handlers.SpecializationCourses
{
    public class GetSpecializationCoursesQueryHandler : IRequestHandler<GetSpecializationCoursesQuery, (IEnumerable<SpecializationCourseDto> Data, int TotalCount)>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetSpecializationCoursesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<SpecializationCourseDto> Data, int TotalCount)> Handle(
            GetSpecializationCoursesQuery request,
            CancellationToken cancellationToken)
        {
            var (specializationCourses, totalCount) = await _unitOfWork.SpecializationCourses
                .GetAllFilteredAsync(request.Query, cancellationToken);

            var dtos = _mapper.Map<IEnumerable<SpecializationCourseDto>>(specializationCourses);

            return (dtos, totalCount);
        }
    }
}
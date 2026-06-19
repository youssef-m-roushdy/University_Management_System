using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using University_Management_System.Application.Dtos.CourseDtos;
using University_Management_System.Application.Queries.Courses;
using University_Management_System.Domain.Contracts;
using MediatR;
using University_Management_System.Shared.Responses;

namespace University_Management_System.Application.Handlers.Courses
{
    public class GetAllCoursesQueryHandler : IRequestHandler<GetAllCoursesQuery, PagedResponse<CourseWithDepartmentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllCoursesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<PagedResponse<CourseWithDepartmentDto>> Handle(GetAllCoursesQuery request, CancellationToken cancellationToken)
        {
            var (courses, totalCount) = await _unitOfWork.Courses.GetFilteredCoursesWithPaginationAsync(request.Query);
            var result = _mapper.Map<IEnumerable<CourseWithDepartmentDto>>(courses);
            return PagedResponse<CourseWithDepartmentDto>.SuccessResponse(result, request.Query.PageNumber, request.Query.PageSize, totalCount);
        }
    }
}
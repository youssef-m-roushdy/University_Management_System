using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using University_Management_System.Application.Dtos.CourseDtos;
using University_Management_System.Application.Queries.Courses;
using University_Management_System.Domain.Contracts;
using University_Management_System.Shared.Exceptions;
using MediatR;

namespace University_Management_System.Application.Handlers.Courses
{
    public class GetDepartmentOpenCoursesQueryHandler : IRequestHandler<GetDepartmentOpenCoursesQuery, IEnumerable<CourseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetDepartmentOpenCoursesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CourseDto>> Handle(GetDepartmentOpenCoursesQuery request, CancellationToken cancellationToken)
        {
            var department = await _unitOfWork.Departments.GetByIdAsync(request.DepartmentId);
            if(department == null)
                throw new NotFoundException("No department found");
            
            var courses = await _unitOfWork.Courses.GetOpenCoursesAsync();
            return _mapper.Map<IEnumerable<CourseDto>>(courses);
        }
    }
}
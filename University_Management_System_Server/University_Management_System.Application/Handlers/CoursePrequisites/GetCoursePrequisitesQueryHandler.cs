using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using University_Management_System.Application.Dtos.CourseDtos;
using University_Management_System.Application.Queries.CoursePrequisites;
using University_Management_System.Domain.Contracts;
using MediatR;

namespace University_Management_System.Application.Handlers.CoursePrequisites
{
    public class GetCoursePrequisitesQueryHandler : IRequestHandler<GetCoursePrequisitesQuery, List<CourseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetCoursePrequisitesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<CourseDto>> Handle(GetCoursePrequisitesQuery request, CancellationToken cancellationToken)
        {
            var prerequisites = await _unitOfWork.Courses.GetCoursePrerequisitesAsync(request.CourseId);

            var prerequisiteDtos = _mapper.Map<List<CourseDto>>(prerequisites);

            return prerequisiteDtos;
        }
    }
}
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
    public class GetCourseDependenciesQueryHandler : IRequestHandler<GetCourseDependenciesQuery, List<CourseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetCourseDependenciesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<CourseDto>> Handle(GetCourseDependenciesQuery request, CancellationToken cancellationToken)
        {
            var dependencies = await _unitOfWork.Courses.GetCourseDependenciesAsync(request.CourseId);

            var dependencyDtos = _mapper.Map<List<CourseDto>>(dependencies);

            return dependencyDtos;
        }
    }
}
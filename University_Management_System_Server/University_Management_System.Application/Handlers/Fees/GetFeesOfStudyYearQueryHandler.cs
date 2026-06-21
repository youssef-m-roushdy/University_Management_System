using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using University_Management_System.Application.Dtos.DepartmentDtos.FeeDtos;
using University_Management_System.Application.Queries.Fees;
using University_Management_System.Domain.Contracts;
using MediatR;

namespace University_Management_System.Application.Handlers.Fees
{
    public class GetFeesOfStudyYearQueryHandler : IRequestHandler<GetFeesOfStudyYearQuery, List<FeeDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetFeesOfStudyYearQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public Task<List<FeeDto>> Handle(GetFeesOfStudyYearQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
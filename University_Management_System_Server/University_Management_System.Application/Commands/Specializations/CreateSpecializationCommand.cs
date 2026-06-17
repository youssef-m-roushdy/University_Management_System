using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Application.Dtos.SpecializationDtos;
using MediatR;

namespace University_Management_System.Application.Commands.Specializations
{
    public class CreateSpecializationCommand : IRequest<SpecializationDto>
    {
        public CreateSpecializationDto CreateSpecializationDto { get; set; }

        public CreateSpecializationCommand(CreateSpecializationDto createSpecializationDto)
        {
            CreateSpecializationDto = createSpecializationDto;
        }
    }
}
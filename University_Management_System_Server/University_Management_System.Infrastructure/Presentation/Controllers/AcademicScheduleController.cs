using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using University_Management_System.Application.Commands.AcademicSchedules;
using University_Management_System.Application.Dtos.AcademicSheduleDtos;
using University_Management_System.Application.Queries.AcademicSchedules;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace University_Management_System.Infrastructure.Presentation.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AcademicScheduleController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AcademicScheduleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        
    }
}

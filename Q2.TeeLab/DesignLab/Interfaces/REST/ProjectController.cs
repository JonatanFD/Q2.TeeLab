using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Q2.TeeLab.DesignLab.Domain.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace Q2.TeeLab.DesignLab.Interfaces.REST;

[ApiController]
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Design Lab Endpoints")]
public class ProjectController(
    IProjectCommandService projectCommandService,
    IProjectQueryService projectQueryService
) : ControllerBase
{
        
    
}
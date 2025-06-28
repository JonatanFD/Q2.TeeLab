using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Q2.TeeLab.DesignLab.Domain.Services;
using Q2.TeeLab.DesignLab.Interfaces.REST.Resources;
using Q2.TeeLab.DesignLab.Interfaces.REST.Transform;
using Swashbuckle.AspNetCore.Annotations;

namespace Q2.TeeLab.DesignLab.Interfaces.REST;

[ApiController]
[Route("api/v1/projects")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Design Lab Endpoints")]
public class ProjectController(
    IProjectCommandService projectCommandService,
    IProjectQueryService projectQueryService
) : BaseApiController
{
    [HttpPost]
    [SwaggerOperation(
        Summary = "Create a new project",
        Description = "Create a new project with the specified details")]
    [SwaggerResponse(201, "Project successfully created", typeof(ApiResponse<ProjectResource>))]
    [SwaggerResponse(400, "Bad request", typeof(ApiResponse<ProjectResource>))]
    public async Task<ActionResult<ApiResponse<ProjectResource>>> CreateProject([FromBody] CreateProjectResource resource)
    {
        try
        {
            var command = CreateProjectCommandFromResourceAssembler.ToCommand(resource);
            var project = await projectCommandService.Handle(command);
            var projectResource = CreateProjectResourceFromEntityAssembler.ToResource(project);
            
            var response = ApiResponse<ProjectResource>.SuccessResponse(projectResource, "Project created successfully");
            return CreatedAtAction(nameof(GetProjectById), new { id = projectResource.Id }, response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ApiResponse<ProjectResource>.ErrorResponse("Invalid input data", new[] { ex.Message }));
        }
        catch (Exception ex)
        {
            return HandleError<ApiResponse<ProjectResource>>(ex, "An error occurred while creating the project");
        }
    }

    [HttpGet("{id:guid}")]
    [SwaggerOperation(
        Summary = "Get project by ID",
        Description = "Get a project by its unique identifier")]
    [SwaggerResponse(200, "Project found", typeof(ApiResponse<ProjectResource>))]
    [SwaggerResponse(404, "Project not found", typeof(ApiResponse<ProjectResource>))]
    public async Task<ActionResult<ApiResponse<ProjectResource>>> GetProjectById(Guid id)
    {
        try
        {
            var query = new Domain.Model.Queries.GetProjectByIdQuery(new Domain.Model.ValueObjects.ProjectId(id));
            var project = await projectQueryService.Handle(query);
            
            if (project == null)
            {
                return NotFound(ApiResponse<ProjectResource>.ErrorResponse($"Project with id {id} not found"));
            }
            
            var resource = CreateProjectResourceFromEntityAssembler.ToResource(project);
            return Ok(ApiResponse<ProjectResource>.SuccessResponse(resource));
        }
        catch (Exception ex)
        {
            return HandleError<ApiResponse<ProjectResource>>(ex, "An error occurred while retrieving the project");
        }
    }

    [HttpGet("user/{userId:guid}")]
    [SwaggerOperation(
        Summary = "Get projects by user ID",
        Description = "Get all projects for a specific user")]
    [SwaggerResponse(200, "Projects found", typeof(ApiResponse<IEnumerable<ProjectResource>>))]
    public async Task<ActionResult<ApiResponse<IEnumerable<ProjectResource>>>> GetProjectsByUserId(Guid userId)
    {
        try
        {
            var query = new Domain.Model.Queries.GetProjectsByUserIdQuery(new Domain.Model.ValueObjects.UserId(userId));
            var projects = await projectQueryService.Handle(query);
            var resources = projects.Select(CreateProjectResourceFromEntityAssembler.ToResource);
            
            return Ok(ApiResponse<IEnumerable<ProjectResource>>.SuccessResponse(resources));
        }
        catch (Exception ex)
        {
            return HandleError<ApiResponse<IEnumerable<ProjectResource>>>(ex, "An error occurred while retrieving the projects");
        }
    }

    [HttpDelete("{id:guid}")]
    [SwaggerOperation(
        Summary = "Delete project",
        Description = "Delete a project by its unique identifier")]
    [SwaggerResponse(204, "Project successfully deleted")]
    [SwaggerResponse(404, "Project not found", typeof(ApiResponse<bool>))]
    public async Task<IActionResult> DeleteProject(Guid id)
    {
        try
        {
            var deleted = await projectCommandService.DeleteAsync(new Domain.Model.ValueObjects.ProjectId(id));
            if (!deleted)
            {
                return NotFound(ApiResponse<bool>.ErrorResponse($"Project with id {id} not found"));
            }
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<bool>.ErrorResponse("An error occurred while deleting the project", new[] { ex.Message }));
        }
    }
}
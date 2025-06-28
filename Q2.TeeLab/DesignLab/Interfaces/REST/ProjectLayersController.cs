using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Q2.TeeLab.DesignLab.Domain.Services;
using Q2.TeeLab.DesignLab.Interfaces.REST.Resources;
using Q2.TeeLab.DesignLab.Interfaces.REST.Transform;
using Swashbuckle.AspNetCore.Annotations;

namespace Q2.TeeLab.DesignLab.Interfaces.REST;

[ApiController]
[Route("api/v1/projects/{projectId:guid}/layers")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Project Layers Management")]
public class ProjectLayersController(
    IProjectCommandService projectCommandService,
    ILayerQueryService layerQueryService
) : BaseApiController
{
    [HttpPost("text")]
    [SwaggerOperation(
        Summary = "Add text layer to project",
        Description = "Add a new text layer to the specified project")]
    [SwaggerResponse(201, "Text layer successfully added", typeof(ApiResponse<LayerResource>))]
    [SwaggerResponse(400, "Bad request", typeof(ApiResponse<LayerResource>))]
    [SwaggerResponse(404, "Project not found", typeof(ApiResponse<LayerResource>))]
    public async Task<ActionResult<ApiResponse<LayerResource>>> AddTextLayer(Guid projectId, [FromBody] AddTextLayerResource resource)
    {
        try
        {
            var command = AddTextLayerCommandFromResourceAssembler.ToCommand(projectId, resource);
            var layerId = await projectCommandService.Handle(command);
            
            var layer = await layerQueryService.Handle(new Domain.Model.Queries.GetLayerByIdQuery(layerId));
            if (layer == null)
            {
                return StatusCode(500, ApiResponse<LayerResource>.ErrorResponse("Layer was created but could not be retrieved"));
            }
            
            var layerResource = LayerResourceFromEntityAssembler.ToResource(layer);
            var response = ApiResponse<LayerResource>.SuccessResponse(layerResource, "Text layer added successfully");
            return CreatedAtAction(nameof(GetLayerById), new { projectId, layerId = layerResource.Id }, response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ApiResponse<LayerResource>.ErrorResponse("Invalid input data", new[] { ex.Message }));
        }
        catch (Exception ex)
        {
            return HandleError<ApiResponse<LayerResource>>(ex, "An error occurred while adding the text layer");
        }
    }

    [HttpPost("image")]
    [SwaggerOperation(
        Summary = "Add image layer to project",
        Description = "Add a new image layer to the specified project")]
    [SwaggerResponse(201, "Image layer successfully added", typeof(ApiResponse<LayerResource>))]
    [SwaggerResponse(400, "Bad request", typeof(ApiResponse<LayerResource>))]
    [SwaggerResponse(404, "Project not found", typeof(ApiResponse<LayerResource>))]
    public async Task<ActionResult<ApiResponse<LayerResource>>> AddImageLayer(Guid projectId, [FromBody] AddImageLayerResource resource)
    {
        try
        {
            var command = AddImageLayerCommandFromResourceAssembler.ToCommand(projectId, resource);
            var layerId = await projectCommandService.Handle(command);
            
            var layer = await layerQueryService.Handle(new Domain.Model.Queries.GetLayerByIdQuery(layerId));
            if (layer == null)
            {
                return StatusCode(500, ApiResponse<LayerResource>.ErrorResponse("Layer was created but could not be retrieved"));
            }
            
            var layerResource = LayerResourceFromEntityAssembler.ToResource(layer);
            var response = ApiResponse<LayerResource>.SuccessResponse(layerResource, "Image layer added successfully");
            return CreatedAtAction(nameof(GetLayerById), new { projectId, layerId = layerResource.Id }, response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ApiResponse<LayerResource>.ErrorResponse("Invalid input data", new[] { ex.Message }));
        }
        catch (Exception ex)
        {
            return HandleError<ApiResponse<LayerResource>>(ex, "An error occurred while adding the image layer");
        }
    }

    [HttpGet("{layerId:guid}")]
    [SwaggerOperation(
        Summary = "Get layer by ID",
        Description = "Get a specific layer from a project")]
    [SwaggerResponse(200, "Layer found", typeof(ApiResponse<LayerResource>))]
    [SwaggerResponse(404, "Layer not found", typeof(ApiResponse<LayerResource>))]
    public async Task<ActionResult<ApiResponse<LayerResource>>> GetLayerById(Guid projectId, Guid layerId)
    {
        try
        {
            var query = new Domain.Model.Queries.GetLayerByIdQuery(new Domain.Model.ValueObjects.LayerId(layerId));
            var layer = await layerQueryService.Handle(query);
            
            if (layer == null)
            {
                return NotFound(ApiResponse<LayerResource>.ErrorResponse($"Layer with id {layerId} not found"));
            }
            
            var resource = LayerResourceFromEntityAssembler.ToResource(layer);
            return Ok(ApiResponse<LayerResource>.SuccessResponse(resource));
        }
        catch (Exception ex)
        {
            return HandleError<ApiResponse<LayerResource>>(ex, "An error occurred while retrieving the layer");
        }
    }

    [HttpDelete("{layerId:guid}")]
    [SwaggerOperation(
        Summary = "Remove layer from project",
        Description = "Remove a specific layer from the project")]
    [SwaggerResponse(204, "Layer successfully removed")]
    [SwaggerResponse(404, "Layer not found", typeof(ApiResponse<bool>))]
    public async Task<IActionResult> RemoveLayer(Guid projectId, Guid layerId)
    {
        try
        {
            var command = new Domain.Model.Commands.RemoveLayerFromProjectCommand(
                new Domain.Model.ValueObjects.ProjectId(projectId),
                new Domain.Model.ValueObjects.LayerId(layerId)
            );
            var removed = await projectCommandService.Handle(command);
            if (!removed)
            {
                return NotFound(ApiResponse<bool>.ErrorResponse($"Layer with id {layerId} not found"));
            }
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<bool>.ErrorResponse("An error occurred while removing the layer", new[] { ex.Message }));
        }
    }

    [HttpPut("{layerId:guid}/send-to-back")]
    [SwaggerOperation(
        Summary = "Send layer to back",
        Description = "Send the specified layer to the back of the layer stack")]
    [SwaggerResponse(200, "Layer successfully moved to back", typeof(ApiResponse<bool>))]
    [SwaggerResponse(404, "Layer not found", typeof(ApiResponse<bool>))]
    public async Task<ActionResult<ApiResponse<bool>>> SendLayerToBack(Guid projectId, Guid layerId)
    {
        try
        {
            var command = new Domain.Model.Commands.SendLayerToBackInProjectCommand(
                new Domain.Model.ValueObjects.ProjectId(projectId),
                new Domain.Model.ValueObjects.LayerId(layerId)
            );
            var moved = await projectCommandService.Handle(command);
            if (!moved)
            {
                return NotFound(ApiResponse<bool>.ErrorResponse($"Layer with id {layerId} not found"));
            }
            return Ok(ApiResponse<bool>.SuccessResponse(true, "Layer moved to back successfully"));
        }
        catch (Exception ex)
        {
            return HandleError<ApiResponse<bool>>(ex, "An error occurred while moving the layer");
        }
    }
}
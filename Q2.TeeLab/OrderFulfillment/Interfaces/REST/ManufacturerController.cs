using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Q2.TeeLab.OrderFulfillment.Application.Internal.CommandServices;
using Q2.TeeLab.OrderFulfillment.Application.Internal.QueryServices;
using Q2.TeeLab.OrderFulfillment.Domain.Model.Queries;
using Q2.TeeLab.OrderFulfillment.Interfaces.REST.Resources;
using Q2.TeeLab.OrderFulfillment.Interfaces.REST.Transform;
using Swashbuckle.AspNetCore.Annotations;

namespace Q2.TeeLab.OrderFulfillment.Interfaces.REST;

[ApiController]
[Route("api/v1/order-fulfillment/manufacturers")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Order Processing")]
public class ManufacturerController(
    IManufacturerCommandService manufacturerCommandService,
    IManufacturerQueryService manufacturerQueryService
) : BaseApiController
{
    [HttpGet]
    [SwaggerOperation(
        Summary = "Get all active manufacturers",
        Description = "Retrieve all active manufacturers in the system",
        Tags = new[] { "Order Processing" })]
    [SwaggerResponse(200, "Manufacturers found", typeof(OrderFulfillmentApiResponse<IEnumerable<ManufacturerResource>>))]
    public async Task<ActionResult<OrderFulfillmentApiResponse<IEnumerable<ManufacturerResource>>>> GetAllActiveManufacturers()
    {
        try
        {
            var query = new GetAllActiveManufacturersQuery();
            var manufacturers = await manufacturerQueryService.Handle(query);
            var resources = manufacturers.Select(ManufacturerResourceAssembler.ToResource);
            
            return Ok(OrderFulfillmentApiResponse<IEnumerable<ManufacturerResource>>.SuccessResponse(resources));
        }
        catch (Exception ex)
        {
            return HandleError<IEnumerable<ManufacturerResource>>(ex, "An error occurred while retrieving manufacturers");
        }
    }

    [HttpGet("{manufacturerId:guid}")]
    [SwaggerOperation(
        Summary = "Get manufacturer by ID",
        Description = "Retrieve a specific manufacturer by their ID",
        Tags = new[] { "Order Processing" })]
    [SwaggerResponse(200, "Manufacturer found", typeof(OrderFulfillmentApiResponse<ManufacturerResource>))]
    [SwaggerResponse(404, "Manufacturer not found", typeof(OrderFulfillmentApiResponse<ManufacturerResource>))]
    public async Task<ActionResult<OrderFulfillmentApiResponse<ManufacturerResource>>> GetManufacturerById(Guid manufacturerId)
    {
        try
        {
            var query = new GetManufacturerByIdQuery(new Domain.Model.ValueObjects.ManufacturerId(manufacturerId));
            var manufacturer = await manufacturerQueryService.Handle(query);

            if (manufacturer == null)
            {
                return NotFound(OrderFulfillmentApiResponse<ManufacturerResource>.ErrorResponse($"Manufacturer with ID {manufacturerId} not found"));
            }

            var resource = ManufacturerResourceAssembler.ToResource(manufacturer);
            return Ok(OrderFulfillmentApiResponse<ManufacturerResource>.SuccessResponse(resource));
        }
        catch (Exception ex)
        {
            return HandleError<ManufacturerResource>(ex, "An error occurred while retrieving the manufacturer");
        }
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Create new manufacturer",
        Description = "Create a new manufacturer in the system",
        Tags = new[] { "Order Processing" })]
    [SwaggerResponse(201, "Manufacturer created successfully", typeof(OrderFulfillmentApiResponse<Guid>))]
    [SwaggerResponse(400, "Bad request", typeof(OrderFulfillmentApiResponse<Guid>))]
    public async Task<ActionResult<OrderFulfillmentApiResponse<Guid>>> CreateManufacturer(
        [FromBody] CreateManufacturerResource resource)
    {
        try
        {
            var command = ManufacturerResourceAssembler.ToCommand(resource);
            var manufacturerId = await manufacturerCommandService.Handle(command);

            return Created($"/api/v1/order-fulfillment/manufacturers/{manufacturerId}", 
                OrderFulfillmentApiResponse<Guid>.SuccessResponse(manufacturerId, "Manufacturer created successfully"));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(OrderFulfillmentApiResponse<Guid>.ErrorResponse("Invalid input data", new[] { ex.Message }));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(OrderFulfillmentApiResponse<Guid>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            return HandleError<Guid>(ex, "An error occurred while creating the manufacturer");
        }
    }
}

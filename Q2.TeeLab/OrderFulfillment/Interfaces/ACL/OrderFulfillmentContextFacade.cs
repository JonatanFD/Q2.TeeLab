using Q2.TeeLab.OrderFulfillment.Application.Internal.CommandServices;
using Q2.TeeLab.OrderFulfillment.Application.Internal.QueryServices;
using Q2.TeeLab.OrderFulfillment.Domain.Model.Aggregates;
using Q2.TeeLab.OrderFulfillment.Domain.Model.Commands;
using Q2.TeeLab.OrderFulfillment.Domain.Model.Queries;
using Q2.TeeLab.OrderFulfillment.Domain.Model.ValueObjects;
using Q2.TeeLab.Shared.Domain.Model.ValueObjects;

namespace Q2.TeeLab.OrderFulfillment.Interfaces.ACL;

/// <summary>
/// Implementation of the Order Fulfillment context facade.
/// This class serves as an Anti-Corruption Layer, providing a simplified
/// interface for other bounded contexts to interact with Order Fulfillment functionality.
/// </summary>
public class OrderFulfillmentContextFacade : IOrderFulfillmentContextFacade
{
    private readonly IManufacturerCommandService manufacturerCommandService;
    private readonly IManufacturerQueryService manufacturerQueryService;
    private readonly IOrderFulfillmentCommandService orderFulfillmentCommandService;
    private readonly IOrderFulfillmentQueryService orderFulfillmentQueryService;

    public OrderFulfillmentContextFacade(
        IManufacturerCommandService manufacturerCommandService,
        IManufacturerQueryService manufacturerQueryService,
        IOrderFulfillmentCommandService orderFulfillmentCommandService,
        IOrderFulfillmentQueryService orderFulfillmentQueryService)
    {
        this.manufacturerCommandService = manufacturerCommandService;
        this.manufacturerQueryService = manufacturerQueryService;
        this.orderFulfillmentCommandService = orderFulfillmentCommandService;
        this.orderFulfillmentQueryService = orderFulfillmentQueryService;
    }

    public async Task<Guid?> CreateManufacturerAsync(
        string companyName,
        string contactPersonName,
        string email,
        string phoneNumber,
        string taxIdentificationNumber,
        string street,
        string city,
        string state,
        string postalCode,
        string country,
        string? website = null,
        string? specialization = null)
    {
        try
        {
            var command = new CreateManufacturerCommand(
                companyName,
                contactPersonName,
                email,
                phoneNumber,
                taxIdentificationNumber,
                street,
                city,
                state,
                postalCode,
                country,
                website,
                specialization);

            var manufacturerId = await manufacturerCommandService.Handle(command);
            return manufacturerId;
        }
        catch (Exception)
        {
            // Log the exception if needed
            return null;
        }
    }

    public async Task<Guid?> FetchManufacturerIdByTaxIdAsync(string taxIdentificationNumber)
    {
        try
        {
            // We need to add this method to the repository and query service
            // For now, we'll implement a workaround using existing methods
            var manufacturers = await manufacturerQueryService.Handle(new GetAllActiveManufacturersQuery());
            var manufacturer = manufacturers.FirstOrDefault(m => m.TaxIdentificationNumber == taxIdentificationNumber);
            return manufacturer?.Id;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<Guid?> FetchManufacturerIdByEmailAsync(string email)
    {
        try
        {
            // We need to add this method to the repository and query service
            // For now, we'll implement a workaround using existing methods
            var manufacturers = await manufacturerQueryService.Handle(new GetAllActiveManufacturersQuery());
            var manufacturer = manufacturers.FirstOrDefault(m => m.Email == email);
            return manufacturer?.Id;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<Guid?> CreateOrderFulfillmentAsync(
        Guid orderId,
        Guid customerId,
        Guid manufacturerId,
        string projectName,
        string? projectDescription = null,
        string? specialInstructions = null)
    {
        try
        {
            var command = new CreateOrderFulfillmentCommand(
                orderId,
                new UserId(customerId),
                new ManufacturerId(manufacturerId),
                projectName,
                projectDescription,
                specialInstructions);

            var orderFulfillmentId = await orderFulfillmentCommandService.Handle(command);
            return orderFulfillmentId;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<Guid?> FetchOrderFulfillmentIdByOrderIdAsync(Guid orderId)
    {
        try
        {
            // We need to add this query to the query service
            // For now, we'll implement a workaround
            var allOrderFulfillments = await orderFulfillmentQueryService.Handle(
                new GetOrderFulfillmentsByStatusQuery(OrderFulfillmentStatus.Pending));
            
            // This is not efficient, but works for the ACL pattern
            // In a real implementation, we'd add a specific query
            var orderFulfillment = allOrderFulfillments.FirstOrDefault(of => of.OrderId == orderId);
            return orderFulfillment?.Id;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<bool> IsManufacturerActiveAsync(Guid manufacturerId)
    {
        try
        {
            var query = new GetManufacturerByIdQuery(new ManufacturerId(manufacturerId));
            var manufacturer = await manufacturerQueryService.Handle(query);
            return manufacturer?.IsActive ?? false;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<string?> FetchOrderFulfillmentStatusAsync(Guid orderFulfillmentId)
    {
        try
        {
            var query = new GetOrderFulfillmentByIdQuery(new OrderFulfillmentId(orderFulfillmentId));
            var orderFulfillment = await orderFulfillmentQueryService.Handle(query);
            return orderFulfillment?.Status.ToString();
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<Manufacturer?> FetchManufacturerAsync(Guid manufacturerId)
    {
        try
        {
            var query = new GetManufacturerByIdQuery(new ManufacturerId(manufacturerId));
            return await manufacturerQueryService.Handle(query);
        }
        catch (Exception)
        {
            return null;
        }
    }
}

using Q2.TeeLab.OrderFulfillment.Domain.Model.Aggregates;

namespace Q2.TeeLab.OrderFulfillment.Interfaces.ACL;

/// <summary>
/// Anti-Corruption Layer facade for Order Fulfillment bounded context.
/// This facade provides a simplified interface for other bounded contexts
/// to interact with Order Fulfillment functionality without exposing internal details.
/// </summary>
public interface IOrderFulfillmentContextFacade
{
    /// <summary>
    /// Creates a new manufacturer in the system
    /// </summary>
    /// <returns>The manufacturer ID if created successfully, null otherwise</returns>
    Task<Guid?> CreateManufacturerAsync(
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
        string? specialization = null);

    /// <summary>
    /// Retrieves a manufacturer by their tax identification number (RUC)
    /// </summary>
    /// <returns>The manufacturer ID if found, null otherwise</returns>
    Task<Guid?> FetchManufacturerIdByTaxIdAsync(string taxIdentificationNumber);

    /// <summary>
    /// Retrieves a manufacturer by their email address
    /// </summary>
    /// <returns>The manufacturer ID if found, null otherwise</returns>
    Task<Guid?> FetchManufacturerIdByEmailAsync(string email);

    /// <summary>
    /// Creates a new order fulfillment for a given order
    /// </summary>
    /// <returns>The order fulfillment ID if created successfully, null otherwise</returns>
    Task<Guid?> CreateOrderFulfillmentAsync(
        Guid orderId,
        Guid customerId,
        Guid manufacturerId,
        string projectName,
        string? projectDescription = null,
        string? specialInstructions = null);

    /// <summary>
    /// Retrieves an order fulfillment by the original order ID
    /// </summary>
    /// <returns>The order fulfillment ID if found, null otherwise</returns>
    Task<Guid?> FetchOrderFulfillmentIdByOrderIdAsync(Guid orderId);

    /// <summary>
    /// Checks if a manufacturer exists and is active
    /// </summary>
    Task<bool> IsManufacturerActiveAsync(Guid manufacturerId);

    /// <summary>
    /// Gets the current status of an order fulfillment
    /// </summary>
    /// <returns>The status as string if found, null otherwise</returns>
    Task<string?> FetchOrderFulfillmentStatusAsync(Guid orderFulfillmentId);

    /// <summary>
    /// Gets basic manufacturer information for display purposes
    /// </summary>
    Task<Manufacturer?> FetchManufacturerAsync(Guid manufacturerId);
}

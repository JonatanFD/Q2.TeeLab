using Q2.TeeLab.OrderProcessing.Application.Internal.CommandServices;
using Q2.TeeLab.OrderProcessing.Domain.Model.Aggregates;
using Q2.TeeLab.OrderProcessing.Domain.Model.Commands;
using Q2.TeeLab.OrderProcessing.Domain.Repositories;
using Q2.TeeLab.OrderProcessing.Domain.Services;
using Q2.TeeLab.Shared.Domain.Repositories;

namespace Q2.TeeLab.OrderProcessing.Application.Internal.CommandServices;

public class OrderCommandService : IOrderCommandService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICartRepository _cartRepository;
    private readonly IOrderManagementService _orderManagementService;
    private readonly IPaymentServicePort _paymentService;
    private readonly IUnitOfWork _unitOfWork;

    public OrderCommandService(
        IOrderRepository orderRepository,
        ICartRepository cartRepository,
        IOrderManagementService orderManagementService,
        IPaymentServicePort paymentService,
        IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _cartRepository = cartRepository;
        _orderManagementService = orderManagementService;
        _paymentService = paymentService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Order> Handle(CreateOrderFromCartCommand command)
    {
        var cart = await _cartRepository.FindByUserIdAsync(command.UserId);
        if (cart == null || cart.IsEmpty)
            throw new InvalidOperationException("Cart is empty or does not exist");

        // Validate all items are still available
        var isValid = await _orderManagementService.ValidateOrderItemsAsync(cart.Items);
        if (!isValid)
            throw new InvalidOperationException("Some items in the cart are no longer available");

        // Create order from cart
        var order = cart.ConvertToOrder(command.Notes);

        // Save order
        await _orderRepository.AddAsync(order);

        // Clear cart after successful order creation
        cart.Clear();
        _cartRepository.Update(cart);

        await _unitOfWork.CompleteAsync();

        return order;
    }

    public async Task<bool> Handle(ConfirmOrderCommand command)
    {
        var order = await _orderRepository.FindByIdAsync(command.OrderId);
        if (order == null)
            return false;

        order.Confirm();
        _orderRepository.Update(order);
        await _unitOfWork.CompleteAsync();

        return true;
    }

    public async Task<bool> Handle(CancelOrderCommand command)
    {
        var order = await _orderRepository.FindByIdAsync(command.OrderId);
        if (order == null)
            return false;

        order.Cancel();
        _orderRepository.Update(order);
        await _unitOfWork.CompleteAsync();

        return true;
    }

    public async Task<bool> Handle(UpdateOrderStatusCommand command)
    {
        var order = await _orderRepository.FindByIdAsync(command.OrderId);
        if (order == null)
            return false;

        order.ChangeStatus(command.NewStatus);

        if (!string.IsNullOrEmpty(command.TrackingNumber))
        {
            order.SetTrackingNumber(command.TrackingNumber);
        }

        _orderRepository.Update(order);
        await _unitOfWork.CompleteAsync();

        return true;
    }

    public async Task<bool> Handle(ApplyDiscountToOrderCommand command)
    {
        var order = await _orderRepository.FindByIdAsync(command.OrderId);
        if (order == null)
            return false;

        order.ApplyGlobalDiscount(command.Discount);
        _orderRepository.Update(order);
        await _unitOfWork.CompleteAsync();

        return true;
    }
}

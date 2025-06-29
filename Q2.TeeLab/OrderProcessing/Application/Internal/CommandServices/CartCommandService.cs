using Q2.TeeLab.OrderProcessing.Application.Internal.CommandServices;
using Q2.TeeLab.OrderProcessing.Domain.Model.Commands;
using Q2.TeeLab.OrderProcessing.Domain.Repositories;
using Q2.TeeLab.OrderProcessing.Domain.Services;
using Q2.TeeLab.Shared.Domain.Repositories;

namespace Q2.TeeLab.OrderProcessing.Application.Internal.CommandServices;

public class CartCommandService : ICartCommandService
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductCatalogService _productCatalogService;
    private readonly IOrderManagementService _orderManagementService;
    private readonly IUnitOfWork _unitOfWork;

    public CartCommandService(
        ICartRepository cartRepository,
        IProductCatalogService productCatalogService,
        IOrderManagementService orderManagementService,
        IUnitOfWork unitOfWork)
    {
        _cartRepository = cartRepository;
        _productCatalogService = productCatalogService;
        _orderManagementService = orderManagementService;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(AddItemToCartCommand command)
    {
        var product = await _productCatalogService.GetProductInfoAsync(command.ProductId);
        if (product == null)
            throw new ArgumentException($"Product with ID {command.ProductId} not found");

        var isAvailable = await _productCatalogService.IsProductAvailableAsync(command.ProductId);
        if (!isAvailable)
            throw new InvalidOperationException($"Product {command.ProductId} is not available");

        var cart = await _cartRepository.GetOrCreateByUserIdAsync(command.UserId);
        cart.AddItem(product, command.Quantity);

        _cartRepository.Update(cart);
        await _unitOfWork.CompleteAsync();
    }

    public async Task Handle(UpdateCartItemQuantityCommand command)
    {
        var cart = await _cartRepository.FindByUserIdAsync(command.UserId);
        if (cart == null)
            throw new InvalidOperationException("Cart not found");

        cart.UpdateItemQuantity(command.ProductId, command.Quantity);
        _cartRepository.Update(cart);
        await _unitOfWork.CompleteAsync();
    }

    public async Task Handle(RemoveItemFromCartCommand command)
    {
        var cart = await _cartRepository.FindByUserIdAsync(command.UserId);
        if (cart == null)
            return; // Nothing to remove

        cart.RemoveItem(command.ProductId);
        _cartRepository.Update(cart);
        await _unitOfWork.CompleteAsync();
    }

    public async Task Handle(ClearCartCommand command)
    {
        var cart = await _cartRepository.FindByUserIdAsync(command.UserId);
        if (cart == null)
            return; // Nothing to clear

        cart.Clear();
        _cartRepository.Update(cart);
        await _unitOfWork.CompleteAsync();
    }

    public async Task Handle(ApplyDiscountToCartCommand command)
    {
        var cart = await _cartRepository.FindByUserIdAsync(command.UserId);
        if (cart == null)
            throw new InvalidOperationException("Cart not found");

        var canApply = await _orderManagementService.CanApplyDiscountAsync(command.Discount, command.UserId);
        if (!canApply)
            throw new InvalidOperationException("Discount cannot be applied to this user");

        cart.ApplyDiscount(command.Discount);
        _cartRepository.Update(cart);
        await _unitOfWork.CompleteAsync();
    }
}

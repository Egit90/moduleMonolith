using Basket.Basket.Dtos;
using Basket.Repository;
using Catalog.Contracts.Products.Features.GetProductById;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Basket.Basket.Features.AddItemIntoBasket;

public sealed record AddItemIntoBasketCommand(string UserName, ShoppingCartItemDto ShoppingCartItem) : ICommand<AddItemIntoBasketResult>;
public sealed record AddItemIntoBasketResult(Guid Id);

public sealed class AddItemIntoBasketCommandValidator : AbstractValidator<AddItemIntoBasketCommand>
{
    public AddItemIntoBasketCommandValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName is required");
        RuleFor(x => x.ShoppingCartItem.ProductId).NotEmpty().WithMessage("ProductId is required");
        RuleFor(x => x.ShoppingCartItem.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0");
    }
}

internal sealed class AddItemIntoBasketHandler(IBasketRepository basketRepository, ISender sender) : ICommandHandler<AddItemIntoBasketCommand, AddItemIntoBasketResult>
{
    public async Task<AddItemIntoBasketResult> Handle(AddItemIntoBasketCommand command, CancellationToken cancellationToken)
    {

        var shoppingCart = await basketRepository.GetBasket(command.UserName, false, cancellationToken);

        var ItemInformation = await sender.Send(new GetProductByIdQuery(command.ShoppingCartItem.ProductId), cancellationToken);

        shoppingCart.AddItem(
            command.ShoppingCartItem.ProductId,
            command.ShoppingCartItem.Quantity,
            command.ShoppingCartItem.Color,
            ItemInformation.Product.Price,
            ItemInformation.Product.Name);

        await basketRepository.SaveChangesAsync(command.UserName, cancellationToken);

        return new AddItemIntoBasketResult(shoppingCart.Id);

    }
}
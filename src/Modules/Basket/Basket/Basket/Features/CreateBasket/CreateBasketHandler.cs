using System.Data;
using Basket.Basket.Dtos;
using Basket.Basket.Models;
using Basket.Data;
using FluentValidation;
using Shared.CQRS;

namespace Basket.Basket.Features.CreateBasket;

public sealed record CreateBasketCommand(ShoppingCartDto ShoppingCart) : ICommand<CreateBasketResult>;
public sealed record CreateBasketResult(Guid Id);

public sealed class CreateBasketCommandValidator : AbstractValidator<CreateBasketCommand>
{
    public CreateBasketCommandValidator()
    {
        RuleFor(x => x.ShoppingCart.UserName).NotEmpty().WithMessage("User Name is Required.");
    }
}

internal sealed class CreateBasketHandler(BasketDbContext context) : ICommandHandler<CreateBasketCommand, CreateBasketResult>
{
    public async Task<CreateBasketResult> Handle(CreateBasketCommand command, CancellationToken cancellationToken)
    {
        ShoppingCart ShoppingCart = CreateNewShoppingCart(command.ShoppingCart);
        context.ShoppingCarts.Add(ShoppingCart);
        await context.SaveChangesAsync(cancellationToken);
        return new(ShoppingCart.Id);
    }

    private ShoppingCart CreateNewShoppingCart(ShoppingCartDto shoppingCart)
    {
        var basket = ShoppingCart.Create(Guid.NewGuid(), shoppingCart.UserName);
        shoppingCart.Items.ForEach(item =>
        {
            basket.AddItem(item.ProductId, item.Quantity, item.Color, item.Price, item.ProductName);
        });

        return basket;
    }
}

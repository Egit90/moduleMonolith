using Basket.Basket.Dtos;
using Basket.Basket.Models;
using Basket.Repository;
using FluentValidation;

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

internal sealed class CreateBasketHandler(IBasketRepository basketRepository) : ICommandHandler<CreateBasketCommand, CreateBasketResult>
{
    public async Task<CreateBasketResult> Handle(CreateBasketCommand command, CancellationToken cancellationToken)
    {
        ShoppingCart ShoppingCart = CreateNewShoppingCart(command.ShoppingCart);
        await basketRepository.CreateBasket(ShoppingCart, cancellationToken);
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

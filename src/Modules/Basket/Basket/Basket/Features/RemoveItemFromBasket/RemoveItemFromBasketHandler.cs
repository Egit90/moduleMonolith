
using Basket.Basket.Exceptions;
using Basket.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Shared.CQRS;

namespace Basket.Basket.Features.RemoveItemFromBasket;

public sealed record RemoveItemFromBasketCommand(string UserName, Guid ProductId) : ICommand<RemoveItemFromBasketResult>;
public sealed record RemoveItemFromBasketResult(Guid Id);

public sealed class RemoveItemFromBasketCommandValidator : AbstractValidator<RemoveItemFromBasketCommand>
{
    public RemoveItemFromBasketCommandValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName is required");
        RuleFor(x => x.ProductId).NotEmpty().WithMessage("ProductId is required");
    }
}

internal sealed class RemoveItemFromBasketHandler(BasketDbContext context) : ICommandHandler<RemoveItemFromBasketCommand, RemoveItemFromBasketResult>
{
    public async Task<RemoveItemFromBasketResult> Handle(RemoveItemFromBasketCommand command, CancellationToken cancellationToken)
    {
        var cart = await context.ShoppingCarts
                                .Include(x => x.Items)
                                .SingleOrDefaultAsync(x => x.UserName == command.UserName, cancellationToken: cancellationToken)
                                ?? throw new BasketNotFoundException(command.UserName);

        cart.RemoveItem(command.ProductId);

        await context.SaveChangesAsync(cancellationToken);

        return new RemoveItemFromBasketResult(cart.Id);


    }
}
using Basket.Basket.Exceptions;
using Basket.Basket.Models;
using Basket.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Shared.CQRS;

namespace Basket.Basket.Features.DeleteBasket;

public sealed record DeleteBasketCommand(string UserName) : ICommand<DeleteBasketResult>;
public sealed record DeleteBasketResult(bool IsSuccess);


public sealed class DeleteBasketCommandValidator : AbstractValidator<DeleteBasketCommand>
{
    public DeleteBasketCommandValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().WithMessage("Basket UserName Must Be Provided.");
    }
}

internal sealed class DeleteBasketHandler(BasketDbContext context) : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
{
    public async Task<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
    {
        var basket = await context.ShoppingCarts.SingleOrDefaultAsync(x => x.UserName == command.UserName, cancellationToken: cancellationToken)
                    ?? throw new BasketNotFoundException(command.UserName);

        context.ShoppingCarts.Remove(basket);
        await context.SaveChangesAsync(cancellationToken);
        return new(true);
    }
}


using Basket.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Basket.Basket.Features.UpdateItemPriceInBasket;

public sealed record UpdateItemPriceInBasketCommand(Guid ProductId, decimal price) : ICommand<UpdateItemPriceInBasketResult>;

public sealed record UpdateItemPriceInBasketResult(bool IsSuccess);

public class UpdateItemPriceInBasketCommandValidator : AbstractValidator<UpdateItemPriceInBasketCommand>
{
    public UpdateItemPriceInBasketCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty().WithMessage("Product Id is required");
        RuleFor(x => x.price).GreaterThan(0).WithMessage("Price Must Be Greater Than 0");
    }
}

public sealed class UpdateItemPriceInBasketHandler(BasketDbContext context) : ICommandHandler<UpdateItemPriceInBasketCommand, UpdateItemPriceInBasketResult>
{
    public async Task<UpdateItemPriceInBasketResult> Handle(UpdateItemPriceInBasketCommand command, CancellationToken cancellationToken)
    {
        var itemsToUpdate = await context.ShoppingCartItems
                            .Where(x => x.ProductId == command.ProductId)
                            .ToListAsync(cancellationToken);

        if (itemsToUpdate.Count == 0)
        {
            return new(false);
        }

        foreach (var item in itemsToUpdate)
        {
            item.UpdatePrice(command.price);
        }

        await context.SaveChangesAsync(cancellationToken);

        return new(true);
    }
}

using Basket.Basket.Dtos;
using Basket.Basket.Exceptions;
using Basket.Basket.Models;
using Basket.Data;
using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Shared.CQRS;

namespace Basket.Basket.Features.GetBasket;

public record GetBasketQuery(string UserName) : ICommand<GetBasketResult>;
public record GetBasketResult(ShoppingCartDto ShoppingCart);

public sealed class GetBasketValidator : AbstractValidator<GetBasketQuery>
{
    public GetBasketValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName Is Required");
    }
}

internal class GetBasketHandler(BasketDbContext context) : ICommandHandler<GetBasketQuery, GetBasketResult>
{
    public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
    {
        var basket = await context.ShoppingCarts
                            .AsNoTracking()
                            .Include(x => x.Items)
                            .SingleOrDefaultAsync(x => x.UserName == query.UserName, cancellationToken: cancellationToken)
                        ?? throw new BasketNotFoundException(query.UserName);

        var dto = basket.Adapt<ShoppingCartDto>();

        return new GetBasketResult(dto);

    }
}
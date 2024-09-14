using Basket.Basket.Dtos;
using Basket.Repository;
using FluentValidation;
using Mapster;

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

internal class GetBasketHandler(IBasketRepository basketRepository) : ICommandHandler<GetBasketQuery, GetBasketResult>
{
    public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
    {
        var basket = await basketRepository.GetBasket(query.UserName, true, cancellationToken: cancellationToken);

        var dto = basket.Adapt<ShoppingCartDto>();

        return new GetBasketResult(dto);

    }
}
using Basket.Repository;
using FluentValidation;
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

internal sealed class RemoveItemFromBasketHandler(IBasketRepository basketRepository) : ICommandHandler<RemoveItemFromBasketCommand, RemoveItemFromBasketResult>
{
    public async Task<RemoveItemFromBasketResult> Handle(RemoveItemFromBasketCommand command, CancellationToken cancellationToken)
    {
        var cart = await basketRepository.GetBasket(command.UserName, false, cancellationToken);

        cart.RemoveItem(command.ProductId);

        await basketRepository.SaveChangesAsync(command.UserName, cancellationToken);

        return new RemoveItemFromBasketResult(cart.Id);


    }
}
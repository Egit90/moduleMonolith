using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Basket.Basket.Features.RemoveItemFromBasket;
public sealed record RemoveItemFromBasketResponse(Guid Id);
public sealed class RemoveItemFromBasketEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/basket/{username}/items/{productId}", async ([FromRoute] string userName, [FromRoute] Guid productId, ISender sender) =>
        {
            var command = new RemoveItemFromBasketCommand(userName, productId);

            var result = await sender.Send(command);

            var response = result.Adapt<RemoveItemFromBasketResponse>();

            return Results.Ok(response);
        })
        .Produces<RemoveItemFromBasketResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Remove Item From Basket")
        .WithDescription("Remove Item From Basket")
        .RequireAuthorization(); ;
    }
}
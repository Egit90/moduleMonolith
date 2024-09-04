using Basket.Basket.Dtos;
using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Basket.Basket.Features.CreateBasket;

public sealed record CreateBasketRequest(ShoppingCartDto ShoppingCart);
public sealed record CreateBasketResponse(Guid Id);

public sealed class CreateBasketEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket", async (CreateBasketRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateBasketCommand>();

            var result = await sender.Send(command);

            return Results.Created($"/basket/{result.Id}", result.Id);
        })
        .Produces<CreateBasketResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Create Basket")
        .WithDescription("Create Basket")
        .RequireAuthorization(); ;
    }
}
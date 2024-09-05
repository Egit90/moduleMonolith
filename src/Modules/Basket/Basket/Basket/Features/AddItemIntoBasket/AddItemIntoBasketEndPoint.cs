using Basket.Basket.Dtos;
using Basket.Basket.Models;
using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Basket.Basket.Features.AddItemIntoBasket;

public sealed record AddItemIntoBasketRequest(string UserName, ShoppingCartItemDto ShoppingCartItem);
public sealed record AddItemIntoBasketResponse(Guid Id);
public sealed class AddItemIntoBasketEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket/{userName}/item", async ([FromRoute] string userName, [FromBody] AddItemIntoBasketRequest request, ISender sender) =>
        {
            var command = new AddItemIntoBasketCommand(userName, request.ShoppingCartItem);
            var res = await sender.Send(command);
            var response = res.Adapt<AddItemIntoBasketResponse>();

            return Results.Created($"/basket/{response.Id}", response.Id);
        })
        .Produces<AddItemIntoBasketResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Add Item Into Basket")
        .WithDescription("Add Item Into Basket");
    }
}
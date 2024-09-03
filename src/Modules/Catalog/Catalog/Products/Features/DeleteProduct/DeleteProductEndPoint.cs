using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Catalog.Products.Features.DeleteProduct;
public sealed record DeleteProductEndPointResponse(bool IsSuccess);
public sealed class DeleteProductEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("products/{id}", async (Guid id, ISender sender) =>
        {
            var commandRes = await sender.Send(new DeleteProductCommand(id));
            var res = commandRes.Adapt<DeleteProductEndPointResponse>();
            return Results.Ok(res);
        })
        .WithName("DeleteProduct")
        .Produces<DeleteProductEndPointResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Delete Product")
        .WithDescription("Delete Product"); ; ;
    }
}

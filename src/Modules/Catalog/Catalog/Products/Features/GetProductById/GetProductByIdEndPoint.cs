using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Catalog.Products.Features.GetProductById;

public sealed record GetProductByIdResponse(ProductDto Product);

public class GetProductByIdEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/{id}", async (Guid id, ISender sender) =>
        {
            var GetProductByIdCommand = new GetProductByIdQuery(id);
            var GetProductByIdCommandRes = await sender.Send(GetProductByIdCommand);
            var res = GetProductByIdCommandRes.Adapt<GetProductByIdResponse>();
            return Results.Ok(res);
        })
        .WithName("GetProductById")
        .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Product By Id")
        .WithDescription("Get Product By Id"); ;
    }
}

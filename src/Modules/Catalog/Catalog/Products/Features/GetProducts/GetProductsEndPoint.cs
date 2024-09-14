using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Pagination;

namespace Catalog.Products.Features.GetProducts;

public sealed record GetProductsResponse(PaginatedResult<ProductDto> Products);
public sealed class GetProductsEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async ([AsParameters] PaginatedRequest request, ISender sender) =>
        {
            var GetProductsQueryRes = await sender.Send(new GetProductsQuery(request));
            var res = GetProductsQueryRes.Adapt<GetProductsResponse>();
            return Results.Ok(res);
        })
        .WithName("GetProducts")
        .Produces<GetProductsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Products")
        .WithDescription("Get Products"); ;
    }
}

using System;
using Carter;
using Catalog.Products.Dtos;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Catalog.Products.Features.UpdateProduct;

public sealed record UpdateProductRequest(ProductDto Product);
public sealed record UpdateProductResponse(bool IsSuccess);
public sealed class UpdateProductEndPoint : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/products", async (UpdateProductRequest request, ISender sender) =>
        {
            var UpdateCommand = request.Adapt<UpdateProductCommand>();
            var UpdateCommandRes = await sender.Send(UpdateCommand);
            var res = UpdateCommandRes.Adapt<UpdateProductResponse>();

            return Results.Ok(res);
        })
        .WithName("UpdateProduct")
        .Produces<UpdateProductResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
           .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Update Product")
        .WithDescription("Update Product"); ;
    }
}

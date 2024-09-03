using System;
using Carter;
using Catalog.Products.Dtos;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Catalog.Products.Features.UpdateProduct;

public sealed record UpdateProductRequest(ProductDto ProductDto);
public sealed record UpdateProductResponse(bool IsSuccess);
public sealed class UpdateProductEndPoint : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/products", async (UpdateProductRequest request, ISender sender) =>
        {
            var UpdateCommand = request.ProductDto.Adapt<UpdateProductCommand>();
            var UpdateCommandRes = await sender.Send(UpdateCommand);
            var res = UpdateCommand.Adapt<UpdateProductResponse>();

            return Results.Ok(res);
        })
        .WithName("UpdateProduct")
        .Produces<UpdateProductResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Update Product")
        .WithDescription("Update Product"); ;
    }
}

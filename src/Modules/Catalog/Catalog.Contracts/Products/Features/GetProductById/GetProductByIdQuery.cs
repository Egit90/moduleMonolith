using Catalog.Contracts.Products.Dtos;
using Shared.Contracts.CQRS;

namespace Catalog.Contracts.Products.Features.GetProductById;

public sealed record GetProductByIdQuery(Guid ProductId) : IQuery<GetProductByIdResults>;
public sealed record GetProductByIdResults(ProductDto Product);

using Catalog.Data;
using Catalog.Products.Exceptions;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Products.Features.GetProductById;

public class GetProductByIdHandler(CatalogDbContext dbContext) : IQueryHandler<GetProductByIdQuery, GetProductByIdResults>
{
    public async Task<GetProductByIdResults> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.Id == request.ProductId, cancellationToken)
                    ?? throw new ProductNotFoundException(request.ProductId);

        var productDto = product.Adapt<ProductDto>();

        return new(productDto);
    }
}
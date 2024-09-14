using Catalog.Data;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Shared.Pagination;

namespace Catalog.Products.Features.GetProducts;

public sealed record GetProductsQuery(PaginatedRequest PaginatedRequest) : IQuery<GetProductsResults>;

public sealed record GetProductsResults(PaginatedResult<ProductDto> Products);
public sealed class GetProductsHandler(CatalogDbContext dbContext) : IQueryHandler<GetProductsQuery, GetProductsResults>
{
    public async Task<GetProductsResults> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var pageIndex = request.PaginatedRequest.PageIndex;
        var pageSize = request.PaginatedRequest.PageSize;
        var totalCount = await dbContext.Products.LongCountAsync(cancellationToken);

        var products = await dbContext.Products
                     .AsNoTracking()
                     .OrderBy(x => x.Name)
                     .Skip(pageSize * pageIndex)
                     .Take(pageSize)
                     .ToListAsync(cancellationToken);

        var productsDto = products.Adapt<List<ProductDto>>();

        return new GetProductsResults(new(pageIndex, pageSize, totalCount, productsDto));
    }
}
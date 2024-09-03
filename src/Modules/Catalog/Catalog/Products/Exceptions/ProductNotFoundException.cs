using Shared.Exceptions;

namespace Catalog.Products.Exceptions;

public sealed class ProductNotFoundException(Guid id) : NotFoundException("Product", id)
{
}
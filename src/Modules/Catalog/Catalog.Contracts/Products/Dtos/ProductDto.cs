namespace Catalog.Contracts.Products.Dtos;

public sealed record ProductDto(
    Guid Id,
    string Name,
    List<string> Category,
    string Description,
    string ImageFile,
    decimal Price
);

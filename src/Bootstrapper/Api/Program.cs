using Basket;
using Carter;
using Catalog;
using Ordering;
using Shared.Exceptions.Handler;
using Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCarterWithAssemblies(typeof(CatalogModule).Assembly);

// Add Services
builder.Services
       .AddCatalogModule(builder.Configuration)
       .AddBasketModule(builder.Configuration)
       .AddOrderingModule(builder.Configuration);

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline

app.MapCarter();

app.UseCatalogModule()
    .UseBasketModule()
    .UseOrderingModule();

app.UseExceptionHandler(opt => { });

app.Run();

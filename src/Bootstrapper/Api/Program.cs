using Basket;
using Carter;
using Catalog;
using Ordering;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCarter(configurator: config =>
{
    var CarterModules = typeof(CarterModule).Assembly.GetTypes()
                .Where(t => t.IsAssignableTo(typeof(ICarterModule)))
                .ToArray();

    config.WithModules(CarterModules);
});

// Add Services
builder.Services
       .AddCatalogModule(builder.Configuration)
       .AddBasketModule(builder.Configuration)
       .AddOrderingModule(builder.Configuration);


var app = builder.Build();

// Configure the HTTP request pipeline

app.MapCarter();

app.UseCatalogModule()
    .UseBasketModule()
    .UseOrderingModule();


app.Run();

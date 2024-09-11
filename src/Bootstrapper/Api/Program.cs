
var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) => config.ReadFrom.Configuration(context.Configuration));

// Add Services
// Carter medatoR , fluentValidation
var catalogAssemble = typeof(CatalogModule).Assembly;
var basketAssemble = typeof(BasketModule).Assembly;

builder.Services
        .AddCarterWithAssemblies(catalogAssemble, basketAssemble);

builder.Services
       .AddMediatorWithAssemblies(catalogAssemble, basketAssemble);

builder.Services.AddStackExchangeRedisCache(opts =>
{
       opts.Configuration = builder.Configuration.GetConnectionString("Redis");
});

builder.Services
       .AddCatalogModule(builder.Configuration)
       .AddBasketModule(builder.Configuration)
       .AddOrderingModule(builder.Configuration);

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline

app.MapCarter();
app.UseSerilogRequestLogging();
app.UseExceptionHandler(opt => { });

app.UseCatalogModule()
    .UseBasketModule()
    .UseOrderingModule();


app.Run();

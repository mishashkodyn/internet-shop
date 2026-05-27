using Blog.Application;
using Blog.Infrastructure;
using Serilog;
using Shared.Application;
using Shared.Infrastructure;
using Shop.Application;
using Shop.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Logging — Serilog, configured from appsettings.json.
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

// Composition root: each bounded context wires its own application + infrastructure layers.
builder.Services.AddSharedApplication();
builder.Services.AddSharedInfrastructure(builder.Configuration);

builder.Services.AddShopApplication();
builder.Services.AddShopInfrastructure(builder.Configuration);

builder.Services.AddBlogApplication();
builder.Services.AddBlogInfrastructure(builder.Configuration);

// Controllers live in the Shop.API / Blog.API class libraries — register their
// assemblies as application parts so their controllers are discovered.
builder.Services.AddControllers()
    .AddApplicationPart(typeof(Shop.API.Controllers.ShopController).Assembly)
    .AddApplicationPart(typeof(Blog.API.Controllers.BlogController).Assembly);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();

using Blog.Application;
using Blog.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Serilog;
using Shared.Application;
using Shared.Infrastructure;
using Shared.Infrastructure.Identity;
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

builder.Services.AddCors(options =>
    options.AddPolicy("AllowAngularDev", policy =>
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials()));

// Controllers live in the Shop.API / Blog.API / Shared.API class libraries — register their
// assemblies as application parts so their controllers are discovered.
builder.Services.AddControllers()
    .AddApplicationPart(typeof(Shop.API.Controllers.ShopController).Assembly)
    .AddApplicationPart(typeof(Blog.API.Controllers.BlogController).Assembly)
    .AddApplicationPart(typeof(Shared.API.Controllers.AuthController).Assembly);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
    foreach (var roleName in new[] { "Admin", "Customer", "Author" })
    {
        if (!await roleManager.RoleExistsAsync(roleName))
            await roleManager.CreateAsync(new ApplicationRole { Name = roleName });
    }
}

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngularDev");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

using Microsoft.OpenApi.Models;

using WebApi.Extensions.Services;

var builder = WebApplication.CreateBuilder(args);
//Настройка сервисов.

builder.Services.AddControllers();
builder.Services.AddRepositoriesAndDbContext(builder.Configuration);
builder.Services.AddSwaggerGen(options =>
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "CoffeeMachine", Version = "v1" }));

var app = builder.Build();
//Настройка приложения.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseRouting();

app.UseEndpoints(endpoint => endpoint.MapControllers());

app.Run();
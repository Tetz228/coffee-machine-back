using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

using WebApi.Db;
using WebApi.Extensions.Services;
using WebApi.Services;
using WebApi.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);
//Настройка сервисов.

builder.Services.AddSwaggerGen(options =>
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "CoffeeMachine", Version = "v1" }));
builder.Services.AddControllers();
builder.Services.AddRepositoriesAndDbContext(builder.Configuration);
builder.Services.AddScoped<ICoffeeService, CoffeeService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IStatisticService, StatisticService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IChangeService, ChangeService>();

var app = builder.Build();
//Настройка приложения.

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<CoffeeMachineContext>().Database.Migrate();
}

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
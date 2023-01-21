using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

using NLog;
using NLog.Web;

using WebApi.Db;
using WebApi.Extensions.Services;
using WebApi.Jwt;
using WebApi.Services;
using WebApi.Services.Interfaces;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
//Настройка сервисов.

    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo { Title = "CoffeeMachine", Version = "v1" });
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "JWT."
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    });
    builder.Services.AddControllers();
    builder.Services.AddRepositoriesAndDbContext(builder.Configuration);
    builder.Services.AddScoped<ICoffeeService, CoffeeService>();
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IOrderService, OrderService>();
    builder.Services.AddScoped<IStatisticService, StatisticService>();
    builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
    builder.Services.AddScoped<IChangeService, ChangeService>();
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = JwtConfigurator.CreateTokenValidationParameters(
            builder.Configuration["Options:Jwt:Issuer"], builder.Configuration["Options:Jwt:Audience"],
            builder.Configuration["Options:Jwt:Key"]);
    });
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(builder.Configuration["Options:Cors:Name"], policy =>
            policy.WithOrigins(builder.Configuration["Options:Cors:URL"])
                .AllowAnyHeader()
                .AllowAnyMethod());
    });
    
//Подключение логирования.

    builder.Logging.ClearProviders();
    builder.Host.UseNLog();
    
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

    app.UseCors(builder.Configuration["Options:Cors:Name"]);

    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();

    app.UseEndpoints(endpoint => endpoint.MapControllers());

    app.Run();
}
catch (Exception exception)
{
    logger.Error(exception, "Программа завершила свою работу из-за ошибки.");
}
finally
{
    LogManager.Shutdown();
}
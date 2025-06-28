using Cortex.Mediator.Behaviors;
using Cortex.Mediator.Commands;
using Cortex.Mediator.DependencyInjection;
using Cortex.Mediator.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Q2.TeeLab.DesignLab.Application.Internal.CommandServices;
using Q2.TeeLab.DesignLab.Application.Internal.QueryServices;
using Q2.TeeLab.DesignLab.Domain.Repositories;
using Q2.TeeLab.DesignLab.Domain.Services;
using Q2.TeeLab.DesignLab.Infrastructure.Persistence.EFC.Repositories;
using Q2.TeeLab.Shared.Infrastructure.Persistence.EFC.Configuration;
using IUnitOfWork = Q2.TeeLab.Shared.Domain.Repositories.IUnitOfWork;
using UnitOfWork = Q2.TeeLab.Shared.Infrastructure.Persistence.EFC.Repositories.UnitOfWork;
using Microsoft.EntityFrameworkCore.SqlServer; // Add this line for SQL Server extension methods

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database Configuration
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);
});

// Shared Bounded Context
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Design Lab Bounded Context Services
builder.Services.AddScoped<IProjectCommandService, ProjectCommandService>();
builder.Services.AddScoped<IProjectQueryService, ProjectQueryService>();

// Design Lab Repositories
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<ILayerRepository, LayerRepository>();

// Add Mediator Injection Configuration
builder.Services.AddScoped(typeof(ICommandPipelineBehavior<>), typeof(LoggingCommandBehavior<>));

// Add Cortex Mediator for Event Handling
builder.Services.AddCortexMediator(
    configuration: builder.Configuration,
    handlerAssemblyMarkerTypes: new[] { typeof(Program) }, configure: options =>
    {
        options.AddOpenCommandPipelineBehavior(typeof(LoggingCommandBehavior<>));
        //options.AddDefaultBehaviors();
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Q2.TeeLab API V1");
        c.RoutePrefix = string.Empty; // This makes Swagger UI available at the root URL
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
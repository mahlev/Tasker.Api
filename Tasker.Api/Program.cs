using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Tasker.Api.Data;
using Tasker.Api.Mapping;
using Tasker.Api.Middleware;
using Tasker.Api.Repositories;
using Tasker.Api.Repositories.Interfaces;
using Tasker.Api.Services;
using Tasker.Api.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations(); // Enables [SwaggerOperation(Summary = "Get all tasks", Description = "Retrieves a list of all tasks.")] in the controllers code.
});

// Tasker Database and AutoMapper configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddAutoMapper(cfg => cfg.AddProfile<AutoMapperProfile>());

builder.Services.AddSingleton<ITaskRepository, TaskRepository>(); // AddSingleton: The DI container creates one single instance the very first time it is requested, and shares that exact same instance with every subsequent class and every subsequent HTTP request until the application shuts down.
                                                                  // You registered TaskRepository as a Singleton. This was a smart architectural choice because it uses static lists (_tasks) to mock a database. If it were Scoped or Transient, those lists might reset between requests, and you'd lose your saved data.
builder.Services.AddScoped<ITaskService, TaskService>(); // It means, if anyone ever asks you for an ITaskService, I want you to build and give them a TaskService (Registration). AddScoped: The DI container creates one instance per HTTP request. If your Controller and your Service both ask for the same dependency during a single web request, they get the exact same instance. Once the web request finishes and sends a response to the user, the instance is destroyed.
                                                         // You registered TaskService as Scoped, which is standard practice for business logic classes.

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection(); // It intercepts any insecure http:// requests and automatically redirects the client to the secure https:// version.
}

app.UseRouting(); // This middleware is responsible for analyzing the incoming URL and figuring out which Controller and Action method should handle it.

app.UseAuthorization(); // This asks the question: "Are you allowed to do this?" (You have this in your code!). Once the user is authenticated, this middleware checks if they have the correct roles or permissions to access the specific endpoint they requested. Crucial Rule: UseAuthentication MUST always come before UseAuthorization. You cannot check what a user is allowed to do if you don't know who they are yet.

app.MapControllers(); // This middleware represents the "Endpoint Middleware." This is the destination. It actually executes your TasksController, runs the business logic, and generates the 200 OK JSON response to send back out through the pipeline.

app.Run();

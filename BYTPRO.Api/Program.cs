using BYTPRO.Api.Middlewares;
using BYTPRO.Core;
using BYTPRO.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDataServices();

builder.Services.AddCoreServices();

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddExceptionHandler<ExceptionHandler>();

builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseExceptionHandler();

app.MapControllers();

await app.RunAsync();
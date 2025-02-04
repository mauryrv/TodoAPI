// program.cs
using Microsoft.AspNetCore.Builder;
using Scalar.AspNetCore;
using TodoAPI.AppDataContext;
using TodoAPI.Middleware;
using TodoAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.Configure<DbSettings>(builder.Configuration.GetSection("DbSettings"));
builder.Services.AddSingleton<TodoDbContext>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>(); 

builder.Services.AddProblemDetails();  

// Adding of login 
builder.Services.AddLogging();  

var app = builder.Build();

{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider;
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUi(opt =>
{
    opt.DocumentPath = "openapi/{documentName}.json";
});
    app.MapScalarApiReference(opt =>
{
    opt.Title = "Scalar Example";
    opt.Theme = ScalarTheme.Mars;
    opt.DefaultHttpClient = new(ScalarTarget.Http, ScalarClient.Http11);
});

}





app.UseHttpsRedirection();

app.UseExceptionHandler();

app.UseAuthorization();

app.MapControllers();

app.Run();
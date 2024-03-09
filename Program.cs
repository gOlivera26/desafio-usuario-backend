using desafio_backend.CQRS.Commands;
using desafio_backend.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using static desafio_backend.CQRS.Commands.PostUsuario;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS Configuration
builder.Services.AddCors(config =>
{
    config.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// AutoMapper
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

// MediatR
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
});

// Add DbContext
builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddScoped<IValidator<PostUsuarioCommand>, PostUsuarioCommandValidator>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// Enable CORS
app.UseCors();

app.MapControllers();

app.Run();

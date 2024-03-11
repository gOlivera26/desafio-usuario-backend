using desafio_backend.CQRS.Commands;
using desafio_backend.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using static desafio_backend.CQRS.Commands.PostRol;
using static desafio_backend.CQRS.Commands.PostUsuario;
using static desafio_backend.CQRS.Commands.PutUsuario;

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
builder.Services.AddScoped<IValidator<PostRolCommand>, PostRolCommandValidator>();
builder.Services.AddScoped<IValidator<PutUsuarioCommand>, PutUsuarioCommandValidator>();
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

    // Agregar un pequeño retraso para permitir que la base de datos se inicie completamente
    Thread.Sleep(5000); // Espera 5 segundos
    try
    {
        dbContext.Database.Migrate();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error al migrar la base de datos: {ex.Message}");
    }
}


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

using Infraestructure.Data;
using Infraestructure.IOC;
using Microsoft.EntityFrameworkCore;
using Application.IOC;
using FluentValidation;
using Application.DTOs.Admin;
using WebAPI.Validation.Admin;
using WebAPI.Validation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//DI
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddAplicationLayer();
builder.Services.AddValidators();
var app = builder.Build();

// Run migrations and create the database if it does not exist
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();

    // Migrate the database automatically
    context.Database.Migrate(); // This will apply any pending migrations
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

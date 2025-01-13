using Infraestructure.Data;
using Infraestructure.IOC;
using Microsoft.EntityFrameworkCore;
using Application.IOC;
using FluentValidation;
using Application.DTOs.Admin;
using WebAPI.Validation.Admin;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//DI
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddAplicationLayer();
builder.Services.AddScoped<IValidator<RegisterRequest>, CreateAdminValidator>();
var app = builder.Build();

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

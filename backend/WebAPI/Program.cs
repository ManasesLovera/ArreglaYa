using Infrastructure.Data;
using Infrastructure.IOC;
using Application.IOC;
using WebAPI.Validation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options =>
{
    // Prevent ASP.NET Core from removing the 'Async' suffix in action method names.
    // This ensures that link generation (e.g., CreatedAtAction) works when using methods named like GetByIdAsync.
    // Recommended when following the async method naming convention across controllers.
    options.SuppressAsyncSuffixInActionNames = false;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Dependency Injection from other layers
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

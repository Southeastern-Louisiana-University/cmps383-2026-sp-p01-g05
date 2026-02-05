using Microsoft.EntityFrameworkCore;
using Selu383.SP26.Api.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add DbContext
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DataContext")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Auto-migrate database on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DataContext>();
    db.Database.Migrate();

    // Ensure seed data exists (for test environments where ClearData() removes it)
    if (!db.Locations.Any())
    {
        db.Locations.AddRange(
            new Selu383.SP26.Api.Entities.Location
            {
                Name = "Caffeinated Lions Downtown",
                Address = "123 Main Street, Hammond, LA 70401",
                TableCount = 15
            },
            new Selu383.SP26.Api.Entities.Location
            {
                Name = "Caffeinated Lions Uptown",
                Address = "456 Oak Avenue, Hammond, LA 70403",
                TableCount = 20
            },
            new Selu383.SP26.Api.Entities.Location
            {
                Name = "Caffeinated Lions Lakeside",
                Address = "789 Lake Drive, Mandeville, LA 70448",
                TableCount = 12
            }
        );
        db.SaveChanges();
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

app.MapControllers();

app.Run();

//see: https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-8.0
// Hi 383 - this is added so we can test our web project automatically
public partial class Program { }
using backend.Data;
using backend.Repositories;

var builder = WebApplication.CreateBuilder(args);

// DB
builder.Services.AddDbContext<AppDbContext>();

// Repositories
builder.Services.AddScoped<IDeskRepository, DeskRepository>();

var app = builder.Build();

// DB Seeding
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    DbSeeder.Seed(dbContext);
}

app.UseHttpsRedirection();

app.MapGet("/", async (IDeskRepository deskRepository) =>
{
    var desks = await deskRepository.GetAllAsync();
    return desks;
});

app.Run();

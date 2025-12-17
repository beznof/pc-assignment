using backend.Data;
using backend.Repositories;

var builder = WebApplication.CreateBuilder(args);

// DB
builder.Services.AddDbContext<AppDbContext>();

// Repositories
builder.Services.AddScoped<IDeskRepository, DeskRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

// DB Seeding
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    DbSeeder.Seed(dbContext);
}

app.UseHttpsRedirection();

app.MapGet("/", async (IUserRepository userRepository) =>
{
    var desks = await userRepository.GetAllAsync();
    return desks;
});

app.Run();

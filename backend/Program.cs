using backend.Data;
using backend.Repositories;

var builder = WebApplication.CreateBuilder(args);

// DB
builder.Services.AddDbContext<AppDbContext>();

// Repositories
builder.Services.AddScoped<IDeskRepository, DeskRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();

var app = builder.Build();

// DB Seeding
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    DbSeeder.Seed(dbContext);
}

app.UseHttpsRedirection();

app.MapGet("/", async (IReservationRepository reservationRepository) =>
{
    var desks = await reservationRepository.GetAllAsync();
    return desks;
});

app.Run();

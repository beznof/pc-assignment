using backend.Data;
using backend.Repositories;
using backend.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// DB
builder.Services.AddDbContext<AppDbContext>();

// Repositories
builder.Services.AddScoped<IDesksRepository, DesksRepository>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IReservationsRepository, ReservationsRepository>();

// Services
builder.Services.AddScoped<IUsersService, UsersService>();

// SwaggerUI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

    options.EnableAnnotations();
});

var app = builder.Build();

// DB Seeding
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    DbSeeder.Seed(dbContext);
}

// Set up SwaggerUI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.MapGet("/", async (IReservationsRepository reservationRepository) =>
{
    var desks = await reservationRepository.GetAllAsync();
    return desks;
});

app.Run();

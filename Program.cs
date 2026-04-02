using UrlShortenerAPI;

var builder = WebApplication.CreateBuilder(args);

// ✅ Add services
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>();

var app = builder.Build();

// ✅ Map controllers
app.MapControllers();

// Ensure DB
using (var db = new AppDbContext())
{
    db.Database.EnsureCreated();
}

app.Run();

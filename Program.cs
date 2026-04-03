using UrlShortenerAPI;

var builder = WebApplication.CreateBuilder(args);

// 🔥 Add CORS (fixes your frontend error)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Controllers + Db
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>();

var app = builder.Build();

// 🔥 Enable CORS here
app.UseCors("AllowAll");

// Map controllers
app.MapControllers();

// Ensure DB
using (var db = new AppDbContext())
{
    db.Database.EnsureCreated();
}

app.Run();
using Microsoft.EntityFrameworkCore;
using TestApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Local SQLite database file (created next to the app)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Test API", Version = "v1",
        Description = "Simple REST API for testing GET, POST, PUT, PATCH, DELETE, HEAD and OPTIONS. No authentication." });

    var xml = Path.Combine(AppContext.BaseDirectory, $"{typeof(Program).Assembly.GetName().Name}.xml");
    if (File.Exists(xml)) c.IncludeXmlComments(xml);
});

// Allow any origin (handy for testing from browsers / other apps)
builder.Services.AddCors(o => o.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

// Create DB + seed data on startup
using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.EnsureCreated();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Test API v1");
    c.RoutePrefix = string.Empty; // Swagger UI at http://localhost:5000/
});

app.UseCors();
app.MapControllers();

app.Run();

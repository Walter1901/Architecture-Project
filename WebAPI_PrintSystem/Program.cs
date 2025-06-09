using Microsoft.EntityFrameworkCore;
using WebAPI_PrintSystem.Services;
using PrintSystem.DAL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add logging
builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
    logging.AddDebug();
});

// Configuration HttpClient
builder.Services.AddHttpClient();

// Services - IMPORTANT: Utilisent maintenant Entity Framework
builder.Services.AddScoped<ISqlService, SqlService>();
builder.Services.AddScoped<IPaymentDBService, PaymentDBService>();
builder.Services.AddScoped<IADService, ADService>();
builder.Services.AddScoped<ISAPHRService, SAPHRService>();

// Database Context - Entity Framework avec SQL Server
builder.Services.AddDbContext<PrintSystemContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);

    // Enable sensitive data logging in development
    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
        options.EnableDetailedErrors();
    }
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMVC", policy =>
    {
        policy.WithOrigins("https://localhost:7226", "http://localhost:5102")
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Database initialization - CORRIGER L'INITIALISATION
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();

    try
    {
        var context = services.GetRequiredService<PrintSystemContext>();

        logger.LogInformation("Initializing database...");

        // Créer la base de données si elle n'existe pas
        await context.Database.EnsureCreatedAsync();

        logger.LogInformation("Database initialized successfully");

    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while initializing the database");
        throw; // Re-throw pour arrêter l'application si la DB ne fonctionne pas
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowMVC");
app.UseAuthorization();
app.MapControllers();

app.Run();
using Microsoft.EntityFrameworkCore;
using WebAPI_PrintSystem.Services;
using PrintSystem.DAL;

var builder = WebApplication.CreateBuilder(args);

// ===== SERVICE REGISTRATION =====
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure logging for debugging and monitoring
builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
    logging.AddDebug();
});

// Register HttpClient for external API calls
builder.Services.AddHttpClient();

// ===== APPLICATION SERVICES =====
// Register business logic services with scoped lifetime

// SQL Service: Handles database operations for quotas and user balances
builder.Services.AddScoped<ISqlService, SqlService>();

// Payment Service: Manages payment transactions and logging
builder.Services.AddScoped<IPaymentDBService, PaymentDBService>();

// Active Directory Service: Use full namespace to avoid ambiguity
builder.Services.AddScoped<PrintSystem.Models.Interfaces.IADService, ADService>();

// SAP HR Service: Use full namespace to avoid ambiguity
builder.Services.AddScoped<PrintSystem.Models.Interfaces.ISAPHRService, SAPHRService>();

// ===== DATABASE CONFIGURATION =====
builder.Services.AddDbContext<PrintSystemContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);

    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
        options.EnableDetailedErrors();
    }
});

// ===== CORS CONFIGURATION =====
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMVC", policy =>
    {
        policy.WithOrigins("https://localhost:7226", "http://localhost:5102")
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// ===== APPLICATION BUILD =====
var app = builder.Build();

// ===== DATABASE INITIALIZATION =====
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();

    try
    {
        var context = services.GetRequiredService<PrintSystemContext>();
        logger.LogInformation("Initializing database...");
        await context.Database.EnsureCreatedAsync();
        logger.LogInformation("Database initialized successfully");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while initializing the database");
        throw;
    }
}

// ===== MIDDLEWARE PIPELINE CONFIGURATION =====
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowMVC");
app.UseAuthorization();
app.MapControllers();

// ===== APPLICATION STARTUP =====
app.Run();
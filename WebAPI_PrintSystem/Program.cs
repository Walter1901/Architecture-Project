using Microsoft.EntityFrameworkCore;
using WebAPI_PrintSystem.Services;
using PrintSystem.DAL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuration HttpClient
builder.Services.AddHttpClient();

// Services
builder.Services.AddScoped<ISqlService, SqlService>();
builder.Services.AddScoped<IPaymentDBService, PaymentDBService>();
builder.Services.AddScoped<IADService, ADService>();
builder.Services.AddScoped<ISAPHRService, SAPHRService>();

// Database Context - MYSQL au lieu de SQL Server
builder.Services.AddDbContext<PrintSystemContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 0))));

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

// Database initialization
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<PrintSystemContext>();
    dbContext.Database.EnsureCreated();
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
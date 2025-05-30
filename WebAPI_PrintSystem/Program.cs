using Microsoft.EntityFrameworkCore;
using WebAPI_PrintSystem.Services;
using PrintSystem.DAL;
using PrintSystem.BLL.Interfaces;
using PrintSystem.BLL.Services;
using PrintSystem.DAL.Interfaces;
using PrintSystem.DAL.Repositories;
using PrintSystem.Models.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Legacy Services - kept for backward compatibility
builder.Services.AddScoped<ISqlService, SqlService>();
builder.Services.AddScoped<IPaymentDBService, PaymentDBService>();

// Business Logic Layer Services
builder.Services.AddScoped<IQuotaService, QuotaService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IUserService, UserService>();

// Data Access Layer Repositories
builder.Services.AddScoped<IQuotaRepository>(provider =>
    new QuotaRepository(provider.GetService<IConfiguration>()
        .GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IPaymentRepository>(provider =>
    new PaymentRepository(provider.GetService<IConfiguration>()
        .GetConnectionString("DefaultConnection")));

// External Services
builder.Services.AddScoped<PrintSystem.Models.Interfaces.IADService, ADService>();
builder.Services.AddScoped<PrintSystem.Models.Interfaces.ISAPHRService, SAPHRService>();

// Database Context
builder.Services.AddDbContext<PrintSystemContext>(options =>
    options.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=PrintSystemDB;Trusted_Connection=True;"));

// HTTP Client for SAP HR Service
builder.Services.AddHttpClient<PrintSystem.Models.Interfaces.ISAPHRService, SAPHRService>();

// CORS policy for MVC application
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMVC", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});


var app = builder.Build();

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<PrintSystemContext>();
    dbContext.Database.EnsureCreated();
}

// Configure development environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure HTTP pipeline
app.UseHttpsRedirection();
app.UseCors("AllowMVC");
app.UseAuthorization();
app.MapControllers();

app.Run();
using Microsoft.EntityFrameworkCore;
using WebAPI_PrintSystem.Services;
using PrintSystem.DAL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Custom Services
builder.Services.AddScoped<ISqlService, SqlService>();
builder.Services.AddScoped<IPaymentDBService, PaymentDBService>();
builder.Services.AddScoped<IADService, ADService>();

builder.Services.AddDbContext<PrintSystemContext>(options =>
    options.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=PrintSystemDB;Trusted_Connection=True;"));

// HTTP Client for SAP HR
builder.Services.AddHttpClient<ISAPHRService, SAPHRService>();
builder.Services.AddScoped<ISAPHRService, SAPHRService>();

// CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMVC", policy =>
    {
        policy.WithOrigins("https://localhost:7001", "http://localhost:5001")
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

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
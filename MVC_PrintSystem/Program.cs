using MVC_PrintSystem.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient<IWebAPIService, WebAPIService>(client =>
{
    var baseUrl = builder.Configuration["WebAPI:BaseUrl"] ?? "https://localhost:7000/";
    client.BaseAddress = new Uri(baseUrl);
});

builder.Services.AddScoped<IWebAPIService, WebAPIService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
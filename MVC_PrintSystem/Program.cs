using MVC_PrintSystem.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient<IWebAPIService, WebAPIService>(client =>
{
    var baseUrl = builder.Configuration["WebAPI:BaseUrl"] ?? "https://localhost:7000/";
    client.BaseAddress = new Uri(baseUrl);
});

builder.Services.AddScoped<IWebAPIService, WebAPIService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IStudentHomeServices, StudentHomeService>();
builder.Services.AddScoped<IStudentPrintPaymentServices, StudentPrintPaymentService>();
builder.Services.AddScoped<IFacultyHomeServices, FacultyHomeService>();
builder.Services.AddScoped<IFacultyStudentPrintPaymentServices, FacultyStudentPrintPaymentService>();
builder.Services.AddScoped<IStudentsListServices, StudentsListService>();

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

app.MapControllerRoute(
    name: "facultyManagement",
    pattern: "{controller=FacultyManagement}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "users",
    pattern: "{controller=Users}/{action=GetUsername}/{id?}");

app.MapControllerRoute(
    name: "quota",
    pattern: "{controller=Quota}/{action=AddQuota}/{id?}");

app.MapControllerRoute(
    name: "payment",
    pattern: "{controller=Payment}/{action=ProcessPayment}/{id?}");

app.MapControllerRoute(
    name: "login",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
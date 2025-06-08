using MVC_PrintSystem.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();

// Configure session with custom options
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20); // Session expires after 20 minutes of inactivity
    options.Cookie.HttpOnly = true; // Cookie is only accessible via HTTP (not JavaScript)
    options.Cookie.IsEssential = true; // Cookie is essential for the application
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; // Use HTTPS in production
    options.Cookie.SameSite = SameSiteMode.Strict; // CSRF protection
});


builder.Services.AddScoped<IWebAPIService, WebAPIService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IStudentHomeServices, StudentHomeService>();
builder.Services.AddHttpClient<IStudentPrintPaymentServices, StudentPrintPaymentService>();
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
app.UseSession();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

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
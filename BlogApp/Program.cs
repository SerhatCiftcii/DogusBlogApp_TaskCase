using BlogApp.Models.Context;
using BlogApp.Models.Repositories;
using BlogApp.Services;
using BlogApp.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Veritabaný baðlantýsýný yapýlandýrma
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repository'leri DI'a kaydetme
builder.Services.AddScoped<IBlogRepository, BlogRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>(); // Eðer varsa

// Service'leri DI'a kaydetme
builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICommentService, CommentService>();

// Authentication middleware yapýlandýrmasý
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login"; // Giriþ yapmamýþ kullanýcýlarýn yönlendirileceði sayfa
        options.LogoutPath = "/Auth/Logout"; // Çýkýþ yapýldýðýnda yönlendirilecek sayfa
        options.AccessDeniedPath = "/Auth/AccessDenied"; // Yetkisiz eriþim durumunda yönlendirilecek sayfa (isteðe baðlý)
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Cookie'nin geçerlilik süresi (isteðe baðlý)
        options.SlidingExpiration = true; // Kullanýcý aktif oldukça cookie süresini uzat (isteðe baðlý)
        options.Cookie.HttpOnly = true; // JavaScript'in cookie'ye eriþimini engelle (güvenlik için önemli)
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; // HTTPS üzerinde çalýþýrken güvenli cookie (önerilir)
        options.Cookie.SameSite = SameSiteMode.Strict; // Siteler arasý isteklerde cookie gönderimini kýsýtla (güvenlik için önemli)
        options.Cookie.Name = ".BlogAppCookie"; // Cookie'nin adý (isteðe baðlý)
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Kimlik doðrulama middleware'ini ekle
app.UseAuthorization(); // Yetkilendirme middleware'ini ekle

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Blog}/{action=Index}/{id?}"); // Anasayfa controller'ý Blog olarak deðiþtirildi

app.Run();
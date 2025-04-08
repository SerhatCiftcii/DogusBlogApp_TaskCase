using BlogApp.Models.Context;
using BlogApp.Models.Repositories;
using BlogApp.Services;
using BlogApp.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Veritaban� ba�lant�s�n� yap�land�rma
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repository'leri DI'a kaydetme
builder.Services.AddScoped<IBlogRepository, BlogRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>(); // E�er varsa

// Service'leri DI'a kaydetme
builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICommentService, CommentService>();

// Authentication middleware yap�land�rmas�
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login"; // Giri� yapmam�� kullan�c�lar�n y�nlendirilece�i sayfa
        options.LogoutPath = "/Auth/Logout"; // ��k�� yap�ld���nda y�nlendirilecek sayfa
        options.AccessDeniedPath = "/Auth/AccessDenied"; // Yetkisiz eri�im durumunda y�nlendirilecek sayfa (iste�e ba�l�)
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Cookie'nin ge�erlilik s�resi (iste�e ba�l�)
        options.SlidingExpiration = true; // Kullan�c� aktif olduk�a cookie s�resini uzat (iste�e ba�l�)
        options.Cookie.HttpOnly = true; // JavaScript'in cookie'ye eri�imini engelle (g�venlik i�in �nemli)
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; // HTTPS �zerinde �al���rken g�venli cookie (�nerilir)
        options.Cookie.SameSite = SameSiteMode.Strict; // Siteler aras� isteklerde cookie g�nderimini k�s�tla (g�venlik i�in �nemli)
        options.Cookie.Name = ".BlogAppCookie"; // Cookie'nin ad� (iste�e ba�l�)
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

app.UseAuthentication(); // Kimlik do�rulama middleware'ini ekle
app.UseAuthorization(); // Yetkilendirme middleware'ini ekle

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Blog}/{action=Index}/{id?}"); // Anasayfa controller'� Blog olarak de�i�tirildi

app.Run();
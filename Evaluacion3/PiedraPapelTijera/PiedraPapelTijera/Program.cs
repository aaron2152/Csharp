
using PiedraPapelTijera.Data;
using PiedraPapelTijera.Services;

var builder = WebApplication.CreateBuilder(args);

// Servicios MVC
builder.Services.AddControllersWithViews();

// Supabase
builder.Services.AddSingleton<SupabaseCliente>();

// Servicios del juego
builder.Services.AddScoped<JuegoServicio>();
builder.Services.AddScoped<RankingServicio>();

// Configuración de sesiones
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(opciones =>
{
    opciones.IdleTimeout = TimeSpan.FromMinutes(30);
    opciones.Cookie.HttpOnly = true;
    opciones.Cookie.IsEssential = true;
    opciones.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
});

var app = builder.Build();

// Configuración del entorno
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

// Habilitar sesiones
app.UseSession();

app.UseAuthorization();

// Archivos estáticos
app.MapStaticAssets();

// Ruta inicial del juego
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Juego}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();

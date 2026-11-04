using Canchas.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// La base de datos se elige por configuración: "MySql" (tu computadora) o "Supabase" (internet).
string proveedor = builder.Configuration["BaseDeDatos:Proveedor"] ?? "MySql";

if (proveedor == "Supabase")
    builder.Services.AddSingleton<IRepositorio, SupabaseRepositorio>();
else
    builder.Services.AddSingleton<IRepositorio, MySqlRepositorio>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Reservas}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
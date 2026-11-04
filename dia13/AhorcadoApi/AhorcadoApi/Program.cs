using AhorcadoApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Registrar controladores
builder.Services.AddControllers();

// Registrar conexión con Supabase
builder.Services.AddHttpClient<SupabaseService>();

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS para permitir futuras conexiones desde un frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AhorcadoCors", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AhorcadoCors");

app.UseAuthorization();

app.MapControllers();

app.Run();
using Microsoft.EntityFrameworkCore;
using Narcisa.BD.Datos;
using Narcisa.BD.Datos.Entity;
using Narcisa.Server;
using Narcisa.Server.Client.Pages;
using Narcisa.Server.Components;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("No existe la conexion con la bd.");

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

// Repositorios (Patrón de la cátedra)
builder.Services.AddScoped<Narcisa.Repositorio.Repositorios.IProductoRepositorio, Narcisa.Repositorio.Repositorios.ProductoRepositorio>();
builder.Services.AddScoped<Narcisa.Repositorio.Repositorios.IPedidoRepositorio, Narcisa.Repositorio.Repositorios.PedidoRepositorio>();
builder.Services.AddScoped<Narcisa.Repositorio.Repositorios.IPagoRepositorio, Narcisa.Repositorio.Repositorios.PagoRepositorio>();
builder.Services.AddScoped<Narcisa.Repositorio.Repositorios.IUsuarioRepositorio, Narcisa.Repositorio.Repositorios.UsuarioRepositorio>();
builder.Services.AddScoped<Narcisa.Repositorio.Repositorios.IProyectoRepositorio, Narcisa.Repositorio.Repositorios.ProyectoRepositorio>();

// Servicio HTTP y Estado para componentes Blazor
builder.Services.AddScoped<Narcisa.Servicios.ServicioHTTP.IHttpService, Narcisa.Servicios.ServicioHTTP.HttpService>();
builder.Services.AddScoped<Narcisa.Servicios.CarritoStateService>();
builder.Services.AddScoped<Narcisa.Servicios.AuthStateService>();
builder.Services.AddScoped(sp =>
{
    var nav = sp.GetService<Microsoft.AspNetCore.Components.NavigationManager>();
    var baseUri = nav?.BaseUri ?? "https://localhost:7158/";
    return new HttpClient { BaseAddress = new Uri(baseUri) };
});

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
    Seed.Iniciar(db);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseStaticFiles();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Narcisa.Server.Client._Imports).Assembly);

app.Run();

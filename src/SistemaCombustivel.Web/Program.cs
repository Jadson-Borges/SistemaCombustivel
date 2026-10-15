using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using SistemaCombustivel.Infrastructure.Identity;
using SistemaCombustivel.Web.Components;
using Microsoft.EntityFrameworkCore;
using SistemaCombustivel.Infrastructure.Data;
using SistemaCombustivel.Application.Abstractions;
using SistemaCombustivel.Application.Services;
using SistemaCombustivel.Infrastructure.Repositories;
using SistemaCombustivel.Application.Common;
using SistemaCombustivel.Domain.Entities;
using SistemaCombustivel.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddCascadingAuthenticationState();

//Adicição do Contexto do projeto!
builder.Services.AddDbContext<AppDbContext>
    (options => options.UseSqlServer
    (builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase= true;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;

    options.User.RequireUniqueEmail = true;

    })
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/login";
    options.AccessDeniedPath = "/acesso-negado";
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Autorizacao.PoiticaAdministrador, policy =>
    policy.RequireClaim(Autorizacao.ClaimTipoUsuario, TipoUsuario.Administrador.ToString()));
});

builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationUserClaimsPrincipalFactory>();

//(Application + Infrastructure)
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IServicoAutenticacao, ServicoAutenticacao>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<RegistroUsuarioService>();
builder.Services.AddScoped<LoginUsuarioService>();
builder.Services.AddScoped<AdministradorInicialService>();
builder.Services.ConfigurarInfra();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var email = app.Configuration["Seed:Admin:Email"];
    var senha = app.Configuration["Seed:Admin:Senha"];

    if (!string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(senha))
    {
        var nome = app.Configuration["Seed:Admin:Nome"] ?? "Administrador";
        var servico = scope.ServiceProvider.GetRequiredService<AdministradorInicialService>();
        var resultado = await servico.GarantirAsync(nome, email, senha);

        if (!resultado.Sucesso)
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            logger.LogError("Falha ao criar o administrador inicial: {Erro}", resultado.Erro);
        }
    }
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

//conexão usuarioS
app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(SistemaCombustivel.Web.Client._Imports).Assembly);

app.MapPost("/logout", async (IServicoAutenticacao servicoAutenticacao, [FromForm] string returnUrl) =>
{
    await servicoAutenticacao.LogoutAsync();
    return Results.LocalRedirect($"~/{returnUrl}");
}).RequireAuthorization();

app.Run();
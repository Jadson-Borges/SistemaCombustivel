using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaCombustivel.Application;
using SistemaCombustivel.Application.Common;
using SistemaCombustivel.Application.Usuarios.Commands.CriarAdministradorInicial;
using SistemaCombustivel.Application.Usuarios.Commands.LogoutUsuario;
using SistemaCombustivel.Domain.Entities;
using SistemaCombustivel.Infrastructure;
using SistemaCombustivel.Infrastructure.Data;
using SistemaCombustivel.Infrastructure.Identity;
using SistemaCombustivel.Infrastructure.Repositories;
using SistemaCombustivel.Web.Components;

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
        options.Password.RequireLowercase = true;
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
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
         .RequireAuthenticatedUser()
         .Build();

    options.AddPolicy(Autorizacao.PoliticaAdministrador, policy =>
        policy.RequireClaim(Autorizacao.ClaimTipoUsuario, TipoUsuario.Administrador.ToString()));
});

builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationUserClaimsPrincipalFactory>();

//(Application + Infrastructure)
builder.Services.ConfigurarApplication();
builder.Services.ConfigurarInfra();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    //verificar nome depois pois não acho que esteja certo
    var nome = app.Configuration["Seed:Admin:Nome"] ?? "Administrador";
    var email = app.Configuration["Seed:Admin:Email"];
    var senha = app.Configuration["Seed:Admin:Senha"];


    if (!string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(senha))
    {

        var handler = scope.ServiceProvider.GetRequiredService<CriarAdministradorInicialCommandHandler>();
        var resultado = await handler.Handle(
    new CriarAdministradorInicialCommand(nome, email, senha), app.Lifetime.ApplicationStopping);

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

app.MapStaticAssets()
    .AllowAnonymous();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(SistemaCombustivel.Web.Client._Imports).Assembly);

app.MapPost("/logout", async (LogoutUsuarioCommandHandler handler, [FromForm] string returnUrl, CancellationToken c) =>
{
    await handler.Handle(new LogoutUsuarioCommand(), c);
    return Results.LocalRedirect($"~/{returnUrl}");
}).RequireAuthorization();

app.Run();
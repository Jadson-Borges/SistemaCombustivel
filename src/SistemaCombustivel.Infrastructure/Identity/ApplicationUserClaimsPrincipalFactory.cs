using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SistemaCombustivel.Application.Abstractions;
using System.Security.Claims;


namespace SistemaCombustivel.Infrastructure.Identity
{
    public class ApplicationUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public ApplicationUserClaimsPrincipalFactory(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<IdentityOptions> options,
            IUsuarioRepository usuarioRepository)
            : base(userManager, roleManager, options)
        {
            _usuarioRepository = usuarioRepository;
        }

        public override async Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
        {
            var principal = await base.CreateAsync(user);
            var identity = (ClaimsIdentity)principal.Identity!;

            var usuario = await _usuarioRepository.ObterPorIdAsync(user.UsuarioId);

            if (usuario is not null)
            {
                identity.AddClaim(new Claim("TipoUsuario", usuario.Tipo.ToString()));
                identity.AddClaim(new Claim("UsuarioId", usuario.Id.ToString()));
            }

            return principal;
        }
    }
}
using Microsoft.AspNetCore.Identity;
using SistemaCombustivel.Application.Abstractions;

namespace SistemaCombustivel.Infrastructure.Identity
{
    public class ServicoAutenticacao : IServicoAutenticacao
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ServicoAutenticacao(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ResultadoAutenticacao> RegistrarAsync(string email, string senha, int usuarioId)
        {
            var applicationUser = new ApplicationUser
            {
                UserName = email,
                Email = email,
                UsuarioId = usuarioId
            };

            var resultado = await _userManager.CreateAsync(applicationUser, senha);

            if (!resultado.Succeeded)
            {
                var erros = string.Join("; ", resultado.Errors.Select(e => e.Description));
                return ResultadoAutenticacao.Falha(erros);
            }

            return ResultadoAutenticacao.Ok(applicationUser.Id);
        }
    }
}
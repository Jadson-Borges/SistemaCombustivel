using Microsoft.AspNetCore.Identity;
using SistemaCombustivel.Application.Abstractions;

namespace SistemaCombustivel.Infrastructure.Identity
{
    public class ServicoAutenticacao : IServicoAutenticacao
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ServicoAutenticacao(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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

        public async Task<ResultadoAutenticacao> LoginAsync(string email, string senha)
        {
            var resultado = await _signInManager.PasswordSignInAsync(
                email, senha, isPersistent: false, lockoutOnFailure: true);

            if (!resultado.Succeeded)
            {
                var erro = resultado.IsLockedOut
                    ? "Conta bloqueada temporariamente por excesso de tentativas."
                    : "E-mail ou senha invalidos.";
                return ResultadoAutenticacao.Falha(erro);
            }

            return ResultadoAutenticacao.Ok(string.Empty);
        }
    }
}
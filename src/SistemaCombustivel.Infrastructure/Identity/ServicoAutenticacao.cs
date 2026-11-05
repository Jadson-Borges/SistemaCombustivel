using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SistemaCombustivel.Application.Abstractions;
using SistemaCombustivel.Application.Common;

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

        public async Task<Resultado<int>> VerificarCredenciaisAsync(string email, string senha)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return Resultado<int>.Falha("E-mail ou senha invalidos.");

            var resultado = await _signInManager.CheckPasswordSignInAsync(user, senha, lockoutOnFailure: true);

            if (!resultado.Succeeded)
            {
                var erro = resultado.IsLockedOut
                    ? "Conta bloqueada temporariamente por excesso de tentativas."
                    : "E-mail ou senha invalidos.";
                return Resultado<int>.Falha(erro);
            }

            return Resultado<int>.Ok(user.UsuarioId);
        }

        public async Task EntrarAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email)
                ?? throw new InvalidOperationException("Usuário não encontrado para iniciar a sessão.");

            await _signInManager.SignInAsync(user, isPersistent: false);
        }

        public async Task InvalidarSessoesAsync(int usuarioId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UsuarioId == usuarioId);
            if (user is not null)
                await _userManager.UpdateSecurityStampAsync(user);
        }

        public async Task LogoutAsync() => await _signInManager.SignOutAsync();
    }
}
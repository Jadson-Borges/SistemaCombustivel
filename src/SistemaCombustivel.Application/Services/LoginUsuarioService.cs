using SistemaCombustivel.Application.Abstractions;
using SistemaCombustivel.Application.Common;

namespace SistemaCombustivel.Application.Services
{
    public class LoginUsuarioService
    {
        private readonly IServicoAutenticacao _servicoAutenticacao;

        public LoginUsuarioService(IServicoAutenticacao servicoAutenticacao)
        {
            _servicoAutenticacao = servicoAutenticacao;
        }

        public async Task<Resultado> LoginAsync(string email, string senha)
        {
            var resultado = await _servicoAutenticacao.LoginAsync(email, senha);

            return resultado.Sucesso
                ? Resultado.Ok()
                : Resultado.Falha(resultado.Erro!);
        }
    }
}

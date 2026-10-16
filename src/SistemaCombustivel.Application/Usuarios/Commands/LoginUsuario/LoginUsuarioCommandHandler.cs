using SistemaCombustivel.Application.Abstractions;
using SistemaCombustivel.Application.Common;

namespace SistemaCombustivel.Application.Usuarios.Commands.LoginUsuario;

public class LoginUsuarioCommandHandler(IServicoAutenticacao servicoAutenticacao)
{
    public async Task<Resultado> Handle(LoginUsuarioCommand command, CancellationToken c)
    {
        var resultado = await servicoAutenticacao.LoginAsync(command.Email, command.Senha);

        return resultado.Sucesso
            ? Resultado.Ok()
            : Resultado.Falha(resultado.Erro!);
    }
}
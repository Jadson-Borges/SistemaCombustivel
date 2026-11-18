using SistemaCombustivel.Application.Abstractions;
using SistemaCombustivel.Application.Common;

namespace SistemaCombustivel.Application.Usuarios.Commands.ReativarUsuario;

public class ReativarUsuarioCommandHandler(IUsuarioRepository usuarioRepository)
{
    public async Task<Resultado> Handle(ReativarUsuarioCommand command, CancellationToken c)
    {
        var usuario = await usuarioRepository.ObterPorIdAsync(command.UsuarioId, c);
        if (usuario is null)
            return Resultado.Falha("Usuário não encontrado.");
        if (usuario.Ativo)
            return Resultado.Falha("O usuário já está ativo.");

        usuario.Reativar();
        await usuarioRepository.SalvarAlteracoesAsync(c);
        return Resultado.Ok();
    }
}
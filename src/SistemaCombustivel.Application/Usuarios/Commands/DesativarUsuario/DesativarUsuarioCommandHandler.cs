using SistemaCombustivel.Application.Abstractions;
using SistemaCombustivel.Application.Common;
using SistemaCombustivel.Domain.Entities;

namespace SistemaCombustivel.Application.Usuarios.Commands.DesativarUsuario;

public class DesativarUsuarioCommandHandler(
    IUsuarioRepository usuarioRepository,
    IServicoAutenticacao servicoAutenticacao)
{
    public async Task<Resultado> Handle(DesativarUsuarioCommand command, CancellationToken c)
    {
        var usuario = await usuarioRepository.ObterPorIdAsync(command.UsuarioId, c);
        if (usuario is null)
            return Resultado.Falha("Usuário não encontrado.");
        if (!usuario.Ativo)
            return Resultado.Falha("O usuário já está inativo.");

        // Regra de segurança: o sistema nunca pode ficar sem administrador ativo.
        if (usuario.Tipo == TipoUsuario.Administrador
            && await usuarioRepository.ContarAdministradoresAtivosAsync(c) <= 1)
            return Resultado.Falha("Não é possível desativar o único administrador ativo do sistema.");

        usuario.Desativar();
        await usuarioRepository.SalvarAlteracoesAsync(c);

        // Derruba as sessões abertas dessa pessoa (security stamp).
        await servicoAutenticacao.InvalidarSessoesAsync(usuario.Id);

        return Resultado.Ok();
    }
}
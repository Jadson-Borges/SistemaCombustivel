using SistemaCombustivel.Application.Abstractions;
using SistemaCombustivel.Application.Common;
using SistemaCombustivel.Domain.Entities;

namespace SistemaCombustivel.Application.Usuarios.Commands.CriarUsuario;

public class CriarUsuarioCommandHandler(
    IUsuarioRepository usuarioRepository,
    IServicoAutenticacao servicoAutenticacao,
    IUnitOfWork unitOfWork)
{
    public async Task<Resultado> Handle(CriarUsuarioCommand command, CancellationToken c)
    {
        if (await usuarioRepository.ExisteEmailAsync(command.Email, c))
            return Resultado.Falha("Já existe um usuário cadastrado com este e-mail.");

        string? erroDeAutenticacao = null;

        try
        {
            await unitOfWork.ExecutarEmTransacaoAsync(async () =>
            {
                var usuario = new Usuario(command.NomeUsuario, command.Email, command.Tipo);

                await usuarioRepository.AdicionarAsync(usuario, c);
                await usuarioRepository.SalvarAlteracoesAsync(c);

                var resultadoAuth = await servicoAutenticacao.RegistrarAsync(
                    command.Email, command.Senha, usuario.Id);   // o Identity não aceita token

                if (!resultadoAuth.Sucesso)
                {
                    erroDeAutenticacao = resultadoAuth.Erro;
                    throw new FalhaNoRegistroException();   // força o rollback da transação
                }
            }, c);
        }
        catch (FalhaNoRegistroException)
        {
            return Resultado.Falha(erroDeAutenticacao!);
        }

        return Resultado.Ok();
    }

    // Exceção interna: só sinaliza "desfaça a transação".
    private sealed class FalhaNoRegistroException : Exception { }
}
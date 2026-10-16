using SistemaCombustivel.Application.Abstractions;
using SistemaCombustivel.Application.Common;
using SistemaCombustivel.Domain.Entities;

namespace SistemaCombustivel.Application.Usuarios.Commands.CriarUsuario;

public class CriarUsuarioCommandHandler(
    IUsuarioRepository usuarioRepository,
    IServicoAutenticacao servicoAutenticacao,
    IUnitOfWork unitOfWork)
{
    // PENDÊNCIA: o CancellationToken ainda não é repassado, pois as interfaces atuais não o aceitam.
    public async Task<Resultado> Handle(CriarUsuarioCommand command, CancellationToken c)
    {
        if (await usuarioRepository.ExisteEmailAsync(command.Email))
            return Resultado.Falha("Já existe um usuário cadastrado com este e-mail.");

        string? erroDeAutenticacao = null;

        try
        {
            await unitOfWork.ExecutarEmTransacaoAsync(async () =>
            {
                var usuario = new Usuario(command.NomeUsuario, command.Email, command.Tipo);

                await usuarioRepository.AdicionarAsync(usuario);
                await usuarioRepository.SalvarAlteracoesAsync();

                var resultadoAuth = await servicoAutenticacao.RegistrarAsync(
                    command.Email, command.Senha, usuario.Id);

                if (!resultadoAuth.Sucesso)
                {
                    erroDeAutenticacao = resultadoAuth.Erro;
                    throw new FalhaNoRegistroException();   // força o rollback da transação
                }
            });
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
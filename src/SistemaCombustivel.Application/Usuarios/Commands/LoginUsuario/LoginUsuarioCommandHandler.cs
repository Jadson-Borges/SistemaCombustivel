using SistemaCombustivel.Application.Abstractions;
using SistemaCombustivel.Application.Common;

namespace SistemaCombustivel.Application.Usuarios.Commands.LoginUsuario;

public class LoginUsuarioCommandHandler(
    IServicoAutenticacao servicoAutenticacao,
    IUsuarioRepository usuarioRepository)
{
    public const string MensagemUsuarioInativo =
        "Login não efetuado. Usuário está inativo, entre em contato com o suporte.";

    public async Task<Resultado> Handle(LoginUsuarioCommand command, CancellationToken c)
    {
        // 1) Confere e-mail e senha, sem abrir sessão ainda.
        var credenciais = await servicoAutenticacao.VerificarCredenciaisAsync(command.Email, command.Senha);
        if (!credenciais.Sucesso)
            return Resultado.Falha(credenciais.Erro!);

        // 2) Só quem acertou a senha descobre que a conta está inativa.
        var usuario = await usuarioRepository.ObterPorIdAsync(credenciais.Valor, c);
        if (usuario is null)
            return Resultado.Falha("E-mail ou senha invalidos.");
        if (!usuario.Ativo)
            return Resultado.Falha(MensagemUsuarioInativo);

        // 3) Tudo certo: agora sim grava o cookie.
        await servicoAutenticacao.EntrarAsync(command.Email);
        return Resultado.Ok();
    }
}
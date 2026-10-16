using SistemaCombustivel.Application.Abstractions;

namespace SistemaCombustivel.Application.Tests.Fakes;

public class FakeServicoAutenticacao : IServicoAutenticacao
{
    public bool FalharNoRegistro { get; set; }
    public string MensagemDeErro { get; set; } = "Senha inválida.";

    // O que foi registrado (para o teste conferir depois).
    public List<(string Email, string Senha, int UsuarioId)> Registros { get; } = new();

    public Task<ResultadoAutenticacao> RegistrarAsync(string email, string senha, int usuarioId)
    {
        if (FalharNoRegistro)
            return Task.FromResult(ResultadoAutenticacao.Falha(MensagemDeErro));

        Registros.Add((email, senha, usuarioId));
        return Task.FromResult(ResultadoAutenticacao.Ok("id-do-identity-fake"));
    }

    // Não usados por estes testes: se alguém chamar, o teste quebra e avisa.
    public Task<ResultadoAutenticacao> LoginAsync(string email, string senha) =>
        throw new NotImplementedException();

    public Task LogoutAsync() => throw new NotImplementedException();
}
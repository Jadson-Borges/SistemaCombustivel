using SistemaCombustivel.Application.Abstractions;

namespace SistemaCombustivel.Application.Tests.Fakes;

public class FakeServicoAutenticacao : IServicoAutenticacao
{
    // Registro
    public bool FalharNoRegistro { get; set; }
    public string MensagemDeErro { get; set; } = "Senha inválida.";
    public List<(string Email, string Senha, int UsuarioId)> Registros { get; } = new();

    // Login
    public bool FalharNoLogin { get; set; }
    public string MensagemDeErroLogin { get; set; } = "E-mail ou senha inválidos.";
    public List<(string Email, string Senha)> Logins { get; } = new();

    // Logout
    public int Logouts { get; private set; }

    public Task<ResultadoAutenticacao> RegistrarAsync(string email, string senha, int usuarioId)
    {
        if (FalharNoRegistro)
            return Task.FromResult(ResultadoAutenticacao.Falha(MensagemDeErro));

        Registros.Add((email, senha, usuarioId));
        return Task.FromResult(ResultadoAutenticacao.Ok("id-do-identity-fake"));
    }

    public Task<ResultadoAutenticacao> LoginAsync(string email, string senha)
    {
        if (FalharNoLogin)
            return Task.FromResult(ResultadoAutenticacao.Falha(MensagemDeErroLogin));

        Logins.Add((email, senha));
        return Task.FromResult(ResultadoAutenticacao.Ok("id-do-identity-fake"));
    }

    public Task LogoutAsync()
    {
        Logouts++;
        return Task.CompletedTask;
    }
}
using SistemaCombustivel.Application.Abstractions;
using SistemaCombustivel.Application.Common;

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
    public int UsuarioIdDoLogin { get; set; } = 1;                 
    public List<string> Entradas { get; } = new();                 
    public List<int> SessoesInvalidadas { get; } = new();          

    // Logout
    public int Logouts { get; private set; }

    public Task<ResultadoAutenticacao> RegistrarAsync(string email, string senha, int usuarioId)
    {
        if (FalharNoRegistro)
            return Task.FromResult(ResultadoAutenticacao.Falha(MensagemDeErro));

        Registros.Add((email, senha, usuarioId));
        return Task.FromResult(ResultadoAutenticacao.Ok("id-do-identity-fake"));
    }

    // novo (substitui o antigo LoginAsync)
    public Task<Resultado<int>> VerificarCredenciaisAsync(string email, string senha)
    {
        if (FalharNoLogin)
            return Task.FromResult(Resultado<int>.Falha(MensagemDeErroLogin));

        Logins.Add((email, senha));
        return Task.FromResult(Resultado<int>.Ok(UsuarioIdDoLogin));
    }

    // novo
    public Task EntrarAsync(string email)
    {
        Entradas.Add(email);
        return Task.CompletedTask;
    }

    // novo
    public Task InvalidarSessoesAsync(int usuarioId)
    {
        SessoesInvalidadas.Add(usuarioId);
        return Task.CompletedTask;
    }

    public Task LogoutAsync()
    {
        Logouts++;
        return Task.CompletedTask;
    }
}
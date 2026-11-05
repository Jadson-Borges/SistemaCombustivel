using SistemaCombustivel.Application.Common;

namespace SistemaCombustivel.Application.Abstractions
{
    public interface IServicoAutenticacao
    {
        Task<ResultadoAutenticacao> RegistrarAsync(string email, string senha, int usuarioId);
        Task<Resultado<int>> VerificarCredenciaisAsync(string email, string senha);
        Task EntrarAsync(string email);
        Task InvalidarSessoesAsync(int usuarioId);
        Task LogoutAsync();
    }
}
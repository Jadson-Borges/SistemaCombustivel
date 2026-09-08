namespace SistemaCombustivel.Application.Abstractions
{
    public interface IServicoAutenticacao
    {
        Task<ResultadoAutenticacao> RegistrarAsync(string email, string senha, int usuarioId);
    }
}
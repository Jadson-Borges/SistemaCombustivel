namespace SistemaCombustivel.Application.Identity
{
    public interface IUserRepository
    {
        Task<CustomUser?> BuscarPorId(string userId, CancellationToken c);
    }
}

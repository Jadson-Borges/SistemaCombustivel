using SistemaCombustivel.Application.Identity;

namespace SistemaCombustivel.Application.Tests.Fakes;

public class FakeUserRepository : IUserRepository
{
    public Dictionary<string, CustomUser> Usuarios { get; } = new();

    public Task<CustomUser?> BuscarPorId(string userId, CancellationToken c)
    {
        c.ThrowIfCancellationRequested();   // imita um repositório real
        Usuarios.TryGetValue(userId, out var usuario);
        return Task.FromResult<CustomUser?>(usuario);
    }
}
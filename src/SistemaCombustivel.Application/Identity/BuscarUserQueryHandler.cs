using SistemaCombustivel.Domain.Identity;

namespace SistemaCombustivel.Application.Identity;

public class BuscarUserQueryHandler(IUserRepository UserRepository)
{
    public async Task<CustomUser?> Handle(BuscarUserQuery query, CancellationToken c)
    {
        try
        {
            return await UserRepository.BuscarPorId(query.UserId, c);
        }
        catch
        {
            throw;
        }
    }
}

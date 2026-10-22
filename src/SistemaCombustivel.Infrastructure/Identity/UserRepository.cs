using Microsoft.AspNetCore.Identity;
using SistemaCombustivel.Application.Identity;

namespace SistemaCombustivel.Infrastructure.Identity
{
    public class UserRepository(UserManager<ApplicationUser> UserManager) : IUserRepository
    {
        public async Task<CustomUser?> BuscarPorId(string userId, CancellationToken c)
        {
            var user = await UserManager.FindByIdAsync(userId);
            if (user is not null)
            {
                return new CustomUser() { Email = user.Email, UserId = user.Id, UserName = user.UserName ?? "" };
            }
            else return null;
        }
    }
}

using Microsoft.AspNetCore.Identity;
namespace SistemaCombustivel.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public int UsuarioId { get; set; }
    }
}

namespace SistemaCombustivel.Application.Identity;

public class CustomUser
{
    public string UserId { get; set; } = "";
    public string UserName { get; set; } = "";
    public string? Email { get; set; }   // o Identity permite e-mail nulo, e o tipo diz a verdade
}
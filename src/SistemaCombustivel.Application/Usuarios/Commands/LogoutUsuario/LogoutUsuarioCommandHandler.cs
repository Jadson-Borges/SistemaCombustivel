using SistemaCombustivel.Application.Abstractions;

namespace SistemaCombustivel.Application.Usuarios.Commands.LogoutUsuario;

public class LogoutUsuarioCommandHandler(IServicoAutenticacao servicoAutenticacao)
{
    public Task Handle(LogoutUsuarioCommand command, CancellationToken c) =>
        servicoAutenticacao.LogoutAsync();
}
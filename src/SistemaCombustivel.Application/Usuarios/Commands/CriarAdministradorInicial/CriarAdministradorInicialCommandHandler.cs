using SistemaCombustivel.Application.Abstractions;
using SistemaCombustivel.Application.Common;
using SistemaCombustivel.Application.Usuarios.Commands.CriarUsuario;
using SistemaCombustivel.Domain.Entities;

namespace SistemaCombustivel.Application.Usuarios.Commands.CriarAdministradorInicial;

public class CriarAdministradorInicialCommandHandler(
    IUsuarioRepository usuarioRepository,
    CriarUsuarioCommandHandler criarUsuario)
{
    public async Task<Resultado> Handle(CriarAdministradorInicialCommand command, CancellationToken c)
    {
        if (await usuarioRepository.ExisteAdministradorAsync())
            return Resultado.Ok();   // já existe: não faz nada (idempotente)

        return await criarUsuario.Handle(
            new CriarUsuarioCommand(command.NomeUsuario, command.Email, command.Senha, TipoUsuario.Administrador), c);
    }
}
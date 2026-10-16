using SistemaCombustivel.Application.Common;
using SistemaCombustivel.Application.Usuarios.Commands.CriarUsuario;
using SistemaCombustivel.Domain.Entities;

namespace SistemaCombustivel.Application.Usuarios.Commands.RegistrarUsuario;

public class RegistrarUsuarioCommandHandler(CriarUsuarioCommandHandler criarUsuario)
{
    // Cadastro público: o tipo é sempre Comum, e não existe como pedi-lo pelo comando.
    public Task<Resultado> Handle(RegistrarUsuarioCommand command, CancellationToken c) =>
        criarUsuario.Handle(
            new CriarUsuarioCommand(command.NomeUsuario, command.Email, command.Senha, TipoUsuario.Comum), c);
}
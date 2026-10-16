using SistemaCombustivel.Domain.Entities;

namespace SistemaCombustivel.Application.Usuarios.Commands.CriarUsuario;

public record CriarUsuarioCommand(string NomeUsuario, string Email, string Senha, TipoUsuario Tipo);
using SistemaCombustivel.Domain.Entities;

namespace SistemaCombustivel.Application.Usuarios.Queries.ListarUsuarios;

public record UsuarioListaDto(int Id, string NomeUsuario, string Email, TipoUsuario Tipo, bool Ativo);
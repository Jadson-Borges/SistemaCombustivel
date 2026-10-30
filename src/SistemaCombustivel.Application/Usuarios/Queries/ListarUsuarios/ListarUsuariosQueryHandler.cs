using SistemaCombustivel.Application.Abstractions;

namespace SistemaCombustivel.Application.Usuarios.Queries.ListarUsuarios;

public class ListarUsuariosQueryHandler(IUsuarioRepository usuarioRepository)
{
    public async Task<IReadOnlyList<UsuarioListaDto>> Handle(ListarUsuariosQuery query, CancellationToken c)
    {
        var usuarios = await usuarioRepository.ListarAsync(c);

        return usuarios
            .OrderBy(u => u.NomeUsuario)
            .Select(u => new UsuarioListaDto(u.Id, u.NomeUsuario, u.Email, u.Tipo, u.Ativo))
            .ToList();
    }
}
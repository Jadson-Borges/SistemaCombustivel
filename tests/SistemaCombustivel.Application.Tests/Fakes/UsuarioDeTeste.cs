using SistemaCombustivel.Domain.Entities;

namespace SistemaCombustivel.Application.Tests.Fakes;

public static class UsuarioDeTeste
{
    public static Usuario Criar(int id, string nome, TipoUsuario tipo = TipoUsuario.Comum, bool ativo = true)
    {
        var usuario = new Usuario(nome, $"{nome.ToLower()}@teste.com", tipo);
        typeof(Usuario).GetProperty(nameof(Usuario.Id))!.SetValue(usuario, id);
        if (!ativo) usuario.Desativar();
        return usuario;
    }
}
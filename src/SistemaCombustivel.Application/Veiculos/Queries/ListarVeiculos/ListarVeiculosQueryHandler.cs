using SistemaCombustivel.Application.Abstractions;

namespace SistemaCombustivel.Application.Veiculos.Queries.ListarVeiculos;

public class ListarVeiculosQueryHandler(IVeiculoRepository veiculoRepository)
{
    public async Task<IReadOnlyList<VeiculoListaDto>> Handle(ListarVeiculosQuery query, CancellationToken c)
    {
        var veiculos = query.VerTodos
            ? await veiculoRepository.ListarTodosAsync(c)
            : await veiculoRepository.ListarDoUsuarioAsync(query.UsuarioId, c);

        return veiculos
            .OrderBy(v => v.Descricao)
            .Select(v => new VeiculoListaDto(
                v.Id, v.Descricao, v.Marca, v.Modelo, v.Placa, v.KmInicial, v.KmOficial, v.Ativo))
            .ToList();
    }
}
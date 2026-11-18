namespace SistemaCombustivel.Application.Veiculos.Queries.ListarVeiculos;

public record VeiculoListaDto(
    int Id, string Descricao, string Marca, string Modelo, string Placa,
    decimal KmInicial, decimal KmOficial, bool Ativo);
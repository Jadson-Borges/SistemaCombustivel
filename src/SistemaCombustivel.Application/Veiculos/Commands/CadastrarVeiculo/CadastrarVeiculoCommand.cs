namespace SistemaCombustivel.Application.Veiculos.Commands.CadastrarVeiculo;

public record CadastrarVeiculoCommand(
    int UsuarioId, string Descricao, string Marca, string Modelo, string Placa, decimal KmInicial);
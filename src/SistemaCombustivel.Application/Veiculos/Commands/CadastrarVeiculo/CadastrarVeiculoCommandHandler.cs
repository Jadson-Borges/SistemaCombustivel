using SistemaCombustivel.Application.Abstractions;
using SistemaCombustivel.Application.Common;
using SistemaCombustivel.Domain.Entities;

namespace SistemaCombustivel.Application.Veiculos.Commands.CadastrarVeiculo;

public class CadastrarVeiculoCommandHandler(
    IVeiculoRepository veiculoRepository,
    IUsuarioRepository usuarioRepository)
{
    public async Task<Resultado> Handle(CadastrarVeiculoCommand command, CancellationToken c)
    {
        var usuario = await usuarioRepository.ObterPorIdAsync(command.UsuarioId, c);
        if (usuario is null || !usuario.Ativo)
            return Resultado.Falha("Usuário não encontrado ou inativo.");

        if (string.IsNullOrWhiteSpace(command.Descricao))
            return Resultado.Falha("Informe a identificação do veículo.");
        if (string.IsNullOrWhiteSpace(command.Marca))
            return Resultado.Falha("Informe a marca do veículo.");
        if (string.IsNullOrWhiteSpace(command.Modelo))
            return Resultado.Falha("Informe o modelo do veículo.");
        if (command.KmInicial < 0)
            return Resultado.Falha("O KM inicial não pode ser negativo.");
        if (!Veiculo.PlacaEhValida(command.Placa))
            return Resultado.Falha("Placa inválida. Use o formato ABC1234 ou ABC1D23.");

        var placa = Veiculo.NormalizarPlaca(command.Placa);
        if (await veiculoRepository.ExistePlacaAsync(placa, c))
            return Resultado.Falha("Já existe um veículo cadastrado com esta placa.");

        var veiculo = new Veiculo(command.Descricao, command.Marca, command.Modelo, placa, command.KmInicial);
        veiculo.AdicionarUsuario(usuario);   // vínculo de quem cadastrou

        await veiculoRepository.AdicionarAsync(veiculo, c);
        await veiculoRepository.SalvarAlteracoesAsync(c);   // veículo + vínculo na mesma transação

        return Resultado.Ok();
    }
}
using SistemaCombustivel.Application.Abstractions;

namespace SistemaCombustivel.Application.Tests.Fakes;

public class FakeUnitOfWork : IUnitOfWork
{
    public int TransacoesIniciadas { get; private set; }
    public int Commits { get; private set; }
    public int Rollbacks { get; private set; }

    public async Task ExecutarEmTransacaoAsync(Func<Task> operacao)
    {
        TransacoesIniciadas++;

        try
        {
            await operacao();
            Commits++;
        }
        catch
        {
            Rollbacks++;   // imita o banco desfazendo a transação
            throw;         // e o erro continua subindo, como no UnitOfWork real
        }
    }
}
using SistemaCombustivel.Application.Abstractions;

namespace SistemaCombustivel.Application.Tests.Fakes;

public class FakeUnitOfWork : IUnitOfWork
{
    public int TransacoesIniciadas { get; private set; }
    public int Commits { get; private set; }
    public int Rollbacks { get; private set; }
    public List<CancellationToken> TokensRecebidos { get; } = new();

    public async Task ExecutarEmTransacaoAsync(Func<Task> operacao, CancellationToken c)
    {
        TokensRecebidos.Add(c);
        c.ThrowIfCancellationRequested();   // como o BeginTransactionAsync real

        TransacoesIniciadas++;

        try
        {
            await operacao();
            Commits++;
        }
        catch
        {
            Rollbacks++;
            throw;
        }
    }
}
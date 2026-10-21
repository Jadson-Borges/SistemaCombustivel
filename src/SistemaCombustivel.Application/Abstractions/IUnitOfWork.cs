namespace SistemaCombustivel.Application.Abstractions
{
    public interface IUnitOfWork
    {
        Task ExecutarEmTransacaoAsync(Func<Task> operacao, CancellationToken c);
    }
}
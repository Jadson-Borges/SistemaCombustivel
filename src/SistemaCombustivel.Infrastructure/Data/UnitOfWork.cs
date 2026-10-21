using Microsoft.EntityFrameworkCore;
using SistemaCombustivel.Application.Abstractions;

namespace SistemaCombustivel.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public async Task ExecutarEmTransacaoAsync(Func<Task> operacao, CancellationToken c)
        {
            var strategy = _context.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                await using var transacao = await _context.Database.BeginTransactionAsync(c);

                try
                {
                    await operacao();
                    await transacao.CommitAsync(c);
                }
                catch
                {
                    // Token novo: se o original foi cancelado, o rollback não pode ser abortado junto.
                    await transacao.RollbackAsync(CancellationToken.None);
                    throw;
                }
            });
        }
    }
}
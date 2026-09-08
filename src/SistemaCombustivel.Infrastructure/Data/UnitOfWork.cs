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

        public async Task ExecutarEmTransacaoAsync(Func<Task> operacao)
        {
            var strategy = _context.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                await using var transacao = await _context.Database.BeginTransactionAsync();

                try
                {
                    await operacao();
                    await transacao.CommitAsync();
                }
                catch
                {
                    await transacao.RollbackAsync();
                    throw;
                }
            });
        }
    }
}
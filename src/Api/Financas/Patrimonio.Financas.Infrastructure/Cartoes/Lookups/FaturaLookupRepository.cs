using Microsoft.EntityFrameworkCore;
using Patrimonio.Financas.Application.Cartoes.Lookups.Abstractions;
using Patrimonio.Financas.Application.Cartoes.Lookups.ReadModels;
using Patrimonio.Financas.Infrastructure.Persistence;

namespace Patrimonio.Financas.Infrastructure.Cartoes.Lookups;

/// <summary>
/// Implementa as operações de consulta de referência de faturas.
/// </summary>
internal sealed class FaturaLookupRepository(
    FinancasDbContext context) : IFaturaLookupRepository
{
    /// <inheritdoc/>
    public async Task<IReadOnlyCollection<FaturaOpcaoReadModel>> ListarPorCartaoAsync(
        int cartaoId,
        CancellationToken cancellationToken)
    {
        return await context.Faturas
            .AsNoTracking()
            .Where(f => f.CartaoId == cartaoId)
            .OrderByDescending(f => f.DataVencimento)
            .Select(f => new FaturaOpcaoReadModel
            {
                Id = f.Id,
                DataVencimento = f.DataVencimento
            })
            .ToListAsync(cancellationToken);
    }
}

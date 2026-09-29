using Microsoft.EntityFrameworkCore;
using Patrimonio.Financas.Application.Contas.Lookups.Abstractions;
using Patrimonio.Financas.Application.Contas.Lookups.ReadModels;
using Patrimonio.Financas.Infrastructure.Persistence;

namespace Patrimonio.Financas.Infrastructure.Contas.Lookups;

/// <summary>
/// Implementa as operações de consulta de referência de contas.
/// </summary>
internal sealed class ContaLookupRepository(
    FinancasDbContext context) : IContaLookupRepository
{
    /// <inheritdoc/>
    public async Task<IReadOnlyCollection<ContaOpcaoReadModel>> ListarOpcoesAsync(
        CancellationToken cancellationToken)
    {
        return await context.Contas
            .AsNoTracking()
            .OrderBy(i => i.Nome.Valor)
            .Select(i => new ContaOpcaoReadModel
            {
                Id = i.Id,
                Nome = i.Nome.Valor
            })
            .ToListAsync(cancellationToken);
    }
}

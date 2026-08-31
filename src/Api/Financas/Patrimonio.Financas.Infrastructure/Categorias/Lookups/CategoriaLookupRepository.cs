using Microsoft.EntityFrameworkCore;
using Patrimonio.Financas.Application.Categorias.Lookups.Abstractions;
using Patrimonio.Financas.Application.Categorias.Lookups.ReadModels;
using Patrimonio.Financas.Infrastructure.Persistence;

namespace Patrimonio.Financas.Infrastructure.Categorias.Lookups;

/// <summary>
/// Implementa as operações de consulta de referência de categorias.
/// </summary>
internal sealed class CategoriaLookupRepository(
    FinancasDbContext context) : ICategoriaLookupRepository
{
    /// <inheritdoc/>
    public async Task<IReadOnlyCollection<CategoriaOpcaoReadModel>> ListarOpcoesAsync(
        CancellationToken cancellationToken)
    {
        return await context.Categorias
            .AsNoTracking()
            .OrderBy(i => i.Nome.Valor)
            .Select(i => new CategoriaOpcaoReadModel
            {
                Id = i.Id,
                Nome = i.Nome.Valor
            })
            .ToListAsync(cancellationToken);
    }
}

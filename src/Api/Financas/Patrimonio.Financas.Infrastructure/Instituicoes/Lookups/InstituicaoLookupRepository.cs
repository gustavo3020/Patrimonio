using Microsoft.EntityFrameworkCore;
using Patrimonio.Financas.Application.Instituicoes.Lookups.Abstractions;
using Patrimonio.Financas.Application.Instituicoes.Lookups.ReadModels;
using Patrimonio.Financas.Infrastructure.Persistence;

namespace Patrimonio.Financas.Infrastructure.Instituicoes.Lookups;

/// <summary>
/// Implementa as operações de consulta de referência de instituições.
/// </summary>
internal sealed class InstituicaoLookupRepository(
    FinancasDbContext context) : IInstituicaoLookupRepository
{
    /// <inheritdoc/>
    public async Task<IReadOnlyCollection<InstituicaoOpcaoReadModel>> ListarOpcoesAsync(
        CancellationToken cancellationToken)
    {
        return await context.Instituicoes
            .AsNoTracking()
            .OrderBy(i => i.Nome.Valor)
            .Select(i => new InstituicaoOpcaoReadModel
            {
                Id = i.Id,
                Nome = i.Nome.Valor
            })
            .ToListAsync(cancellationToken);
    }
}

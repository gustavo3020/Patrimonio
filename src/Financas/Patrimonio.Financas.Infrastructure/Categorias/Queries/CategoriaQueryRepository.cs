using Microsoft.EntityFrameworkCore;
using Patrimonio.Financas.Application.Categorias.Queries.Abstractions;
using Patrimonio.Financas.Application.Categorias.Queries.ReadModels;
using Patrimonio.Financas.Infrastructure.Common.Exceptions;
using Patrimonio.Financas.Infrastructure.Persistence;

namespace Patrimonio.Financas.Infrastructure.Categorias.Queries;

/// <summary>
/// Implementa as operações de leitura de categorias.
/// </summary>
internal sealed class CategoriaQueryRepository(FinancasDbContext context) : ICategoriaQueryRepository
{
    /// <inheritdoc/>
    public async Task<IReadOnlyCollection<CategoriaListaReadModel>> ListarAsync(CancellationToken cancellationToken)
    {
        return await context.Categorias
            .AsNoTracking()
            .Select(c => new CategoriaListaReadModel
            {
                Id = c.Id,
                Nome = c.Nome.Valor,
                DataCriacao = c.DataCriacao,
                DataAlteracao = c.DataAlteracao
            })
            .OrderByDescending(c => c.DataCriacao)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<CategoriaDetalheReadModel> ObterPorIdAsync(int categoriaId, CancellationToken cancellationToken)
    {
        var entidade = await context.Categorias
            .AsNoTracking()
            .Select(c => new CategoriaDetalheReadModel
            {
                Id = c.Id,
                Nome = c.Nome.Valor,
                DataCriacao = c.DataCriacao,
                DataAlteracao = c.DataAlteracao
            })
            .SingleOrDefaultAsync(c => c.Id == categoriaId, cancellationToken);

        return entidade ?? throw new RecursoNaoEncontradoException($"Categoria com Id {categoriaId} não encontrada.");
    }
}

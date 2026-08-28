using Microsoft.EntityFrameworkCore;
using Patrimonio.Financas.Application.Instituicoes.Queries.Abstractions;
using Patrimonio.Financas.Application.Instituicoes.Queries.ReadModels;
using Patrimonio.Financas.Infrastructure.Common.Exceptions;
using Patrimonio.Financas.Infrastructure.Persistence;

namespace Patrimonio.Financas.Infrastructure.Instituicoes.Queries;

/// <summary>
/// Implementa as operações de leitura de instituições.
/// </summary>
internal sealed class InstituicaoQueryRepository(FinancasDbContext context) : IInstituicaoQueryRepository
{
    /// <inheritdoc/>
    public async Task<IReadOnlyCollection<InstituicaoListaReadModel>> ListarAsync(CancellationToken cancellationToken)
    {
        return await context.Instituicoes
            .AsNoTracking()
            .Select(c => new InstituicaoListaReadModel
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
    public async Task<InstituicaoDetalheReadModel> ObterPorIdAsync(int instituicaoId, CancellationToken cancellationToken)
    {
        var entidade = await context.Instituicoes
            .AsNoTracking()
            .Select(c => new InstituicaoDetalheReadModel
            {
                Id = c.Id,
                Nome = c.Nome.Valor,
                DataCriacao = c.DataCriacao,
                DataAlteracao = c.DataAlteracao
            })
            .SingleOrDefaultAsync(c => c.Id == instituicaoId, cancellationToken);

        return entidade ?? throw new RecursoNaoEncontradoException($"Instituição com Id {instituicaoId} não encontrada.");
    }
}

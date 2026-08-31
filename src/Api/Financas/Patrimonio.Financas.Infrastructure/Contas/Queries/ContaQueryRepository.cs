using Microsoft.EntityFrameworkCore;
using Patrimonio.Financas.Application.Contas.Queries.Abstractions;
using Patrimonio.Financas.Application.Contas.Queries.ReadModels;
using Patrimonio.Financas.Infrastructure.Persistence;

namespace Patrimonio.Financas.Infrastructure.Contas.Queries;

/// <summary>
/// Implementa as operações de leitura de contas.
/// </summary>
internal sealed class ContaQueryRepository(FinancasDbContext context) : IContaQueryRepository
{
    /// <inheritdoc/>
    public async Task<IReadOnlyCollection<ContaListaReadModel>> ListarAsync(CancellationToken cancellationToken)
    {
        return await (
            from conta in context.Contas
            join instituicao in context.Instituicoes
                on conta.InstituicaoId equals instituicao.Id
            select new ContaListaReadModel
            {
                Id = conta.Id,
                Nome = conta.Nome.Valor,
                InstituicaoNome = instituicao.Nome.Valor,
                DataCriacao = conta.DataCriacao,
                DataAlteracao = conta.DataAlteracao
            })
            .AsNoTracking()
            .OrderByDescending(c => c.DataCriacao)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<ContaDetalheReadModel?> ObterPorIdAsync(int contaId, CancellationToken cancellationToken)
    {
        return await (
            from conta in context.Contas
            join instituicao in context.Instituicoes
                on conta.InstituicaoId equals instituicao.Id
            where conta.Id == contaId
            select new ContaDetalheReadModel
            {
                Id = conta.Id,
                Nome = conta.Nome.Valor,
                InstituicaoId = instituicao.Id,
                InstituicaoNome = instituicao.Nome.Valor,
                DataCriacao = conta.DataCriacao,
                DataAlteracao = conta.DataAlteracao
            })
            .AsNoTracking()
            .SingleOrDefaultAsync(cancellationToken);
    }
}

using Microsoft.EntityFrameworkCore;
using Patrimonio.Financas.Application.Cartoes.Queries.Abstractions;
using Patrimonio.Financas.Application.Cartoes.Queries.ReadModels;
using Patrimonio.Financas.Infrastructure.Persistence;

namespace Patrimonio.Financas.Infrastructure.Cartoes.Queries;

/// <summary>
/// Implementa as operações de leitura de faturas.
/// </summary>
internal sealed class FaturaQueryRepository(FinancasDbContext context) : IFaturaQueryRepository
{
    /// <inheritdoc/>
    public async Task<IReadOnlyCollection<FaturaListaReadModel>> ListarAsync(
        int cartaoId,
        CancellationToken cancellationToken)
    {
        return await (
            from fatura in context.Faturas
            join cartao in context.Cartoes
                on fatura.CartaoId equals cartao.Id
            where fatura.CartaoId == cartaoId
            select new FaturaListaReadModel
            {
                Id = fatura.Id,
                DataFechamento = fatura.DataFechamento,
                DataVencimento = fatura.DataVencimento,
                Status = fatura.Status,
                DataPagamento = fatura.DataPagamento,
                CartaoNome = cartao.Nome.Valor,
                DataCriacao = fatura.DataCriacao,
                DataAlteracao = fatura.DataAlteracao
            })
            .AsNoTracking()
            .OrderByDescending(f => f.DataVencimento)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<FaturaDetalheReadModel?> ObterPorIdAsync(int faturaId, CancellationToken cancellationToken)
    {
        return await (
            from fatura in context.Faturas
            join cartao in context.Cartoes
                on fatura.CartaoId equals cartao.Id
            where fatura.Id == faturaId
            select new FaturaDetalheReadModel
            {
                Id = fatura.Id,
                DataFechamento = fatura.DataFechamento,
                DataVencimento = fatura.DataVencimento,
                Status = fatura.Status,
                DataPagamento = fatura.DataPagamento,
                CartaoId = fatura.CartaoId,
                CartaoNome = cartao.Nome.Valor,
                DataCriacao = fatura.DataCriacao,
                DataAlteracao = fatura.DataAlteracao
            })
            .AsNoTracking()
            .SingleOrDefaultAsync(cancellationToken);
    }
}

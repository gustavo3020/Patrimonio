using Microsoft.EntityFrameworkCore;
using Patrimonio.Financas.Application.Cartoes.Queries.Abstractions;
using Patrimonio.Financas.Application.Cartoes.Queries.ReadModels;
using Patrimonio.Financas.Infrastructure.Persistence;

namespace Patrimonio.Financas.Infrastructure.Cartoes.Queries;

/// <summary>
/// Implementa as operações de leitura de lancamentos.
/// </summary>
internal sealed class LancamentoQueryRepository(FinancasDbContext context) : ILancamentoQueryRepository
{
    /// <inheritdoc/>
    public async Task<IReadOnlyCollection<LancamentoListaReadModel>> ListarAsync(CancellationToken cancellationToken)
    {
        return await (
            from lancamento in context.Lancamentos
            join categoria in context.Categorias
                on lancamento.CategoriaId equals categoria.Id
            select new LancamentoListaReadModel
            {
                Id = lancamento.Id,
                Descricao = lancamento.Descricao.Valor,
                Valor = lancamento.Valor.Valor,
                DataCompra = lancamento.DataCompra,
                Estabelecimento = lancamento.Estabelecimento.Valor,
                Responsavel = lancamento.Responsavel.Valor,
                NumeroParcela = lancamento.Parcelamento.NumeroParcela,
                TotalParcelas = lancamento.Parcelamento.TotalParcelas,
                CategoriaNome = categoria.Nome.Valor,
                DataCriacao = lancamento.DataCriacao,
                DataAlteracao = lancamento.DataAlteracao
            })
            .AsNoTracking()
            .OrderByDescending(l => l.DataCriacao)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<LancamentoDetalheReadModel?> ObterPorIdAsync(int lancamentoId, CancellationToken cancellationToken)
    {
        return await (
            from lancamento in context.Lancamentos
            join categoria in context.Categorias
                on lancamento.CategoriaId equals categoria.Id
            where lancamento.Id == lancamentoId
            select new LancamentoDetalheReadModel
            {
                Id = lancamento.Id,
                Descricao = lancamento.Descricao.Valor,
                Valor = lancamento.Valor.Valor,
                DataCompra = lancamento.DataCompra,
                Estabelecimento = lancamento.Estabelecimento.Valor,
                Responsavel = lancamento.Responsavel.Valor,
                NumeroParcela = lancamento.Parcelamento.NumeroParcela,
                TotalParcelas = lancamento.Parcelamento.TotalParcelas,
                FaturaId = lancamento.FaturaId,
                CategoriaId = categoria.Id,
                CategoriaNome = categoria.Nome.Valor,
                DataCriacao = lancamento.DataCriacao,
                DataAlteracao = lancamento.DataAlteracao
            })
            .AsNoTracking()
            .SingleOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<decimal> ObterValorTotalPorFaturaAsync(int faturaId, CancellationToken cancellationToken)
    {
        return await context.Lancamentos
            .Where(l => l.FaturaId == faturaId)
            .AsNoTracking()
            .SumAsync(l => l.Valor.Valor, cancellationToken);
    }
}

using Microsoft.EntityFrameworkCore;
using Patrimonio.Financas.Application.Movimentacoes.Queries.Abstractions;
using Patrimonio.Financas.Application.Movimentacoes.Queries.ReadModels;
using Patrimonio.Financas.Infrastructure.Persistence;

namespace Patrimonio.Financas.Infrastructure.Movimentacoes.Queries;

/// <summary>
/// Implementa as operações de leitura de movimentações.
/// </summary>
internal sealed class MovimentacaoQueryRepository(FinancasDbContext context) : IMovimentacaoQueryRepository
{
    /// <inheritdoc/>
    public async Task<IReadOnlyCollection<MovimentacaoListaReadModel>> ListarAsync(CancellationToken cancellationToken)
    {
        return await (
            from m in context.Movimentacoes
            join conta in context.Contas
                on m.ContaId equals conta.Id
            join categoria in context.Categorias
                on m.CategoriaId equals categoria.Id
            select new MovimentacaoListaReadModel
            {
                Id = m.Id,

                Data = m.Data,
                Valor = m.Valor.Valor,
                Natureza = m.Natureza,
                Tipo = m.Tipo,
                Descricao = m.Descricao,

                ContaNome = conta.Nome.Valor,
                CategoriaNome = categoria.Nome.Valor,

                DataCriacao = m.DataCriacao,
                DataAlteracao = m.DataAlteracao
            })
            .AsNoTracking()
            .OrderByDescending(c => c.DataCriacao)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<MovimentacaoDetalheReadModel?> ObterPorIdAsync(int movimentacaoId, CancellationToken cancellationToken)
    {
        return await (
            from m in context.Movimentacoes.AsNoTracking()
            join conta in context.Contas
                on m.ContaId equals conta.Id
            join categoria in context.Categorias
                on m.CategoriaId equals categoria.Id
            where m.Id == movimentacaoId
            select new MovimentacaoDetalheReadModel
            {
                Id = m.Id,

                Data = m.Data,
                Valor = m.Valor.Valor,
                Natureza = m.Natureza,
                Tipo = m.Tipo,
                Descricao = m.Descricao,

                ContaId = conta.Id,
                ContaNome = conta.Nome.Valor,

                CategoriaId = categoria.Id,
                CategoriaNome = categoria.Nome.Valor,

                DataCriacao = m.DataCriacao,
                DataAlteracao = m.DataAlteracao
            })
            .SingleOrDefaultAsync(cancellationToken);
    }
}

using Microsoft.EntityFrameworkCore;
using Patrimonio.Financas.Application.Cartoes.Queries.Abstractions;
using Patrimonio.Financas.Application.Cartoes.Queries.ReadModels;
using Patrimonio.Financas.Infrastructure.Persistence;

namespace Patrimonio.Financas.Infrastructure.Cartoes.Queries;

/// <summary>
/// Implementa as operações de leitura de cartões.
/// </summary>
internal sealed class CartaoQueryRepository(FinancasDbContext context) : ICartaoQueryRepository
{
    /// <inheritdoc/>
    public async Task<IReadOnlyCollection<CartaoListaReadModel>> ListarAsync(CancellationToken cancellationToken)
    {
        return await (
            from cartao in context.Cartoes
            join instituicao in context.Instituicoes
                on cartao.InstituicaoId equals instituicao.Id
            select new CartaoListaReadModel
            {
                Id = cartao.Id,
                Nome = cartao.Nome.Valor,
                Limite = cartao.Limite.Valor,
                DiaFechamento = cartao.DiaFechamento.Valor,
                DiaVencimento = cartao.DiaVencimento.Valor,
                InstituicaoNome = instituicao.Nome.Valor,
                DataCriacao = cartao.DataCriacao,
                DataAlteracao = cartao.DataAlteracao
            })
            .AsNoTracking()
            .OrderByDescending(c => c.DataCriacao)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<CartaoDetalheReadModel?> ObterPorIdAsync(int cartaoId, CancellationToken cancellationToken)
    {
        return await (
            from cartao in context.Cartoes
            join instituicao in context.Instituicoes
                on cartao.InstituicaoId equals instituicao.Id
            where cartao.Id == cartaoId
            select new CartaoDetalheReadModel
            {
                Id = cartao.Id,
                Nome = cartao.Nome.Valor,
                Bandeira = cartao.Bandeira,
                Limite = cartao.Limite.Valor,
                DiaFechamento = cartao.DiaFechamento.Valor,
                DiaVencimento = cartao.DiaVencimento.Valor,
                InstituicaoId = instituicao.Id,
                InstituicaoNome = instituicao.Nome.Valor,
                DataCriacao = cartao.DataCriacao,
                DataAlteracao = cartao.DataAlteracao
            })
            .AsNoTracking()
            .SingleOrDefaultAsync(cancellationToken);
    }
}

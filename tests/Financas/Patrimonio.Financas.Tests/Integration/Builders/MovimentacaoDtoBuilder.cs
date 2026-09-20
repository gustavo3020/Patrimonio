using Patrimonio.Financas.Contracts.Movimentacoes.Dtos;
using Patrimonio.Financas.SharedKernel.Movimentacoes.Enums;

namespace Patrimonio.Financas.Tests.Integration.Builders;

internal static class MovimentacaoDtoBuilder
{
    public static MovimentacaoCriacaoDto Criar(
        int categoriaId,
        int contaId,
        decimal? valor = null,
        string? descricao = null)
    {
        return new MovimentacaoCriacaoDto
        {
            Data = DateOnly.FromDateTime(DateTime.Now),
            Valor = valor ?? 500.00m,
            Natureza = Natureza.Saida,
            Tipo = TipoMovimentacao.Pix,
            Descricao = descricao ?? $"Movimentação {Guid.NewGuid()}",
            CategoriaId = categoriaId,
            ContaId = contaId
        };
    }

    public static MovimentacaoAlteracaoDto Alterar(
        int categoriaId,
        int contaId,
        decimal? valor = null,
        string? descricao = null)
    {
        return new MovimentacaoAlteracaoDto
        {
            Data = DateOnly.FromDateTime(DateTime.Now).AddDays(5),
            Valor = valor ?? 1000.00m,
            Natureza = Natureza.Entrada,
            Tipo = TipoMovimentacao.Credito,
            Descricao = descricao ?? $"Movimentação alterada {Guid.NewGuid()}",
            CategoriaId = categoriaId,
            ContaId = contaId
        };
    }
}

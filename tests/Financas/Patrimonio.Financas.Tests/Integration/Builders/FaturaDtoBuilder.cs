using Patrimonio.Financas.Contracts.Cartoes.Dtos;

namespace Patrimonio.Financas.Tests.Integration.Builders;

internal static class FaturaDtoBuilder
{
    public static FaturaCriacaoDto Criar(
        int cartaoId,
        DateOnly? dataFechamento = null,
        DateOnly? dataVencimento = null)
    {
        return new FaturaCriacaoDto
        {
            DataFechamento = dataFechamento ?? new DateOnly(2026, 09, 11),
            DataVencimento = dataVencimento ?? new DateOnly(2026, 09, 15),
            CartaoId = cartaoId
        };
    }

    public static FaturaAlteracaoDto Alterar(
        DateOnly? dataFechamento = null,
        DateOnly? dataVencimento = null)
    {
        return new FaturaAlteracaoDto
        {
            DataFechamento = dataFechamento ?? new DateOnly(2026, 09, 21),
            DataVencimento = dataVencimento ?? new DateOnly(2026, 09, 25)
        };
    }

    public static FaturaPagamentoDto Pagar(
        int contaId,
        int categoriaId,
        DateOnly? dataPagamento = null)
    {
        return new FaturaPagamentoDto
        {
            DataPagamento = dataPagamento ?? new DateOnly(2026, 9, 10),
            ContaId = contaId,
            CategoriaId = categoriaId
        };
    }
}

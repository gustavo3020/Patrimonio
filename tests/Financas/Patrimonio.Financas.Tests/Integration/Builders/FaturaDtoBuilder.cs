using Patrimonio.Financas.Contracts.Cartoes.Dtos;

namespace Patrimonio.Financas.Tests.Integration.Builders;

internal static class FaturaDtoBuilder
{
    public static FaturaCriacaoDto Criar(int cartaoId)
    {
        return new FaturaCriacaoDto
        {
            DataFechamento = new DateOnly(2026, 09, 11),
            DataVencimento = new DateOnly(2026, 09, 15),
            CartaoId = cartaoId
        };
    }

    public static FaturaAlteracaoDto Alterar()
    {
        return new FaturaAlteracaoDto
        {
            DataFechamento = new DateOnly(2026, 09, 21),
            DataVencimento = new DateOnly(2026, 09, 25),
        };
    }
}

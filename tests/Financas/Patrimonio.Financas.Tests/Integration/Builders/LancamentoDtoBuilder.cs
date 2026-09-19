using Patrimonio.Financas.Contracts.Cartoes.Dtos;

namespace Patrimonio.Financas.Tests.Integration.Builders;

internal static class LancamentoDtoBuilder
{
    public static LancamentoCriacaoDto Criar(int faturaId, int categoriaId, int totalParcelas = 1)
    {
        return new LancamentoCriacaoDto
        {
            Descricao = $"Lancamento {Guid.NewGuid()}",
            Valor = 100.00m,
            DataCompra = new DateOnly(2026, 9, 1),
            Estabelecimento = "Estabelecimento Teste",
            Responsavel = "Responsavel Teste",
            TotalParcelas = totalParcelas,
            FaturaId = faturaId,
            CategoriaId = categoriaId
        };
    }

    public static LancamentoAlteracaoDto Alterar(int categoriaId)
    {
        return new LancamentoAlteracaoDto
        {
            Descricao = $"Lancamento alterado {Guid.NewGuid()}",
            Valor = 200.00m,
            DataCompra = new DateOnly(2026, 9, 6),
            Estabelecimento = "Estabelecimento Alterado",
            Responsavel = "Responsavel Alterado",
            CategoriaId = categoriaId
        };
    }
}

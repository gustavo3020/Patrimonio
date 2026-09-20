using Patrimonio.Financas.Contracts.Cartoes.Dtos;

namespace Patrimonio.Financas.Tests.Integration.Builders;

internal static class LancamentoDtoBuilder
{
    public static LancamentoCriacaoDto Criar(
        int faturaId,
        int categoriaId,
        string? descricao = null,
        int? totalParcelas = null)
    {
        return new LancamentoCriacaoDto
        {
            Descricao = descricao ?? $"Lancamento {Guid.NewGuid()}",
            Valor = 100.00m,
            DataCompra = new DateOnly(2026, 9, 1),
            Estabelecimento = "Estabelecimento Teste",
            Responsavel = "Responsavel Teste",
            TotalParcelas = totalParcelas ?? 1,
            FaturaId = faturaId,
            CategoriaId = categoriaId
        };
    }

    public static LancamentoAlteracaoDto Alterar(
        int categoriaId,
        string? descricao = null)
    {
        return new LancamentoAlteracaoDto
        {
            Descricao = descricao ?? $"Lancamento alterado {Guid.NewGuid()}",
            Valor = 200.00m,
            DataCompra = new DateOnly(2026, 9, 6),
            Estabelecimento = "Estabelecimento Alterado",
            Responsavel = "Responsavel Alterado",
            CategoriaId = categoriaId
        };
    }
}

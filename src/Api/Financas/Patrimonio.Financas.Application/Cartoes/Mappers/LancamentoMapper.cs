using Patrimonio.Financas.Application.Cartoes.Queries.ReadModels;
using Patrimonio.Financas.Contracts.Cartoes.Dtos;

namespace Patrimonio.Financas.Application.Cartoes.Mappers;

/// <summary>
/// Realiza o mapeamento de lançamentos para seus respectivos DTOs.
/// </summary>
internal static class LancamentoMapper
{
    public static LancamentoListaDto Mapear(LancamentoListaReadModel lancamento)
    {
        return new LancamentoListaDto
        {
            Id = lancamento.Id,
            Descricao = lancamento.Descricao,
            Valor = lancamento.Valor,
            DataCompra = lancamento.DataCompra,
            Estabelecimento = lancamento.Estabelecimento,
            Responsavel = lancamento.Responsavel,
            NumeroParcela = lancamento.NumeroParcela,
            TotalParcelas = lancamento.TotalParcelas,
            FaturaNome = lancamento.FaturaNome,
            CategoriaNome = lancamento.CategoriaNome,
            DataCriacao = lancamento.DataCriacao,
            DataAlteracao = lancamento.DataAlteracao
        };
    }

    public static LancamentoDetalheDto Mapear(LancamentoDetalheReadModel lancamento)
    {
        return new LancamentoDetalheDto
        {
            Id = lancamento.Id,
            Descricao = lancamento.Descricao,
            Valor = lancamento.Valor,
            DataCompra = lancamento.DataCompra,
            Estabelecimento = lancamento.Estabelecimento,
            Responsavel = lancamento.Responsavel,
            NumeroParcela = lancamento.NumeroParcela,
            TotalParcelas = lancamento.TotalParcelas,
            FaturaId = lancamento.FaturaId,
            FaturaNome = lancamento.FaturaNome,
            CategoriaId = lancamento.CategoriaId,
            CategoriaNome = lancamento.CategoriaNome,
            DataCriacao = lancamento.DataCriacao,
            DataAlteracao = lancamento.DataAlteracao
        };
    }
}

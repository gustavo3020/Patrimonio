using Patrimonio.Financas.Application.Movimentacoes.Queries.ReadModels;
using Patrimonio.Financas.Contracts.Movimentacoes.Dtos;

namespace Patrimonio.Financas.Application.Movimentacoes.Mappers;

/// <summary>
/// Realiza o mapeamento de movimentações para seus respectivos DTOs.
/// </summary>
internal static class MovimentacaoMapper
{
    public static MovimentacaoListaDto Mapear(MovimentacaoListaReadModel movimentacao)
    {
        return new MovimentacaoListaDto
        {
            Id = movimentacao.Id,
            Data = movimentacao.Data,
            Valor = movimentacao.Valor,
            Natureza = movimentacao.Natureza,
            Tipo = movimentacao.Tipo,
            Descricao = movimentacao.Descricao,
            ContaNome = movimentacao.ContaNome,
            CategoriaNome = movimentacao.CategoriaNome,
            DataCriacao = movimentacao.DataCriacao,
            DataAlteracao = movimentacao.DataAlteracao
        };
    }

    public static MovimentacaoDetalheDto Mapear(MovimentacaoDetalheReadModel movimentacao)
    {
        return new MovimentacaoDetalheDto
        {
            Id = movimentacao.Id,
            Data = movimentacao.Data,
            Valor = movimentacao.Valor,
            Natureza = movimentacao.Natureza,
            Tipo = movimentacao.Tipo,
            Descricao = movimentacao.Descricao,
            ContaId = movimentacao.ContaId,
            ContaNome = movimentacao.ContaNome,
            CategoriaId = movimentacao.CategoriaId,
            CategoriaNome = movimentacao.CategoriaNome,
            DataCriacao = movimentacao.DataCriacao,
            DataAlteracao = movimentacao.DataAlteracao
        };
    }
}

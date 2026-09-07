using Patrimonio.Financas.Application.Cartoes.Queries.ReadModels;
using Patrimonio.Financas.Contracts.Cartoes.Dtos;

namespace Patrimonio.Financas.Application.Cartoes.Mappers;

/// <summary>
/// Realiza o mapeamento de faturas para seus respectivos DTOs.
/// </summary>
internal static class FaturaMapper
{
    public static FaturaListaDto Mapear(FaturaListaReadModel fatura)
    {
        return new FaturaListaDto
        {
            Id = fatura.Id,
            DataFechamento = fatura.DataFechamento,
            DataVencimento = fatura.DataVencimento,
            Status = fatura.Status,
            DataPagamento = fatura.DataPagamento,
            CartaoNome = fatura.CartaoNome,
            DataCriacao = fatura.DataCriacao,
            DataAlteracao = fatura.DataAlteracao
        };
    }

    public static FaturaDetalheDto Mapear(FaturaDetalheReadModel fatura)
    {
        return new FaturaDetalheDto
        {
            Id = fatura.Id,
            DataFechamento = fatura.DataFechamento,
            DataVencimento = fatura.DataVencimento,
            Status = fatura.Status,
            DataPagamento = fatura.DataPagamento,
            CartaoId = fatura.CartaoId,
            CartaoNome = fatura.CartaoNome,
            DataCriacao = fatura.DataCriacao,
            DataAlteracao = fatura.DataAlteracao
        };
    }
}

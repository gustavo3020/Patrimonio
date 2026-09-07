using Patrimonio.Financas.Application.Cartoes.Queries.ReadModels;
using Patrimonio.Financas.Contracts.Cartoes.Dtos;

namespace Patrimonio.Financas.Application.Cartoes.Mappers;

/// <summary>
/// Realiza o mapeamento de cartões para seus respectivos DTOs.
/// </summary>
internal static class CartaoMapper
{
    public static CartaoListaDto Mapear(CartaoListaReadModel cartao)
    {
        return new CartaoListaDto
        {
            Id = cartao.Id,
            Nome = cartao.Nome,
            Limite = cartao.Limite,
            DiaFechamento = cartao.DiaFechamento,
            DiaVencimento = cartao.DiaVencimento,
            InstituicaoNome = cartao.InstituicaoNome,
            DataCriacao = cartao.DataCriacao,
            DataAlteracao = cartao.DataAlteracao
        };
    }

    public static CartaoDetalheDto Mapear(CartaoDetalheReadModel cartao)
    {
        return new CartaoDetalheDto
        {
            Id = cartao.Id,
            Nome = cartao.Nome,
            Bandeira = cartao.Bandeira,
            Limite = cartao.Limite,
            DiaFechamento = cartao.DiaFechamento,
            DiaVencimento = cartao.DiaVencimento,
            InstituicaoId = cartao.InstituicaoId,
            InstituicaoNome = cartao.InstituicaoNome,
            DataCriacao = cartao.DataCriacao,
            DataAlteracao = cartao.DataAlteracao
        };
    }
}

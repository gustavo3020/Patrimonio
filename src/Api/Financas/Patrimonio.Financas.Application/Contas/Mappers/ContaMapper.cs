using Patrimonio.Financas.Application.Contas.Queries.ReadModels;
using Patrimonio.Financas.Contracts.Contas.Dtos;

namespace Patrimonio.Financas.Application.Contas.Mappers;

/// <summary>
/// Realiza o mapeamento de contas para seus respectivos DTOs.
/// </summary>
internal static class ContaMapper
{
    public static ContaListaDto Mapear(ContaListaReadModel conta)
    {
        return new ContaListaDto
        {
            Id = conta.Id,
            Nome = conta.Nome,
            InstituicaoNome = conta.InstituicaoNome,
            DataCriacao = conta.DataCriacao,
            DataAlteracao = conta.DataAlteracao
        };
    }

    public static ContaDetalheDto Mapear(ContaDetalheReadModel conta)
    {
        return new ContaDetalheDto
        {
            Id = conta.Id,
            Nome = conta.Nome,
            InstituicaoId = conta.InstituicaoId,
            InstituicaoNome = conta.InstituicaoNome,
            DataCriacao = conta.DataCriacao,
            DataAlteracao = conta.DataAlteracao
        };
    }
}

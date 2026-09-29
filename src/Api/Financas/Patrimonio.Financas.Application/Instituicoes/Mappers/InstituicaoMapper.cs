using Patrimonio.Financas.Application.Instituicoes.Queries.ReadModels;
using Patrimonio.Financas.Contracts.Instituicoes.Dtos;

namespace Patrimonio.Financas.Application.Instituicoes.Mappers;

/// <summary>
/// Realiza o mapeamento de instituições para seus respectivos DTOs.
/// </summary>
internal static class InstituicaoMapper
{
    public static InstituicaoListaDto Mapear(InstituicaoListaReadModel instituicao)
    {
        return new InstituicaoListaDto
        {
            Id = instituicao.Id,
            Nome = instituicao.Nome,
            DataCriacao = instituicao.DataCriacao,
            DataAlteracao = instituicao.DataAlteracao
        };
    }

    public static InstituicaoDetalheDto Mapear(InstituicaoDetalheReadModel instituicao)
    {
        return new InstituicaoDetalheDto
        {
            Id = instituicao.Id,
            Nome = instituicao.Nome,
            DataCriacao = instituicao.DataCriacao,
            DataAlteracao = instituicao.DataAlteracao
        };
    }
}

using Patrimonio.Financas.Application.Categorias.Queries.ReadModels;
using Patrimonio.Financas.Contracts.Categorias.Dtos;

namespace Patrimonio.Financas.Application.Categorias.Mappers;

/// <summary>
/// Realiza o mapeamento de categorias para seus respectivos DTOs.
/// </summary>
internal static class CategoriaMapper
{
    public static CategoriaListaDto Mapear(CategoriaListaReadModel categoria)
    {
        return new CategoriaListaDto
        {
            Id = categoria.Id,
            Nome = categoria.Nome,
            DataCriacao = categoria.DataCriacao,
            DataAlteracao = categoria.DataAlteracao
        };
    }

    public static CategoriaDetalheDto Mapear(CategoriaDetalheReadModel categoria)
    {
        return new CategoriaDetalheDto
        {
            Id = categoria.Id,
            Nome = categoria.Nome,
            DataCriacao = categoria.DataCriacao,
            DataAlteracao = categoria.DataAlteracao
        };
    }
}

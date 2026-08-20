using Patrimonio.Financas.Contracts.Common.Dtos;

namespace Patrimonio.Financas.Contracts.Categorias.Dtos;

/// <summary>
/// Representa os dados de uma categoria retornados em uma listagem.
/// </summary>
public sealed record CategoriaListaDto : LeituraDtoBase
{
    public required string Nome { get; init; }
}

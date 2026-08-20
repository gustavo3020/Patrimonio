using Patrimonio.Financas.Contracts.Common.Dtos;

namespace Patrimonio.Financas.Contracts.Categorias.Dtos;

/// <summary>
/// Representa os dados detalhados de uma categoria.
/// </summary>
public sealed record CategoriaDetalheDto : LeituraDtoBase
{
    public required string Nome { get; init; }
}

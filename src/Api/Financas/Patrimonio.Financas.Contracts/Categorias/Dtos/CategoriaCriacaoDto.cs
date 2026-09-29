namespace Patrimonio.Financas.Contracts.Categorias.Dtos;

/// <summary>
/// Define os dados necessários para criar uma categoria.
/// </summary>
public sealed record CategoriaCriacaoDto
{
    public required string Nome { get; init; }
}

namespace Patrimonio.Financas.Contracts.Categorias.Dtos;

/// <summary>
/// Representa os dados detalhados de uma categoria.
/// </summary>
public sealed record CategoriaDetalheDto
{
    public required int Id { get; init; }
    public required string Nome { get; init; }
}

namespace Patrimonio.Financas.Contracts.Categorias.Dtos;

/// <summary>
/// Representa os dados de uma categoria retornados em uma listagem.
/// </summary>
public sealed record CategoriaListaDto
{
    public required int Id { get; init; }
    public required string Nome { get; init; }
}

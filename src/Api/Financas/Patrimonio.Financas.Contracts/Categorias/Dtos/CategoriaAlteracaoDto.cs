namespace Patrimonio.Financas.Contracts.Categorias.Dtos;

/// <summary>
/// Define os dados necessários para alterar uma categoria.
/// </summary>
public sealed record CategoriaAlteracaoDto
{
    public required string Nome { get; init; }
}

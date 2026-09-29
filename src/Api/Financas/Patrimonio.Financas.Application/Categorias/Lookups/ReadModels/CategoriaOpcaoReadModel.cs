namespace Patrimonio.Financas.Application.Categorias.Lookups.ReadModels;

/// <summary>
/// Representa os dados de referência de uma categoria.
/// </summary>
public sealed record CategoriaOpcaoReadModel
{
    public required int Id { get; init; }
    public required string Nome { get; init; }
}

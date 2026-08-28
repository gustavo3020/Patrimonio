using Patrimonio.Financas.Application.Common.ReadModels;

namespace Patrimonio.Financas.Application.Categorias.Queries.ReadModels;

/// <summary>
/// Representa os dados de uma categoria necessários para uma listagem.
/// </summary>
public sealed record CategoriaListaReadModel : LeituraReadModelBase
{
    // Dados próprios.
    public required string Nome { get; init; }
}

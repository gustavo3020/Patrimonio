using Patrimonio.Financas.Application.Common.ReadModels;

namespace Patrimonio.Financas.Application.Categorias.Queries.ReadModels;

/// <summary>
/// Representa os dados de uma categoria necessários para sua visualização detalhada.
/// </summary>
public sealed record CategoriaDetalheReadModel : LeituraReadModelBase
{
    // Dados próprios.
    public required string Nome { get; init; }
}

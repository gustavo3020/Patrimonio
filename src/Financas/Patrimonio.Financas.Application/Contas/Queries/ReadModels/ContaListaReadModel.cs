using Patrimonio.Financas.Application.Common.ReadModels;

namespace Patrimonio.Financas.Application.Contas.Queries.ReadModels;

/// <summary>
/// Representa os dados de uma conta necessários para uma listagem.
/// </summary>
public sealed record ContaListaReadModel : LeituraReadModelBase
{
    // Dados próprios.
    public required string Nome { get; init; }

    // Relacionamentos.
    public required string InstituicaoNome { get; init; }
}

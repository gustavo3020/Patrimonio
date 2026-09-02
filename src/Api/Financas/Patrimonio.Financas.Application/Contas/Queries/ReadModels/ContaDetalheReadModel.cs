using Patrimonio.Financas.Application.Common.ReadModels;

namespace Patrimonio.Financas.Application.Contas.Queries.ReadModels;

/// <summary>
/// Representa os dados de uma conta necessários para sua visualização detalhada.
/// </summary>
public sealed record ContaDetalheReadModel : LeituraReadModelBase
{
    // Dados próprios.
    public required string Nome { get; init; }

    // Relacionamentos.
    public required int InstituicaoId { get; init; }
    public required string InstituicaoNome { get; init; }
}

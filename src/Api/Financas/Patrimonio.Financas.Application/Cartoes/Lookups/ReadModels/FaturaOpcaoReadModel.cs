namespace Patrimonio.Financas.Application.Cartoes.Lookups.ReadModels;

/// <summary>
/// Representa os dados de referência de uma fatura.
/// </summary>
public sealed record FaturaOpcaoReadModel
{
    public required int Id { get; init; }
    public required DateOnly DataVencimento { get; init; }
}

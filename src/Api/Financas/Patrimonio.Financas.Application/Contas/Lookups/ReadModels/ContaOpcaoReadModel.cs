namespace Patrimonio.Financas.Application.Contas.Lookups.ReadModels;

/// <summary>
/// Representa os dados de referência de uma conta.
/// </summary>
public sealed record ContaOpcaoReadModel
{
    public required int Id { get; init; }
    public required string Nome { get; init; }
}

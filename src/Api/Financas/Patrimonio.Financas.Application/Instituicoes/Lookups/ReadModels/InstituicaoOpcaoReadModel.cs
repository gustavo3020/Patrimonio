namespace Patrimonio.Financas.Application.Instituicoes.Lookups.ReadModels;

/// <summary>
/// Representa os dados de referência de uma instituição.
/// </summary>
public sealed record InstituicaoOpcaoReadModel
{
    public required int Id { get; init; }
    public required string Nome { get; init; }
}

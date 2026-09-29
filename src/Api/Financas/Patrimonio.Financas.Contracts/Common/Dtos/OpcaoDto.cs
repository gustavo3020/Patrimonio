namespace Patrimonio.Financas.Contracts.Common.Dtos;

/// <summary>
/// Representa uma opção de referência para um registro.
/// </summary>
public sealed record OpcaoDto
{
    public required int Id { get; init; }
    public required string Nome { get; init; }
}

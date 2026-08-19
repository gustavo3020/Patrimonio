namespace Patrimonio.Financas.Contracts.Instituicoes.Dtos;

/// <summary>
/// Representa os dados detalhados de uma instituição.
/// </summary>
public sealed record InstituicaoDetalheDto
{
    public required int Id { get; init; }
    public required string Nome { get; init; }
}

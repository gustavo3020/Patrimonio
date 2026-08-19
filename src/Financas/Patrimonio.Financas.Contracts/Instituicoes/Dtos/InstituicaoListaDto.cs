namespace Patrimonio.Financas.Contracts.Instituicoes.Dtos;

/// <summary>
/// Representa os dados de uma instituição retornados em uma listagem.
/// </summary>
public sealed record InstituicaoListaDto
{
    public required int Id { get; init; }
    public required string Nome { get; init; }
}

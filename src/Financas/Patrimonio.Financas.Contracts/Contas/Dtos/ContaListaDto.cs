namespace Patrimonio.Financas.Contracts.Contas.Dtos;

/// <summary>
/// Representa os dados de uma conta retornados em uma listagem.
/// </summary>
public sealed record ContaListaDto
{
    // Identificação.
    public required int Id { get; init; }

    // Dados próprios.
    public required string Nome { get; init; }

    // Relacionamentos.
    public required string InstituicaoNome { get; init; }
}

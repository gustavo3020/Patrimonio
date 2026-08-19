namespace Patrimonio.Financas.Contracts.Contas.Dtos;

/// <summary>
/// Representa os dados detalhados de uma conta.
/// </summary>
public sealed record ContaDetalheDto
{
    // Identificação.
    public required int Id { get; init; }

    // Dados próprios.
    public required string Nome { get; init; }

    // Relacionamentos.
    public required int InstituicaoId { get; init; }
    public required string InstituicaoNome { get; init; }
}

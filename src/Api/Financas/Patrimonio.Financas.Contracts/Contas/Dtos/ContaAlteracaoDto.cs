namespace Patrimonio.Financas.Contracts.Contas.Dtos;

/// <summary>
/// Define os dados necessários para alterar uma conta.
/// </summary>
public sealed record ContaAlteracaoDto
{
    // Dados próprios.
    public required string Nome { get; init; }

    // Relacionamentos.
    public required int InstituicaoId { get; init; }
}

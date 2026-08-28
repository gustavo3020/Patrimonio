namespace Patrimonio.Financas.Contracts.Contas.Dtos;

/// <summary>
/// Define os dados necessários para criar uma conta.
/// </summary>
public sealed record ContaCriacaoDto
{
    // Dados próprios.
    public required string Nome { get; init; }

    // Relacionamentos.
    public required int InstituicaoId { get; init; }
}

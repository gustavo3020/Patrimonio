namespace Patrimonio.Financas.Contracts.Instituicoes.Dtos;

/// <summary>
/// Define os dados necessários para criar uma instituição.
/// </summary>
public sealed record InstituicaoCriacaoDto
{
    public required string Nome { get; init; }
}

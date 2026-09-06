namespace Patrimonio.Financas.Contracts.Cartoes.Dtos;

/// <summary>
/// Define os dados necessários para alterar um lançamento.
/// </summary>
public sealed record LancamentoAlteracaoDto
{
    // Dados próprios.
    public required string Descricao { get; init; }
    public required decimal Valor { get; init; }
    public required DateOnly DataCompra { get; init; }
    public required string Estabelecimento { get; init; }
    public required string Responsavel { get; init; }

    // Relacionamentos.
    public required int CategoriaId { get; init; }
}

namespace Patrimonio.Financas.Contracts.Cartoes.Dtos;

/// <summary>
/// Define os dados necessários para criar um lançamento.
/// </summary>
public sealed record LancamentoCriacaoDto
{
    // Dados próprios.
    public required string Descricao { get; init; }
    public required decimal Valor { get; init; }
    public required DateOnly DataCompra { get; init; }
    public required string Estabelecimento { get; init; }
    public required string Responsavel { get; init; }
    public required string TotalParcelas { get; init; }

    // Relacionamentos.
    public required int FaturaId { get; init; }
    public required int CategoriaId { get; init; }
}

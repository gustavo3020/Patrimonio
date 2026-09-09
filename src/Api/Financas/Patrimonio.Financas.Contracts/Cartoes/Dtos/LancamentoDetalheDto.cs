using Patrimonio.Financas.Contracts.Common.Dtos;

namespace Patrimonio.Financas.Contracts.Cartoes.Dtos;

/// <summary>
/// Representa os dados detalhados de um lançamento.
/// </summary>
public sealed record LancamentoDetalheDto : LeituraDtoBase
{
    // Dados próprios.
    public required string Descricao { get; init; }
    public required decimal Valor { get; init; }
    public required DateOnly DataCompra { get; init; }
    public required string Estabelecimento { get; init; }
    public required string Responsavel { get; init; }
    public required int NumeroParcela { get; init; }
    public required int TotalParcelas { get; init; }

    // Relacionamentos.
    public required int FaturaId { get; init; }
    public required int CategoriaId { get; init; }
    public required string CategoriaNome { get; init; }
}

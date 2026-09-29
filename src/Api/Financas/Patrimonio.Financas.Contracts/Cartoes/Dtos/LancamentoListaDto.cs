using Patrimonio.Financas.Contracts.Common.Dtos;

namespace Patrimonio.Financas.Contracts.Cartoes.Dtos;

/// <summary>
/// Representa os dados de um lançamento retornados em uma listagem.
/// </summary>
public sealed record LancamentoListaDto : LeituraDtoBase
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
    public required string CategoriaNome { get; init; }
}

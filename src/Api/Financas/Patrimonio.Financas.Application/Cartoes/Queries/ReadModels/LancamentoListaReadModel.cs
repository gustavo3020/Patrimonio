using Patrimonio.Financas.Application.Common.ReadModels;

namespace Patrimonio.Financas.Application.Cartoes.Queries.ReadModels;

/// <summary>
/// Representa os dados de um lançamento necessários para uma listagem.
/// </summary>
public sealed record LancamentoListaReadModel : LeituraReadModelBase
{
    // Dados próprios.
    public required string Descricao { get; init; }
    public required decimal Valor { get; init; }
    public required DateOnly DataCompra { get; init; }
    public required string Estabelecimento { get; init; }
    public required string Responsavel { get; init; }
    public required string NumeroParcela { get; init; }
    public required string TotalParcelas { get; init; }

    // Relacionamentos.
    public required string FaturaNome { get; init; }
    public required string CategoriaNome { get; init; }
}

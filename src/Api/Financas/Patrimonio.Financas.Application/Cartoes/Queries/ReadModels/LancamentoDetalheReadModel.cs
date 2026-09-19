using Patrimonio.Financas.Application.Common.ReadModels;

namespace Patrimonio.Financas.Application.Cartoes.Queries.ReadModels;

/// <summary>
/// Representa os dados de um lançamento necessários para sua visualização detalhada.
/// </summary>
public sealed record LancamentoDetalheReadModel : LeituraReadModelBase
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

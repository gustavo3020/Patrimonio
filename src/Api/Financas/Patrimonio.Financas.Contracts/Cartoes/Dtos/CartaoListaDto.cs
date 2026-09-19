using Patrimonio.Financas.Contracts.Common.Dtos;

namespace Patrimonio.Financas.Contracts.Cartoes.Dtos;

/// <summary>
/// Representa os dados de um cartão retornados em uma listagem.
/// </summary>
public sealed record CartaoListaDto : LeituraDtoBase
{
    // Dados próprios.
    public required string Nome { get; init; }
    public required decimal Limite { get; init; }
    public required int DiaFechamento { get; init; }
    public required int DiaVencimento { get; init; }

    // Relacionamentos.
    public required string InstituicaoNome { get; init; }
}

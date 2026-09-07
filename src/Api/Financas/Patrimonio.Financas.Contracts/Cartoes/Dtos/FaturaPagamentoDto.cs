namespace Patrimonio.Financas.Contracts.Cartoes.Dtos;

/// <summary>
/// Representa os dados necessários para registrar o pagamento de uma fatura.
/// </summary>
public sealed record FaturaPagamentoDto
{
    // Dados próprios.
    public required DateOnly DataPagamento { get; init; }

    // Relacionamentos.
    public required int ContaId { get; init; }
    public required int CategoriaId { get; init; }
}

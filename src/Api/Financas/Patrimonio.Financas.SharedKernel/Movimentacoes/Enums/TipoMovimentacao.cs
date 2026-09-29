namespace Patrimonio.Financas.SharedKernel.Movimentacoes.Enums;

/// <summary>
/// Representa o tipo de pagamento de uma movimentação.
/// </summary>
public enum TipoMovimentacao
{
    Especie = 1,
    Pix = 2,
    Transferencia = 3,
    Boleto = 4,
    Debito = 5,
    Credito = 6
}

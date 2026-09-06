using Patrimonio.Financas.Domain.Exceptions;

namespace Patrimonio.Financas.Domain.Cartoes.ValueObjects;

/// <summary>
/// Representa o parcelamento de uma compra no domínio de cartões de crédito.
/// </summary>
public sealed record Parcelamento
{
    /// <summary>
    /// Cria uma instância de Parcelamento com validação dos parâmetros.
    /// </summary>
    /// <param name="grupoId">O ID do grupo de parcelamento.</param>
    /// <param name="numeroParcela">O número da parcela.</param>
    /// <param name="totalParcelas">O número total de parcelas.</param>
    /// <returns>Uma instância de <see cref="Parcelamento"/>.</returns>
    /// <exception cref="RegraDeNegocioException">
    /// Lançada quando os parâmetros não são válidos.
    /// </exception>
    public Parcelamento(Guid grupoId, int numeroParcela, int totalParcelas)
    {
        if (grupoId == Guid.Empty)
            throw new RegraDeNegocioException("O ID do grupo de parcelamento não pode ser vazio.");

        if (numeroParcela <= 0)
            throw new RegraDeNegocioException("O número da parcela deve ser maior que zero.");

        if (totalParcelas <= 0)
            throw new RegraDeNegocioException("O número total de parcelas deve ser maior que zero.");

        if (numeroParcela > totalParcelas)
            throw new RegraDeNegocioException("O número da parcela não pode ser maior que o total de parcelas.");

        GrupoId = grupoId;
        NumeroParcela = numeroParcela;
        TotalParcelas = totalParcelas;
    }

    public Guid GrupoId { get; }
    public int NumeroParcela { get; }
    public int TotalParcelas { get; }
}

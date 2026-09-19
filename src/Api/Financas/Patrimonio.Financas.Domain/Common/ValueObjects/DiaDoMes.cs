using Patrimonio.Financas.Domain.Exceptions;

namespace Patrimonio.Financas.Domain.Common.ValueObjects;

/// <summary>
/// Representa um dia do mês.
/// </summary>
public sealed record DiaDoMes
{
    /// <summary>
    /// Cria uma instância de DiaDoMes.
    /// </summary>
    /// <param name="valor">Número do dia do mês.</param>
    /// <returns>Uma instância de <see cref="DiaDoMes"/>.</returns>
    /// <exception cref="RegraDeNegocioException">
    /// Lançada quando o valor não está entre 1 e 31.
    /// </exception>
    public DiaDoMes(int valor)
    {
        if (valor < 1 || valor > 31)
            throw new RegraDeNegocioException("O dia deve estar entre 1 e 31.");

        Valor = valor;
    }

    public int Valor { get; }

    /// <inheritdoc/>
    public override string ToString() => Valor.ToString();
}

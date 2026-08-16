using Patrimonio.Financas.Domain.Exceptions;
using System.Globalization;

namespace Patrimonio.Financas.Domain.Movimentacoes.ValueObjects;

/// <summary>
/// Representa um valor monetário válido dentro do domínio financeiro.
/// </summary>
public sealed record Dinheiro
{
    public decimal Valor { get; }

    /// <summary>
    /// Inicializa uma nova instância de <see cref="Dinheiro"/>.
    /// </summary>
    /// <param name="valor">Valor monetário.</param>
    /// <exception cref="RegraDeNegocioException">
    /// Lançada quando o valor é menor ou igual a zero ou possui mais de duas casas decimais.
    /// </exception>
    public Dinheiro(decimal valor)
    {
        if (valor <= 0)
            throw new RegraDeNegocioException("O valor deve ser maior que zero.");

        if (decimal.Round(valor, 2) != valor)
            throw new RegraDeNegocioException("O valor deve possuir no máximo duas casas decimais.");

        Valor = valor;
    }

    /// <inheritdoc/>
    public override string ToString() => Valor.ToString("F2", CultureInfo.InvariantCulture);
}

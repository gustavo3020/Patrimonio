using Patrimonio.Financas.Domain.Exceptions;

namespace Patrimonio.Financas.Domain.Common.ValueObjects;

/// <summary>
/// Representa um nome válido utilizado pelas entidades do domínio.
/// </summary>
public sealed record Nome
{
    public string Valor { get; }
    public string Normalizado { get; }

    /// <summary>
    /// Inicializa uma nova instância de <see cref="Nome"/>.
    /// </summary>
    /// <param name="valor">Valor do nome.</param>
    /// <exception cref="RegraDeNegocioException">
    /// Lançada quando o nome não é informado ou ultrapassa o tamanho máximo permitido.
    /// </exception>
    public Nome(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new RegraDeNegocioException("O nome deve ser informado.");

        Valor = valor.Trim();

        if (Valor.Length > 200)
            throw new RegraDeNegocioException("O nome deve possuir no máximo 200 caracteres.");

        Normalizado = Valor.ToUpperInvariant();
    }

    /// <inheritdoc/>
    public override string ToString() => Valor;
}

using Patrimonio.Financas.Domain.Exceptions;

namespace Patrimonio.Financas.Domain.Common.ValueObjects;

/// <summary>
/// Representa um nome válido utilizado pelas entidades do domínio.
/// </summary>
public sealed record Nome : Texto
{
    /// <summary>
    /// Inicializa uma nova instância de <see cref="Nome"/>.
    /// </summary>
    /// <param name="valor">Valor do nome.</param>
    /// <returns>Uma instância de <see cref="Nome"/>.</returns>
    /// <exception cref="RegraDeNegocioException">
    /// Lançada quando o nome não é informado ou ultrapassa o tamanho máximo permitido.
    /// </exception>
    public Nome(string valor) : base(valor, 200, "nome")
    {
        Normalizado = Valor.ToUpperInvariant();
    }

    public string Normalizado { get; }

    /// <inheritdoc/>
    public override string ToString() => Valor;
}

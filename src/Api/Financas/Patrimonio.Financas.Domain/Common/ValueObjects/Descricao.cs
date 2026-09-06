namespace Patrimonio.Financas.Domain.Common.ValueObjects;

/// <summary>
/// Representa uma descrição válida utilizada pelas entidades do domínio.
/// </summary>
/// <param name="Valor">O valor da descrição.</param>
public sealed record Descricao(string Valor) : Texto(Valor, 500, "descrição")
{
    /// <inheritdoc/>
    public override string ToString() => Valor;
}

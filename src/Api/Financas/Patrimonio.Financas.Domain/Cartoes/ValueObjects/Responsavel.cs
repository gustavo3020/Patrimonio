using Patrimonio.Financas.Domain.Common.ValueObjects;

namespace Patrimonio.Financas.Domain.Cartoes.ValueObjects;

/// <summary>
/// Representa um responsável válido utilizado pelas entidades do domínio.
/// </summary>
/// <param name="Valor">O nome do responsável.</param>
public sealed record Responsavel(string Valor) : Texto(Valor, 100, "responsável")
{
    /// <inheritdoc/>
    public override string ToString() => Valor;
}

using Patrimonio.Financas.Domain.Common.ValueObjects;

namespace Patrimonio.Financas.Domain.Cartoes.ValueObjects;

/// <summary>
/// Representa um estabelecimento válido utilizado pelas entidades do domínio.
/// </summary>
/// <param name="Valor">O nome do estabelecimento.</param>
public sealed record Estabelecimento(string Valor) : Texto(Valor, 150, "estabelecimento")
{
    /// <inheritdoc/>
    public override string ToString() => Valor;
}

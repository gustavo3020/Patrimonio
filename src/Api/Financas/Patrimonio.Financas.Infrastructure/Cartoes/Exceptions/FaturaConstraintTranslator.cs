using Patrimonio.Financas.Infrastructure.Common.Exceptions;

namespace Patrimonio.Financas.Infrastructure.Cartoes.Exceptions;

/// <summary>
/// Traduz as violações de constraints do banco de dados relacionadas a faturas
/// para mensagens de erro compreensíveis pela aplicação.
/// </summary>
internal sealed class FaturaConstraintTranslator : IConstraintTranslator
{
    private static readonly Dictionary<string, string> Messages = new()
    {
        // Chaves estrangeiras
        ["FK_Faturas_Cartoes_CartaoId"] = "Não é possível excluir o cartão pois existem faturas vinculadas.",
    };

    /// <inheritdoc/>
    public bool TentarTraduzir(string constraintName, out string mensagem)
    {
        return Messages.TryGetValue(constraintName, out mensagem!);
    }
}

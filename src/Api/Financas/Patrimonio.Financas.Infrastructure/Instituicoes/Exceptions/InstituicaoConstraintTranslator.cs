using Patrimonio.Financas.Infrastructure.Common.Exceptions;

namespace Patrimonio.Financas.Infrastructure.Instituicoes.Exceptions;

/// <summary>
/// Traduz as violações de constraints do banco de dados relacionadas a instituições
/// para mensagens de erro compreensíveis pela aplicação.
/// </summary>
internal sealed class InstituicaoConstraintTranslator : IConstraintTranslator
{
    private static readonly Dictionary<string, string> Messages = new()
    {
        // Índices únicos
        ["IX_Instituicoes_NomeNormalizado"] = "Já existe uma instituição com esse nome."
    };

    /// <inheritdoc/>
    public bool TentarTraduzir(string constraintName, out string mensagem)
    {
        return Messages.TryGetValue(constraintName, out mensagem!);
    }
}

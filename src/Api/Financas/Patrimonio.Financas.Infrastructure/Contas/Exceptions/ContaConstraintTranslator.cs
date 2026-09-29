using Patrimonio.Financas.Infrastructure.Common.Exceptions;

namespace Patrimonio.Financas.Infrastructure.Contas.Exceptions;

/// <summary>
/// Traduz as violações de constraints do banco de dados relacionadas a contas
/// para mensagens de erro compreensíveis pela aplicação.
/// </summary>
internal sealed class ContaConstraintTranslator : IConstraintTranslator
{
    private static readonly Dictionary<string, string> Messages = new()
    {
        // Chaves estrangeiras
        ["FK_Contas_Instituicoes_InstituicaoId"] = "Não é possível excluir a instituição pois existem contas vinculadas.",

        // Índices únicos
        ["IX_Contas_NomeNormalizado"] = "Já existe uma conta com esse nome."
    };

    /// <inheritdoc/>
    public bool TentarTraduzir(string constraintName, out string mensagem)
    {
        return Messages.TryGetValue(constraintName, out mensagem!);
    }
}

using Patrimonio.Financas.Infrastructure.Common.Exceptions;

namespace Patrimonio.Financas.Infrastructure.Movimentacoes.Exceptions;

/// <summary>
/// Traduz as violações de constraints do banco de dados relacionadas a movimentações
/// para mensagens de erro compreensíveis pela aplicação.
/// </summary>
internal sealed class MovimentacaoConstraintTranslator : IConstraintTranslator
{
    private static readonly Dictionary<string, string> Messages = new()
    {
        // Chaves estrangeiras
        ["FK_Movimentacoes_Categorias_CategoriaId"] = "Não é possível excluir a categoria pois existem movimentações vinculadas.",
        ["FK_Movimentacoes_Contas_ContaId"] = "Não é possível excluir a conta pois existem movimentações vinculadas."
    };

    /// <inheritdoc/>
    public bool TentarTraduzir(string constraintName, out string mensagem)
    {
        return Messages.TryGetValue(constraintName, out mensagem!);
    }
}

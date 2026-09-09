using Patrimonio.Financas.Infrastructure.Common.Exceptions;

namespace Patrimonio.Financas.Infrastructure.Cartoes.Exceptions;

/// <summary>
/// Traduz as violações de constraints do banco de dados relacionadas a lançamentos
/// para mensagens de erro compreensíveis pela aplicação.
/// </summary>
internal sealed class LancamentoConstraintTranslator : IConstraintTranslator
{
    private static readonly Dictionary<string, string> Messages = new()
    {
        // Chaves estrangeiras
        ["FK_Lancamentos_Categorias_CategoriaId"] = "Não é possível excluir a categoria pois existem lançamentos vinculados.",
        ["FK_Lancamentos_Faturas_FaturaId"] = "Não é possível excluir a fatura pois existem lançamentos vinculados.",
    };

    /// <inheritdoc/>
    public bool TentarTraduzir(string constraintName, out string mensagem)
    {
        return Messages.TryGetValue(constraintName, out mensagem!);
    }
}

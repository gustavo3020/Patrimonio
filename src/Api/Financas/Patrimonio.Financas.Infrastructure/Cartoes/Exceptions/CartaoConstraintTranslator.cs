using Patrimonio.Financas.Infrastructure.Common.Exceptions;

namespace Patrimonio.Financas.Infrastructure.Cartoes.Exceptions;

/// <summary>
/// Traduz as violações de constraints do banco de dados relacionadas a cartões
/// para mensagens de erro compreensíveis pela aplicação.
/// </summary>
internal sealed class CartaoConstraintTranslator : IConstraintTranslator
{
    private static readonly Dictionary<string, string> Messages = new()
    {
        // Chaves estrangeiras
        ["FK_Cartoes_Instituicoes_InstituicaoId"] = "Não é possível excluir a instituição pois existem cartões vinculados.",

        // Índices únicos
        ["IX_Cartoes_NomeNormalizado"] = "Já existe um cartão com esse nome."
    };

    /// <inheritdoc/>
    public bool TentarTraduzir(string constraintName, out string mensagem)
    {
        return Messages.TryGetValue(constraintName, out mensagem!);
    }
}

using Patrimonio.Financas.Infrastructure.Common.Exceptions;

namespace Patrimonio.Financas.Infrastructure.Categorias.Exceptions;

/// <summary>
/// Traduz as violações de constraints do banco de dados relacionadas a categorias
/// para mensagens de erro compreensíveis pela aplicação.
/// </summary>
internal sealed class CategoriaConstraintTranslator : IConstraintTranslator
{
    private static readonly Dictionary<string, string> Messages = new()
    {
        // Índices únicos
        ["IX_Categorias_NomeNormalizado"] = "Já existe uma categoria com esse nome."
    };

    /// <inheritdoc/>
    public bool TentarTraduzir(string constraintName, out string mensagem)
    {
        return Messages.TryGetValue(constraintName, out mensagem!);
    }
}

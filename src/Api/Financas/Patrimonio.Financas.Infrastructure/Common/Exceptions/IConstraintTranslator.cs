namespace Patrimonio.Financas.Infrastructure.Common.Exceptions;

/// <summary>
/// Define o contrato para tradutores de constraints do banco de dados.
/// </summary>
internal interface IConstraintTranslator
{
    /// <summary>
    /// Tenta traduzir uma constraint do banco de dados para uma mensagem compreensível pela aplicação.
    /// </summary>
    /// <param name="constraintName">Nome da constraint violada.</param>
    /// <param name="mensagem">
    /// Quando a tradução é encontrada, recebe a mensagem correspondente à constraint.
    /// </param>
    /// <returns>
    /// <see langword="true"/> quando existe uma tradução para a constraint;
    /// caso contrário, <see langword="false"/>.
    /// </returns>
    bool TentarTraduzir(string constraintName, out string mensagem);
}

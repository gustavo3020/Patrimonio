using Npgsql;

namespace Patrimonio.Financas.Infrastructure.Common.Exceptions;

/// <summary>
/// Traduz exceções de persistência do banco de dados para exceções compreensíveis pela aplicação,
/// utilizando os tradutores de constraints registrados na infraestrutura.
/// </summary>
/// <param name="tradutores">Tradutores de constraints registrados pelas funcionalidades.</param>
internal sealed class DatabaseConstraintTranslator(IEnumerable<IConstraintTranslator> tradutores)
{
    /// <summary>
    /// Traduz uma exceção do PostgreSQL utilizando os tradutores de constraints disponíveis.
    /// </summary>
    /// <param name="exception">Exceção lançada pelo PostgreSQL durante a operação de persistência.</param>
    /// <returns>
    /// Uma exceção traduzida quando a constraint possui uma mensagem configurada;
    /// caso contrário, a própria exceção original.
    /// </returns>
    public Exception Traduzir(PostgresException exception)
    {
        if (exception.ConstraintName is null)
            return exception;

        foreach (var translator in tradutores)
        {
            if (translator.TentarTraduzir(exception.ConstraintName, out var message))
            {
                return new ConflitoException(message);
            }
        }

        return exception;
    }
}

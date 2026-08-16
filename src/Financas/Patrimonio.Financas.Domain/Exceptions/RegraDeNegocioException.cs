namespace Patrimonio.Financas.Domain.Exceptions;

/// <summary>
/// Representa uma violação de uma regra de negócio do domínio.
/// </summary>
public sealed class RegraDeNegocioException(string message) : Exception(message)
{
}

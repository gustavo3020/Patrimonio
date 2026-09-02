namespace Patrimonio.Financas.Domain.Exceptions;

/// <summary>
/// Exceção lançada quando ocorre a violação de uma regra de negócio do domínio.
/// </summary>
public sealed class RegraDeNegocioException(string mensagem) : Exception(mensagem)
{
}

namespace Patrimonio.Financas.Infrastructure.Common.Exceptions;

/// <summary>
/// Representa uma exceção lançada quando um recurso solicitado não é encontrado.
/// </summary>
public sealed class RecursoNaoEncontradoException(string mensagem) : Exception(mensagem)
{
}

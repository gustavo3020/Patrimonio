namespace Patrimonio.Financas.Domain.Exceptions;

/// <summary>
/// Exceção lançada quando uma operação viola uma regra de unicidade
/// ou gera conflito com dados já existentes.
/// </summary>
public sealed class ConflitoException(string mensagem) : Exception(mensagem)
{
}

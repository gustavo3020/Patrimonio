namespace Patrimonio.Financas.Infrastructure.Common.Exceptions;

/// <summary>
/// Exceção lançada quando uma configuração obrigatória não é encontrada.
/// </summary>
public sealed class ConfiguracaoInvalidaException(string mensagem) : Exception(mensagem)
{
}

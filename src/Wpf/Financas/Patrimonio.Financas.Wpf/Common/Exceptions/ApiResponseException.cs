namespace Patrimonio.Financas.Wpf.Common.Exceptions;

/// <summary>
/// Representa uma resposta bem-sucedida da API cujo conteúdo não pôde ser
/// convertido para o tipo de dados esperado pelo cliente.
/// </summary>
public sealed class ApiResponseException(string message)
    : Exception(message);

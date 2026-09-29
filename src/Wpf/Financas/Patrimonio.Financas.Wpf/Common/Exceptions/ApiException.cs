namespace Patrimonio.Financas.Wpf.Common.Exceptions;

/// <summary>
/// Representa uma exceção causada por uma resposta de erro retornada pela API.
/// </summary>
/// <param name="statusCode">Código de status HTTP retornado pela API.</param>
/// <param name="message">Mensagem de erro retornada ou gerada para a resposta.</param>
/// <param name="title">Título associado ao problema retornado pela API, quando disponível.</param>
/// <param name="traceId">
/// Identificador utilizado para rastrear a requisição na API, quando disponível.
/// </param>
public sealed class ApiException(
    int statusCode,
    string message,
    string? title = null,
    string? traceId = null)
    : Exception(message)
{
    /// <summary>
    /// Obtém o código de status HTTP retornado pela API.
    /// </summary>
    public int StatusCode { get; } = statusCode;

    /// <summary>
    /// Obtém o título associado ao erro retornado pela API, quando disponível.
    /// </summary>
    public string? Title { get; } = title;

    /// <summary>
    /// Obtém o identificador de rastreamento da requisição na API, quando disponível.
    /// </summary>
    public string? TraceId { get; } = traceId;
}

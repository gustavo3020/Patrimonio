namespace Patrimonio.Financas.Wpf.Common.Http;

/// <summary>
/// Representa os dados de problema retornados pela API em respostas de erro.
/// </summary>
internal sealed class ApiProblemDetails
{
    /// <summary>
    /// Obtém o título do problema.
    /// </summary>
    public string? Title { get; init; }

    /// <summary>
    /// Obtém a descrição detalhada do problema.
    /// </summary>
    public string? Detail { get; init; }

    /// <summary>
    /// Obtém o identificador utilizado para rastrear a requisição na API.
    /// </summary>
    public string? TraceId { get; init; }
}

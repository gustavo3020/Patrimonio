using Patrimonio.Financas.Wpf.Common.Exceptions;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace Patrimonio.Financas.Wpf.Common.Http;

/// <summary>
/// Intercepta as respostas HTTP da API e converte respostas de erro
/// em exceções específicas do cliente.
/// </summary>
internal sealed class ApiResponseHandler : DelegatingHandler
{
    /// <summary>
    /// Envia uma requisição HTTP e trata respostas que indiquem falha da API.
    /// </summary>
    /// <param name="request">Requisição HTTP a ser enviada.</param>
    /// <param name="cancellationToken">
    /// Token utilizado para cancelar a operação.
    /// </param>
    /// <returns>
    /// A resposta HTTP quando a operação é concluída com sucesso.
    /// </returns>
    /// <exception cref="ApiException">
    /// Lançada quando a API retorna uma resposta com status HTTP de erro.
    /// </exception>
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var resposta = await base.SendAsync(request, cancellationToken);

        if (resposta.IsSuccessStatusCode)
            return resposta;

        var statusCode = (int)resposta.StatusCode;

        ApiProblemDetails? problem = null;

        try
        {
            problem = await resposta.Content.ReadFromJsonAsync<ApiProblemDetails>(
                cancellationToken);
        }
        catch (JsonException)
        {
            // Resposta de erro não está no formato esperado.
        }
        finally
        {
            resposta.Dispose();
        }

        throw new ApiException(
            statusCode,
            problem?.Detail ?? $"A API retornou o status {statusCode}.",
            problem?.Title,
            problem?.TraceId);
    }
}

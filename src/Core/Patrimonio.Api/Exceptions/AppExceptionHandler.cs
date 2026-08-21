using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Patrimonio.Financas.Domain.Exceptions;
using Patrimonio.Financas.Infrastructure.Common.Exceptions;

namespace Patrimonio.Api.Exceptions;

/// <summary>
/// Trata exceções conhecidas da aplicação e retorna respostas padronizadas
/// de acordo com o tipo de erro ocorrido.
/// </summary>
public sealed class AppExceptionHandler(ILogger<AppExceptionHandler> logger) : IExceptionHandler
{
    /// <inheritdoc/>
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var (status, title) = exception switch
        {
            RegraDeNegocioException => (StatusCodes.Status400BadRequest, "Regra de negócio violada"),
            RecursoNaoEncontradoException => (StatusCodes.Status404NotFound, "Recurso não encontrado"),
            ConflitoException => (StatusCodes.Status409Conflict, "Conflito de dados"),
            ConfiguracaoInvalidaException => (StatusCodes.Status500InternalServerError, "Configuração inválida"),
            _ => (0, null)
        };

        if (status == 0)
            return false;

        var traceId = httpContext.TraceIdentifier;

        logger.LogWarning(exception, "Erro de aplicação. | TraceId: {TraceId} | Status: {Status}", traceId, status);

        var problemDetails = new ProblemDetails
        {
            Status = status,
            Title = title,
            Detail = exception.Message,
            Instance = httpContext.Request.Path
        };

        problemDetails.Extensions["traceId"] = traceId;

        httpContext.Response.StatusCode = status;

        await httpContext.Response.WriteAsJsonAsync(
            problemDetails,
            cancellationToken);

        return true;
    }
}

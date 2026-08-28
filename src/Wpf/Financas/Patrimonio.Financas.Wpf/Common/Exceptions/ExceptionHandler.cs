using Microsoft.Extensions.Logging;
using Patrimonio.Financas.Wpf.Common.Dialogs;
using System.Net.Http;

namespace Patrimonio.Financas.Wpf.Common.Exceptions;

/// <summary>
/// Serviço responsável pelo tratamento centralizado de exceções.
/// </summary>
internal sealed class ExceptionHandler(
    IDialogService dialogService,
    ILogger<ExceptionHandler> logger)
{
    /// <summary>
    /// Trata uma exceção e apresenta a mensagem apropriada ao usuário.
    /// </summary>
    /// <param name="exception">Exceção ocorrida durante a operação.</param>
    public void Handle(Exception exception)
    {
        switch (exception)
        {
            case ApiException apiException:
                dialogService.ShowError(apiException.Message);
                logger.LogInformation("Erro de API: {Erro}", apiException.Message);
                break;

            case HttpRequestException:
                dialogService.ShowError("Não foi possível se comunicar com a API.");
                logger.LogWarning("Não foi possível se comunicar com a API.");
                break;

            case OperationCanceledException:
                break;

            default:
                dialogService.ShowError("Ocorreu um erro inesperado. Tente novamente.");
                logger.LogError(exception, "Erro inesperado não tratado no cliente.");
                break;
        }
    }
}

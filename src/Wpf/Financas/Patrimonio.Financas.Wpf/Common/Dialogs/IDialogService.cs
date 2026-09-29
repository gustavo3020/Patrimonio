namespace Patrimonio.Financas.Wpf.Common.Dialogs;

/// <summary>
/// Fornece operações centralizadas para exibição de diálogos na aplicação.
/// </summary>
public interface IDialogService
{
    /// <summary>
    /// Exibe uma mensagem de erro ao usuário.
    /// </summary>
    /// <param name="message">Mensagem de erro a ser exibida.</param>
    void ShowError(string message);

    /// <summary>
    /// Exibe uma mensagem informativa ao usuário.
    /// </summary>
    /// <param name="message">Mensagem a ser exibida.</param>
    void ShowInformation(string message);

    /// <summary>
    /// Exibe uma mensagem de aviso ao usuário.
    /// </summary>
    /// <param name="message">Mensagem de aviso a ser exibida.</param>
    void ShowWarning(string message);

    /// <summary>
    /// Exibe uma pergunta de confirmação ao usuário.
    /// </summary>
    /// <param name="message">Mensagem da confirmação.</param>
    /// <returns><see langword="true"/> quando o usuário confirmar a operação.</returns>
    bool ShowConfirmation(string message);
}

using System.Windows;

namespace Patrimonio.Financas.Wpf.Common.Dialogs;

/// <summary>
/// Fornece operações centralizadas para exibição de diálogos na aplicação.
/// </summary>
internal sealed class DialogService : IDialogService
{
    /// <inheritdoc/>
    public void ShowError(string message)
    {
        MessageBox.Show(
            message,
            "Erro",
            MessageBoxButton.OK,
            MessageBoxImage.Error);
    }

    /// <inheritdoc/>
    public void ShowInformation(string message)
    {
        MessageBox.Show(
            message,
            "Informação",
            MessageBoxButton.OK,
            MessageBoxImage.Information);
    }


    /// <inheritdoc/>
    public void ShowWarning(string message)
    {
        MessageBox.Show(
            message,
            "Atenção",
            MessageBoxButton.OK,
            MessageBoxImage.Warning);
    }

    /// <inheritdoc/>
    public bool ShowConfirmation(string message)
    {
        return MessageBox.Show(
            message,
            "Confirmação",
            MessageBoxButton.YesNoCancel,
            MessageBoxImage.Question) == MessageBoxResult.Yes;
    }
}

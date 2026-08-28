using CommunityToolkit.Mvvm.Input;
using Patrimonio.Financas.Wpf.Common.Navigation;
using Patrimonio.Financas.Wpf.Navigation;

namespace Patrimonio.Wpf.Navigation;

/// <summary>
/// Responsável pela navegação principal entre os módulos da aplicação.
/// </summary>
public sealed partial class AppNavigation(
    FinancasNavigation financasNavigation) : NavigationBase
{
    /// <summary>
    /// Abre o módulo de finanças.
    /// </summary>
    [RelayCommand]
    private void AbrirFinancas()
    {
        ConteudoAtual = financasNavigation;
    }
}

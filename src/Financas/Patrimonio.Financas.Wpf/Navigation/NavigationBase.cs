using CommunityToolkit.Mvvm.ComponentModel;

namespace Patrimonio.Financas.Wpf.Common.Navigation;

/// <summary>
/// Define o comportamento e o estado comuns às estruturas de navegação da aplicação.
/// </summary>
public abstract partial class NavigationBase : ObservableObject
{
    /// <summary>
    /// Obtém o conteúdo atualmente selecionado pela navegação.
    /// </summary>
    [ObservableProperty]
    private object? conteudoAtual;
}

using CommunityToolkit.Mvvm.ComponentModel;

namespace Patrimonio.Financas.Wpf.Common.ViewModels;

/// <summary>
/// Define o comportamento e o estado comuns aos ViewModels da aplicação.
/// </summary>
internal abstract partial class BaseViewModel : ObservableObject
{
    /// <summary>
    /// Obtém o título apresentado pela tela associada ao ViewModel.
    /// </summary>
    public abstract string Titulo { get; }

    /// <summary>
    /// Indica se o ViewModel está executando uma operação assíncrona.
    /// </summary>
    [ObservableProperty] protected bool carregando;
}

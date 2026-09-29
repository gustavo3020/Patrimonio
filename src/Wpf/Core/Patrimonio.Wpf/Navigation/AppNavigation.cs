using Patrimonio.Financas.Wpf.Common.Navigation;
using Patrimonio.Financas.Wpf.Navigation;

namespace Patrimonio.Wpf.Navigation;

/// <summary>
/// Responsável pela navegação principal entre os módulos da aplicação.
/// </summary>
public sealed partial class AppNavigation(
    FinancasNavigation financasNavigation) : NavigationBase
{
    public string Titulo { get; } = "Patrimonio";

    public FinancasNavigation FinancasNavigation { get; } = financasNavigation;

    public void Inicializar()
    {
        FinancasNavigation.Inicializar();

        FinancasNavigation.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(FinancasNavigation.ConteudoAtual))
            {
                ConteudoAtual = FinancasNavigation.ConteudoAtual;
            }
        };
    }
}

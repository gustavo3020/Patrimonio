using Microsoft.Extensions.DependencyInjection;
using Patrimonio.Financas.Wpf.Common.Navigation;
using Patrimonio.Financas.Wpf.Contas.ViewModels;

namespace Patrimonio.Financas.Wpf.Contas.Navigation;

/// <summary>
/// Responsável pela navegação da funcionalidade de contas.
/// </summary>
public sealed partial class ContaNavigation(
    IServiceProvider serviceProvider) : NavigationBase
{
    /// <summary>
    /// Abre a tela de alteração da conta selecionada.
    /// </summary>
    public async Task AbrirAlteracaoAsync(int contaId, CancellationToken cancellationToken)
    {
        var viewModel = serviceProvider.GetRequiredService<ContaAlteracaoViewModel>();

        await viewModel.InicializarAsync(contaId, cancellationToken);

        ConteudoAtual = viewModel;
    }

    /// <summary>
    /// Abre a tela de criação de conta.
    /// </summary>
    public async Task AbrirCriacaoAsync(CancellationToken cancellationToken)
    {
        var viewModel = serviceProvider.GetRequiredService<ContaCriacaoViewModel>();

        await viewModel.InicializarAsync(cancellationToken);

        ConteudoAtual = viewModel;
    }

    /// <summary>
    /// Abre a tela de listagem de contas.
    /// </summary>
    public async Task AbrirListaAsync(CancellationToken cancellationToken)
    {
        var viewModel = serviceProvider.GetRequiredService<ContaListaViewModel>();

        await viewModel.InicializarAsync(cancellationToken);

        ConteudoAtual = viewModel;
    }
}

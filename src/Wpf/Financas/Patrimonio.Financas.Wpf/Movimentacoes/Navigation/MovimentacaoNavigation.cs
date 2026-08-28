using Microsoft.Extensions.DependencyInjection;
using Patrimonio.Financas.Wpf.Common.Navigation;
using Patrimonio.Financas.Wpf.Movimentacoes.ViewModels;

namespace Patrimonio.Financas.Wpf.Movimentacoes.Navigation;

/// <summary>
/// Responsável pela navegação da funcionalidade de movimentações.
/// </summary>
public sealed partial class MovimentacaoNavigation(
    IServiceProvider serviceProvider) : NavigationBase
{
    /// <summary>
    /// Abre a tela de alteração da movimentação selecionada.
    /// </summary>
    public async Task AbrirAlteracaoAsync(int movimentacaoId, CancellationToken cancellationToken)
    {
        var viewModel = serviceProvider.GetRequiredService<MovimentacaoAlteracaoViewModel>();

        await viewModel.InicializarAsync(movimentacaoId, cancellationToken);

        ConteudoAtual = viewModel;
    }

    /// <summary>
    /// Abre a tela de criação de movimentação.
    /// </summary>
    public async Task AbrirCriacaoAsync(CancellationToken cancellationToken)
    {
        var viewModel = serviceProvider.GetRequiredService<MovimentacaoCriacaoViewModel>();

        await viewModel.InicializarAsync(cancellationToken);

        ConteudoAtual = viewModel;
    }

    /// <summary>
    /// Abre a tela de listagem de movimentações.
    /// </summary>
    public async Task AbrirListaAsync(CancellationToken cancellationToken)
    {
        var viewModel = serviceProvider.GetRequiredService<MovimentacaoListaViewModel>();

        await viewModel.InicializarAsync(cancellationToken);

        ConteudoAtual = viewModel;
    }
}

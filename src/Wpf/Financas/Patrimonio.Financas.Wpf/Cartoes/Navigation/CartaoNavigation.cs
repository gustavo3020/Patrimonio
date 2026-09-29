using Microsoft.Extensions.DependencyInjection;
using Patrimonio.Financas.Wpf.Cartoes.ViewModels;
using Patrimonio.Financas.Wpf.Common.Navigation;

namespace Patrimonio.Financas.Wpf.Cartoes.Navigation;

/// <summary>
/// Responsável pela navegação da funcionalidade de cartões.
/// </summary>
public sealed partial class CartaoNavigation(
    IServiceProvider serviceProvider) : NavigationBase
{
    /// <summary>
    /// Abre a tela de alteração do cartão selecionado.
    /// </summary>
    public async Task AbrirAlteracaoAsync(int cartaoId, CancellationToken cancellationToken)
    {
        var viewModel = serviceProvider.GetRequiredService<CartaoAlteracaoViewModel>();

        await viewModel.InicializarAsync(voltar: AbrirListaAsync, cartaoId, cancellationToken);

        ConteudoAtual = viewModel;
    }

    /// <summary>
    /// Abre a tela de criação de cartão.
    /// </summary>
    public async Task AbrirCriacaoAsync(CancellationToken cancellationToken)
    {
        var viewModel = serviceProvider.GetRequiredService<CartaoCriacaoViewModel>();

        await viewModel.InicializarAsync(voltar: AbrirListaAsync, cancellationToken);

        ConteudoAtual = viewModel;
    }

    /// <summary>
    /// Abre a tela de listagem de cartões.
    /// </summary>
    public async Task AbrirListaAsync(CancellationToken cancellationToken)
    {
        var viewModel = serviceProvider.GetRequiredService<CartaoListaViewModel>();

        await viewModel.InicializarAsync(cancellationToken);

        ConteudoAtual = viewModel;
    }

    /// <summary>
    /// Abre a tela de listagem de faturas.
    /// </summary>
    public async Task AbrirFaturasAsync(int cartaoId, CancellationToken cancellationToken)
    {
        var navigation = serviceProvider.GetRequiredService<FaturaNavigation>();
        navigation.Inicializar(voltar: AbrirListaAsync, cartaoId);

        await navigation.AbrirListaAsync(cancellationToken);

        ConteudoAtual = navigation;
    }
}

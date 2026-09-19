using Microsoft.Extensions.DependencyInjection;
using Patrimonio.Financas.Wpf.Cartoes.ViewModels;
using Patrimonio.Financas.Wpf.Common.Navigation;

namespace Patrimonio.Financas.Wpf.Cartoes.Navigation;

/// <summary>
/// Responsável pela navegação da funcionalidade de faturas.
/// </summary>
public sealed partial class FaturaNavigation(
    IServiceProvider serviceProvider) : NavigationBase
{
    private Func<CancellationToken, Task> _voltar = _ => Task.CompletedTask;
    private int _cartaoId;

    public void Inicializar(
        Func<CancellationToken, Task> voltar,
        int cartaoId)
    {
        _voltar = voltar;
        _cartaoId = cartaoId;
    }

    /// <summary>
    /// Abre a tela de alteração da fatura selecionada.
    /// </summary>
    public async Task AbrirAlteracaoAsync(int faturaId, CancellationToken cancellationToken)
    {
        var viewModel = serviceProvider.GetRequiredService<FaturaAlteracaoViewModel>();

        await viewModel.InicializarAsync(voltar: AbrirListaAsync, faturaId, cancellationToken);

        ConteudoAtual = viewModel;
    }

    /// <summary>
    /// Abre a tela de criação de fatura.
    /// </summary>
    public void AbrirCriacao()
    {
        var viewModel = serviceProvider.GetRequiredService<FaturaCriacaoViewModel>();

        viewModel.Inicializar(voltar: AbrirListaAsync, _cartaoId);

        ConteudoAtual = viewModel;
    }

    /// <summary>
    /// Abre a tela de listagem de faturas.
    /// </summary>
    public async Task AbrirListaAsync(CancellationToken cancellationToken)
    {
        var viewModel = serviceProvider.GetRequiredService<FaturaListaViewModel>();

        await viewModel.InicializarAsync(_voltar, _cartaoId, cancellationToken);

        ConteudoAtual = viewModel;
    }

    /// <summary>
    /// Abre a tela de pagamento de de fatura.
    /// </summary>
    public async Task AbrirPagamentoAsync(int faturaId, CancellationToken cancellationToken)
    {
        var viewModel = serviceProvider.GetRequiredService<FaturaPagamentoViewModel>();

        await viewModel.InicializarAsync(voltar: AbrirListaAsync, faturaId, cancellationToken);

        ConteudoAtual = viewModel;
    }

    /// <summary>
    /// Abre a tela de listagem de lançamentos.
    /// </summary>
    public async Task AbrirLancamentosAsync(int faturaId, DateOnly dataVencimento, CancellationToken cancellationToken)
    {
        var navigation = serviceProvider.GetRequiredService<LancamentoNavigation>();
        navigation.Inicializar(voltar: AbrirListaAsync, dataVencimento, faturaId);

        await navigation.AbrirListaAsync(cancellationToken);

        ConteudoAtual = navigation;
    }
}

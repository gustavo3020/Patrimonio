using Microsoft.Extensions.DependencyInjection;
using Patrimonio.Financas.Wpf.Cartoes.ViewModels;
using Patrimonio.Financas.Wpf.Common.Navigation;

namespace Patrimonio.Financas.Wpf.Cartoes.Navigation;

/// <summary>
/// Responsável pela navegação da funcionalidade de lançamentos.
/// </summary>
public sealed partial class LancamentoNavigation(
    IServiceProvider serviceProvider) : NavigationBase
{
    private Func<CancellationToken, Task> _voltar = _ => Task.CompletedTask;
    private DateOnly _dataVencimento;
    private int _faturaId;

    public void Inicializar(
        Func<CancellationToken, Task> voltar,
        DateOnly dataVencimento,
        int faturaId)
    {
        _voltar = voltar;
        _dataVencimento = dataVencimento;
        _faturaId = faturaId;
    }

    /// <summary>
    /// Abre a tela de alteração do lançamento selecionado.
    /// </summary>
    public async Task AbrirAlteracaoAsync(int lancamentoId, CancellationToken cancellationToken)
    {
        var viewModel = serviceProvider.GetRequiredService<LancamentoAlteracaoViewModel>();

        await viewModel.InicializarAsync(voltar: AbrirListaAsync, lancamentoId, cancellationToken);

        ConteudoAtual = viewModel;
    }

    /// <summary>
    /// Abre a tela de criação de lançamento.
    /// </summary>
    public async Task AbrirCriacaoAsync(CancellationToken cancellationToken)
    {
        var viewModel = serviceProvider.GetRequiredService<LancamentoCriacaoViewModel>();

        await viewModel.InicializarAsync(voltar: AbrirListaAsync, _faturaId, cancellationToken);

        ConteudoAtual = viewModel;
    }

    /// <summary>
    /// Abre a tela de listagem de lançamentos.
    /// </summary>
    public async Task AbrirListaAsync(CancellationToken cancellationToken)
    {
        var viewModel = serviceProvider.GetRequiredService<LancamentoListaViewModel>();

        await viewModel.InicializarAsync(_voltar, _faturaId, _dataVencimento, cancellationToken);

        ConteudoAtual = viewModel;
    }
}

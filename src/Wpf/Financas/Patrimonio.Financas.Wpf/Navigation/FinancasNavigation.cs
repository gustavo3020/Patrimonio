using CommunityToolkit.Mvvm.Input;
using Patrimonio.Financas.Wpf.Categorias.Navigation;
using Patrimonio.Financas.Wpf.Common.Navigation;
using Patrimonio.Financas.Wpf.Contas.Navigation;
using Patrimonio.Financas.Wpf.Instituicoes.Navigation;
using Patrimonio.Financas.Wpf.Movimentacoes.Navigation;

namespace Patrimonio.Financas.Wpf.Navigation;

/// <summary>
/// Responsável pela navegação do módulo de finanças.
/// </summary>
public sealed partial class FinancasNavigation(
    CategoriaNavigation categoriaNavigation,
    ContaNavigation contaNavigation,
    InstituicaoNavigation instituicaoNavigation,
    MovimentacaoNavigation movimentacaoNavigation) : NavigationBase
{
    /// <summary>
    /// Abre a funcionalidade de categorias.
    /// </summary>
    [RelayCommand]
    private async Task AbrirCategorias(CancellationToken cancellationToken)
    {
        await categoriaNavigation.AbrirListaAsync(cancellationToken);

        ConteudoAtual = categoriaNavigation;
    }

    /// <summary>
    /// Abre a funcionalidade de contas.
    /// </summary>
    [RelayCommand]
    private async Task AbrirContas(CancellationToken cancellationToken)
    {
        await contaNavigation.AbrirListaAsync(cancellationToken);

        ConteudoAtual = contaNavigation;
    }

    /// <summary>
    /// Abre a funcionalidade de instituições.
    /// </summary>
    [RelayCommand]
    private async Task AbrirInstituicoes(CancellationToken cancellationToken)
    {
        await instituicaoNavigation.AbrirListaAsync(cancellationToken);

        ConteudoAtual = instituicaoNavigation;
    }

    /// <summary>
    /// Abre a funcionalidade de movimentações.
    /// </summary>
    [RelayCommand]
    private async Task AbrirMovimentacoes(CancellationToken cancellationToken)
    {
        await movimentacaoNavigation.AbrirListaAsync(cancellationToken);

        ConteudoAtual = movimentacaoNavigation;
    }
}

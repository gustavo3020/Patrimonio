using Microsoft.Extensions.DependencyInjection;
using Patrimonio.Financas.Wpf.Categorias.ViewModels;
using Patrimonio.Financas.Wpf.Common.Navigation;

namespace Patrimonio.Financas.Wpf.Categorias.Navigation;

/// <summary>
/// Responsável pela navegação da funcionalidade de categorias.
/// </summary>
public sealed partial class CategoriaNavigation(
    IServiceProvider serviceProvider) : NavigationBase
{
    /// <summary>
    /// Abre a tela de alteração da categoria selecionada.
    /// </summary>
    public async Task AbrirAlteracaoAsync(int categoriaId, CancellationToken cancellationToken)
    {
        var viewModel = serviceProvider.GetRequiredService<CategoriaAlteracaoViewModel>();

        await viewModel.InicializarAsync(categoriaId, cancellationToken);

        ConteudoAtual = viewModel;
    }

    /// <summary>
    /// Abre a tela de criação de categoria.
    /// </summary>
    public void AbrirCriacao()
    {
        var viewModel = serviceProvider.GetRequiredService<CategoriaCriacaoViewModel>();

        ConteudoAtual = viewModel;
    }

    /// <summary>
    /// Abre a tela de listagem de categorias.
    /// </summary>
    public async Task AbrirListaAsync(CancellationToken cancellationToken)
    {
        var viewModel = serviceProvider.GetRequiredService<CategoriaListaViewModel>();

        await viewModel.InicializarAsync(cancellationToken);

        ConteudoAtual = viewModel;
    }
}

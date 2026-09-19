using Microsoft.Extensions.DependencyInjection;
using Patrimonio.Financas.Wpf.Common.Navigation;
using Patrimonio.Financas.Wpf.Instituicoes.ViewModels;

namespace Patrimonio.Financas.Wpf.Instituicoes.Navigation;

/// <summary>
/// Responsável pela navegação da funcionalidade de instituições.
/// </summary>
public sealed partial class InstituicaoNavigation(
    IServiceProvider serviceProvider) : NavigationBase
{
    /// <summary>
    /// Abre a tela de alteração da instituição selecionada.
    /// </summary>
    public async Task AbrirAlteracaoAsync(int instituicaoId, CancellationToken cancellationToken)
    {
        var viewModel = serviceProvider.GetRequiredService<InstituicaoAlteracaoViewModel>();

        await viewModel.InicializarAsync(voltar: AbrirListaAsync, instituicaoId, cancellationToken);

        ConteudoAtual = viewModel;
    }

    /// <summary>
    /// Abre a tela de criação de instituição.
    /// </summary>
    public void AbrirCriacao()
    {
        var viewModel = serviceProvider.GetRequiredService<InstituicaoCriacaoViewModel>();

        viewModel.Inicializar(voltar: AbrirListaAsync);

        ConteudoAtual = viewModel;
    }

    /// <summary>
    /// Abre a tela de listagem de instituições.
    /// </summary>
    public async Task AbrirListaAsync(CancellationToken cancellationToken)
    {
        var viewModel = serviceProvider.GetRequiredService<InstituicaoListaViewModel>();

        await viewModel.InicializarAsync(cancellationToken);

        ConteudoAtual = viewModel;
    }
}

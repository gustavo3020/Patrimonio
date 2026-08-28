using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Patrimonio.Financas.Contracts.Instituicoes.Dtos;
using Patrimonio.Financas.Contracts.Instituicoes.Services;
using Patrimonio.Financas.Wpf.Common.Exceptions;
using Patrimonio.Financas.Wpf.Common.ViewModels;
using Patrimonio.Financas.Wpf.Instituicoes.Navigation;

namespace Patrimonio.Financas.Wpf.Instituicoes.ViewModels;

/// <summary>
/// ViewModel responsável pela criação de instituições.
/// </summary>
internal sealed partial class InstituicaoCriacaoViewModel(
    IInstituicaoCommandService commandService,
    InstituicaoNavigation navigation,
    ExceptionHandler exceptionHandler) : BaseViewModel
{
    /// <inheritdoc />
    public override string Titulo => "Nova instituição";

    [ObservableProperty] private string nome = string.Empty;

    /// <summary>
    /// Cria uma nova instituição utilizando os dados informados no formulário.
    /// </summary>
    /// <param name="cancellationToken">
    /// Token utilizado para cancelar a operação.
    /// </param>
    [RelayCommand]
    private async Task SalvarAsync(CancellationToken cancellationToken)
    {
        try
        {
            Carregando = true;

            await commandService.CriarAsync(
                new InstituicaoCriacaoDto
                {
                    Nome = Nome
                },
                cancellationToken);

            await navigation.AbrirListaAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            exceptionHandler.Handle(ex);
        }
        finally
        {
            Carregando = false;
        }
    }

    [RelayCommand]
    private async Task CancelarAsync(CancellationToken cancellationToken)
    {
        await navigation.AbrirListaAsync(cancellationToken);
    }
}

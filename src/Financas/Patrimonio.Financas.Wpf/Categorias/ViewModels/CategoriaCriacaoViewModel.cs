using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Patrimonio.Financas.Contracts.Categorias.Dtos;
using Patrimonio.Financas.Contracts.Categorias.Services;
using Patrimonio.Financas.Wpf.Categorias.Navigation;
using Patrimonio.Financas.Wpf.Common.Exceptions;
using Patrimonio.Financas.Wpf.Common.ViewModels;

namespace Patrimonio.Financas.Wpf.Categorias.ViewModels;

/// <summary>
/// ViewModel responsável pela criação de categorias.
/// </summary>
internal sealed partial class CategoriaCriacaoViewModel(
    ICategoriaCommandService commandService,
    CategoriaNavigation navigation,
    ExceptionHandler exceptionHandler) : BaseViewModel
{
    /// <inheritdoc />
    public override string Titulo => "Nova categoria";

    [ObservableProperty] private string nome = string.Empty;

    /// <summary>
    /// Cria uma nova categoria utilizando os dados informados no formulário.
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
                new CategoriaCriacaoDto
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

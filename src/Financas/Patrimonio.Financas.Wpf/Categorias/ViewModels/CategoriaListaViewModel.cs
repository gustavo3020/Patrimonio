using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Patrimonio.Financas.Contracts.Categorias.Dtos;
using Patrimonio.Financas.Contracts.Categorias.Services;
using Patrimonio.Financas.Wpf.Common.Exceptions;
using Patrimonio.Financas.Wpf.Common.ViewModels;

namespace Patrimonio.Financas.Wpf.Categorias.ViewModels;

/// <summary>
/// ViewModel responsável pela listagem de categorias.
/// </summary>
internal partial class CategoriaListaViewModel(
    ICategoriaQueryService queryService,
    ExceptionHandler exceptionHandler) : BaseViewModel
{
    /// <inheritdoc />
    public override string Titulo => "Categorias";

    [ObservableProperty] private IReadOnlyCollection<CategoriaListaDto> categorias = [];

    /// <summary>
    /// Carrega as categorias disponíveis.
    /// </summary>
    /// <param name="cancellationToken">
    /// Token utilizado para cancelar a operação.
    /// </param>
    [RelayCommand]
    private async Task CarregarAsync(CancellationToken cancellationToken)
    {
        try
        {
            Carregando = true;

            Categorias = await queryService.ListarAsync(cancellationToken);
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
}

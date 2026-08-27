using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Patrimonio.Financas.Contracts.Contas.Dtos;
using Patrimonio.Financas.Contracts.Contas.Services;
using Patrimonio.Financas.Wpf.Common.Exceptions;
using Patrimonio.Financas.Wpf.Common.ViewModels;

namespace Patrimonio.Financas.Wpf.Contas.ViewModels;

/// <summary>
/// ViewModel responsável pela listagem de contas.
/// </summary>
internal sealed partial class ContaListaViewModel(
    IContaQueryService queryService,
    ExceptionHandler exceptionHandler) : BaseViewModel
{
    public override string Titulo => "Contas";

    [ObservableProperty] private IReadOnlyCollection<ContaListaDto> contas = [];

    /// <summary>
    /// Carrega as contas disponíveis.
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

            Contas = await queryService.ListarAsync(
                cancellationToken);
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

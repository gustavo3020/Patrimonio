using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Patrimonio.Financas.Contracts.Movimentacoes.Dtos;
using Patrimonio.Financas.Contracts.Movimentacoes.Services;
using Patrimonio.Financas.Wpf.Common.Exceptions;
using Patrimonio.Financas.Wpf.Common.ViewModels;

namespace Patrimonio.Financas.Wpf.Movimentacoes.ViewModels;

/// <summary>
/// ViewModel responsável pela listagem de movimentações financeiras.
/// </summary>
internal partial class MovimentacaoListaViewModel(
    IMovimentacaoQueryService queryService,
    ExceptionHandler exceptionHandler) : BaseViewModel
{
    /// <inheritdoc />
    public override string Titulo => "Movimentações";

    [ObservableProperty] private IReadOnlyCollection<MovimentacaoListaDto> movimentacoes = [];

    /// <summary>
    /// Carrega as movimentações financeiras disponíveis.
    /// </summary>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    [RelayCommand]
    private async Task CarregarAsync(CancellationToken cancellationToken)
    {
        try
        {
            Carregando = true;

            Movimentacoes = await queryService.ListarAsync(
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

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Patrimonio.Financas.Contracts.Instituicoes.Dtos;
using Patrimonio.Financas.Contracts.Instituicoes.Services;
using Patrimonio.Financas.Wpf.Common.Exceptions;
using Patrimonio.Financas.Wpf.Common.ViewModels;

namespace Patrimonio.Financas.Wpf.Instituicoes.ViewModels;

/// <summary>
/// ViewModel responsável pela listagem de instituições.
/// </summary>
internal partial class InstituicaoListaViewModel(
    IInstituicaoQueryService queryService,
    ExceptionHandler exceptionHandler) : BaseViewModel
{
    /// <inheritdoc />
    public override string Titulo => "Instituições";

    /// <summary>
    /// Obtém ou define as instituições exibidas na lista.
    /// </summary>
    [ObservableProperty] private IReadOnlyCollection<InstituicaoListaDto> instituicoes = [];

    /// <summary>
    /// Carrega as instituições disponíveis.
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

            Instituicoes = await queryService.ListarAsync(cancellationToken);
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

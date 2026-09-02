using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Patrimonio.Financas.Contracts.Contas.Dtos;
using Patrimonio.Financas.Contracts.Contas.Services;
using Patrimonio.Financas.Wpf.Common.Dialogs;
using Patrimonio.Financas.Wpf.Common.Exceptions;
using Patrimonio.Financas.Wpf.Common.ViewModels;
using Patrimonio.Financas.Wpf.Contas.Navigation;
using System.Collections.ObjectModel;

namespace Patrimonio.Financas.Wpf.Contas.ViewModels;

/// <summary>
/// ViewModel responsável pela listagem de contas.
/// </summary>
internal sealed partial class ContaListaViewModel(
    IContaCommandService commandService,
    IContaQueryService queryService,
    IDialogService dialogService,
    ContaNavigation navigation,
    ExceptionHandler exceptionHandler) : BaseViewModel
{
    /// <inheritdoc />
    public override string Titulo => "Contas";

    [ObservableProperty] private ObservableCollection<ContaListaDto> contas = [];

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AbrirAlteracaoCommand))]
    [NotifyCanExecuteChangedFor(nameof(ExcluirCommand))]
    private ContaListaDto? selecionado;

    private bool AlterarHabilitado => Selecionado != null && !Carregando;

    /// <summary>
    /// Carrega as contas disponíveis.
    /// </summary>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    public async Task InicializarAsync(CancellationToken cancellationToken)
    {
        try
        {
            Carregando = true;

            var resultado = await queryService.ListarAsync(cancellationToken);

            Contas = new ObservableCollection<ContaListaDto>(resultado);
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
    private async Task AbrirCriacaoAsync(CancellationToken cancellationToken)
        => await navigation.AbrirCriacaoAsync(cancellationToken);

    [RelayCommand(CanExecute = nameof(AlterarHabilitado))]
    private async Task AbrirAlteracaoAsync(CancellationToken cancellationToken)
        => await navigation.AbrirAlteracaoAsync(Selecionado!.Id, cancellationToken);

    [RelayCommand(CanExecute = nameof(AlterarHabilitado))]
    private async Task Excluir(CancellationToken cancellationToken)
    {
        var resultado = dialogService.ShowConfirmation("Tem certeza que deseja excluir essa conta?");

        if (!resultado)
            return;

        try
        {
            Carregando = true;

            await commandService.ExcluirAsync(Selecionado!.Id, cancellationToken);

            Contas.Remove(Selecionado);
            Selecionado = null;
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

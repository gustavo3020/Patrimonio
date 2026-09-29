using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Patrimonio.Financas.Contracts.Cartoes.Dtos;
using Patrimonio.Financas.Wpf.Cartoes.HttpClients;
using Patrimonio.Financas.Wpf.Cartoes.Navigation;
using Patrimonio.Financas.Wpf.Common.Dialogs;
using Patrimonio.Financas.Wpf.Common.Exceptions;
using Patrimonio.Financas.Wpf.Common.ViewModels;
using System.Collections.ObjectModel;

namespace Patrimonio.Financas.Wpf.Cartoes.ViewModels;

/// <summary>
/// ViewModel responsável pela listagem de cartões.
/// </summary>
internal sealed partial class CartaoListaViewModel(
    CartaoHttpClient client,
    CartaoNavigation navigation,
    ExceptionHandler exceptionHandler,
    IDialogService dialogService) : BaseViewModel
{
    /// <inheritdoc />
    public override string Titulo => $"Cartões ({Cartoes.Count})";

    partial void OnCartoesChanged(ObservableCollection<CartaoListaDto> value)
    {
        value.CollectionChanged += (_, __) => OnPropertyChanged(nameof(Titulo));
        OnPropertyChanged(nameof(Titulo));
    }

    [ObservableProperty]
    public partial ObservableCollection<CartaoListaDto> Cartoes { get; set; } = [];

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AbrirFaturasCommand))]
    [NotifyCanExecuteChangedFor(nameof(AbrirAlteracaoCommand))]
    [NotifyCanExecuteChangedFor(nameof(ExcluirCommand))]
    public partial CartaoListaDto? Selecionado { get; set; }

    private bool AlterarHabilitado =>
        Selecionado != null &&
        !Carregando;

    /// <summary>
    /// Carrega os cartões disponíveis.
    /// </summary>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    public async Task InicializarAsync(CancellationToken cancellationToken)
    {
        try
        {
            Carregando = true;

            var resultado = await client.ListarAsync(cancellationToken);

            Cartoes = new ObservableCollection<CartaoListaDto>(resultado);
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

    [RelayCommand(CanExecute = nameof(AlterarHabilitado))]
    private async Task AbrirFaturasAsync(CancellationToken cancellationToken)
        => await navigation.AbrirFaturasAsync(Selecionado!.Id, cancellationToken);

    [RelayCommand]
    private async Task AbrirCriacaoAsync(CancellationToken cancellationToken)
        => await navigation.AbrirCriacaoAsync(cancellationToken);

    [RelayCommand(CanExecute = nameof(AlterarHabilitado))]
    private async Task AbrirAlteracaoAsync(CancellationToken cancellationToken)
        => await navigation.AbrirAlteracaoAsync(Selecionado!.Id, cancellationToken);

    [RelayCommand(CanExecute = nameof(AlterarHabilitado))]
    private async Task Excluir(CancellationToken cancellationToken)
    {
        var resultado = dialogService.ShowConfirmation("Tem certeza que deseja excluir essa cartão?");

        if (!resultado)
            return;

        try
        {
            Carregando = true;

            await client.ExcluirAsync(Selecionado!.Id, cancellationToken);

            Cartoes.Remove(Selecionado);
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

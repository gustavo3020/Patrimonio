using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Patrimonio.Financas.Contracts.Movimentacoes.Dtos;
using Patrimonio.Financas.Wpf.Common.Dialogs;
using Patrimonio.Financas.Wpf.Common.Exceptions;
using Patrimonio.Financas.Wpf.Common.ViewModels;
using Patrimonio.Financas.Wpf.Movimentacoes.HttpClients;
using Patrimonio.Financas.Wpf.Movimentacoes.Navigation;
using System.Collections.ObjectModel;

namespace Patrimonio.Financas.Wpf.Movimentacoes.ViewModels;

/// <summary>
/// ViewModel responsável pela listagem de movimentações financeiras.
/// </summary>
internal sealed partial class MovimentacaoListaViewModel(
    MovimentacaoHttpClient client,
    MovimentacaoNavigation navigation,
    ExceptionHandler exceptionHandler,
    IDialogService dialogService) : BaseViewModel
{
    /// <inheritdoc />
    public override string Titulo => $"Movimentações ({Movimentacoes.Count})";

    partial void OnMovimentacoesChanged(ObservableCollection<MovimentacaoListaDto> value)
    {
        value.CollectionChanged += (_, __) => OnPropertyChanged(nameof(Titulo));
        OnPropertyChanged(nameof(Titulo));
    }

    [ObservableProperty]
    public partial ObservableCollection<MovimentacaoListaDto> Movimentacoes { get; set; } = [];

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AbrirAlteracaoCommand))]
    [NotifyCanExecuteChangedFor(nameof(ExcluirCommand))]
    public partial MovimentacaoListaDto? Selecionado { get; set; }

    private bool AlterarHabilitado => Selecionado != null && !Carregando;

    /// <summary>
    /// Carrega as movimentações financeiras disponíveis.
    /// </summary>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    public async Task InicializarAsync(CancellationToken cancellationToken)
    {
        try
        {
            Carregando = true;

            var resultado = await client.ListarAsync(cancellationToken);

            Movimentacoes = new ObservableCollection<MovimentacaoListaDto>(resultado);
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
        var resultado = dialogService.ShowConfirmation("Tem certeza que deseja excluir essa movimentação?");

        if (!resultado)
            return;

        try
        {
            Carregando = true;

            await client.ExcluirAsync(Selecionado!.Id, cancellationToken);

            Movimentacoes.Remove(Selecionado);
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

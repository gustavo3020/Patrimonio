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
/// ViewModel responsável pela listagem de faturas.
/// </summary>
internal sealed partial class FaturaListaViewModel(
    FaturaHttpClient client,
    FaturaNavigation navigation,
    ExceptionHandler exceptionHandler,
    IDialogService dialogService) : BaseViewModel
{
    /// <inheritdoc />
    public override string Titulo => $"Faturas do cartão {_cartaoNome} ({Faturas.Count})";

    partial void OnFaturasChanged(ObservableCollection<FaturaListaDto> value)
    {
        value.CollectionChanged += (_, __) => OnPropertyChanged(nameof(Titulo));
        OnPropertyChanged(nameof(Titulo));
    }

    [ObservableProperty]
    public partial ObservableCollection<FaturaListaDto> Faturas { get; set; } = [];

    private Func<CancellationToken, Task> _voltar = _ => Task.CompletedTask;
    private int _cartaoId;
    private string _cartaoNome = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AbrirPagamentoCommand))]
    [NotifyCanExecuteChangedFor(nameof(AbrirLancamentosCommand))]
    [NotifyCanExecuteChangedFor(nameof(AbrirAlteracaoCommand))]
    [NotifyCanExecuteChangedFor(nameof(FecharFaturaCommand))]
    [NotifyCanExecuteChangedFor(nameof(ExcluirCommand))]
    public partial FaturaListaDto? Selecionado { get; set; }

    private bool AlterarHabilitado => Selecionado != null && !Carregando;

    /// <summary>
    /// Carrega as faturas disponíveis.
    /// </summary>
    /// <param name="voltar">Função de callback utilizada para retornar à tela anterior.</param>
    /// <param name="cartaoId">Identificador do cartão cujas faturas devem ser recuperadas.</param>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    public async Task InicializarAsync(
        Func<CancellationToken, Task> voltar,
        int cartaoId,
        CancellationToken cancellationToken)
    {
        try
        {
            Carregando = true;
            _voltar = voltar;
            _cartaoId = cartaoId;

            var resultado = await client.ListarAsync(cartaoId, cancellationToken);

            Faturas = new ObservableCollection<FaturaListaDto>(resultado);

            if (Faturas.Count > 0)
                _cartaoNome = Faturas[0].CartaoNome;
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
    private async Task AbrirLancamentosAsync(CancellationToken cancellationToken)
        => await navigation.AbrirLancamentosAsync(Selecionado!.Id, Selecionado.DataVencimento, cancellationToken);

    [RelayCommand]
    private void AbrirCriacao() => navigation.AbrirCriacao();

    [RelayCommand(CanExecute = nameof(AlterarHabilitado))]
    private async Task AbrirAlteracaoAsync(CancellationToken cancellationToken)
        => await navigation.AbrirAlteracaoAsync(Selecionado!.Id, cancellationToken);

    [RelayCommand(CanExecute = nameof(AlterarHabilitado))]
    private async Task AbrirPagamentoAsync(CancellationToken cancellationToken)
        => await navigation.AbrirPagamentoAsync(Selecionado!.Id, cancellationToken);

    [RelayCommand(CanExecute = nameof(AlterarHabilitado))]
    private async Task FecharFaturaAsync(CancellationToken cancellationToken)
    {
        var resultado = dialogService.ShowConfirmation("Tem certeza que deseja fechar essa fatura?");

        if (!resultado)
            return;

        try
        {
            Carregando = true;

            await client.FecharAsync(Selecionado!.Id, cancellationToken);

            var faturas = await client.ListarAsync(_cartaoId, cancellationToken);

            Faturas = new ObservableCollection<FaturaListaDto>(faturas);
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
    private async Task ExcluirAsync(CancellationToken cancellationToken)
    {
        var resultado = dialogService.ShowConfirmation("Tem certeza que deseja excluir essa fatura?");

        if (!resultado)
            return;

        try
        {
            Carregando = true;

            await client.ExcluirAsync(Selecionado!.Id, cancellationToken);

            Faturas.Remove(Selecionado);
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

    [RelayCommand]
    private async Task VoltarAsync(CancellationToken cancellationToken)
    {
        await _voltar(cancellationToken);
    }
}

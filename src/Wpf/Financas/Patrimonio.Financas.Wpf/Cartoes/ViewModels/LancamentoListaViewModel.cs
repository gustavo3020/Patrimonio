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
/// ViewModel responsável pela listagem de lançamentos.
/// </summary>
internal sealed partial class LancamentoListaViewModel(
    LancamentoHttpClient client,
    LancamentoNavigation navigation,
    ExceptionHandler exceptionHandler,
    IDialogService dialogService) : BaseViewModel
{
    /// <inheritdoc />
    public override string Titulo => $"Lançamentos da fatura vencendo em {_dataVencimento} ({Lancamentos.Count})";

    partial void OnLancamentosChanged(ObservableCollection<LancamentoListaDto> value)
    {
        value.CollectionChanged += (_, __) => OnPropertyChanged(nameof(Titulo));
        OnPropertyChanged(nameof(Titulo));
    }

    [ObservableProperty]
    public partial ObservableCollection<LancamentoListaDto> Lancamentos { get; set; } = [];

    private Func<CancellationToken, Task> _voltar = _ => Task.CompletedTask;
    private DateOnly _dataVencimento;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AbrirAlteracaoCommand))]
    [NotifyCanExecuteChangedFor(nameof(ExcluirCommand))]
    public partial LancamentoListaDto? Selecionado { get; set; }

    private bool AlterarHabilitado =>
        Selecionado != null &&
        !Carregando;

    /// <summary>
    /// Carrega os lançamentos disponíveis.
    /// </summary>
    /// <param name="voltar">Função de callback utilizada para retornar à tela anterior.</param>
    /// <param name="faturaId">Identificador da fatura cujos lançamentos devem ser recuperados.</param>
    /// <param name="dataVencimento">Data de vencimento da fatura relacionada.</param>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    public async Task InicializarAsync(
        Func<CancellationToken, Task> voltar,
        int faturaId,
        DateOnly dataVencimento,
        CancellationToken cancellationToken)
    {
        try
        {
            Carregando = true;
            _voltar = voltar;
            _dataVencimento = dataVencimento;

            var resultado = await client.ListarAsync(faturaId, cancellationToken);

            Lancamentos = new ObservableCollection<LancamentoListaDto>(resultado);
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
        var resultado = dialogService.ShowConfirmation("Tem certeza que deseja excluir essa lancamento?");

        if (!resultado)
            return;

        try
        {
            Carregando = true;

            await client.ExcluirAsync(Selecionado!.Id, cancellationToken);

            Lancamentos.Remove(Selecionado);
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

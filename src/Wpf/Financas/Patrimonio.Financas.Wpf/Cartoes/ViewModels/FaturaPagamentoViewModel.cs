using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Patrimonio.Financas.Contracts.Cartoes.Dtos;
using Patrimonio.Financas.Contracts.Common.Dtos;
using Patrimonio.Financas.Wpf.Cartoes.HttpClients;
using Patrimonio.Financas.Wpf.Common.Exceptions;
using Patrimonio.Financas.Wpf.Common.ViewModels;

namespace Patrimonio.Financas.Wpf.Cartoes.ViewModels;

/// <summary>
/// ViewModel responsável pelo pagamento de faturas.
/// </summary>
internal sealed partial class FaturaPagamentoViewModel(
    FaturaHttpClient client,
    ExceptionHandler exceptionHandler) : BaseViewModel
{
    private Func<CancellationToken, Task> _voltar = _ => Task.CompletedTask;

    /// <inheritdoc />
    public override string Titulo => "Pagar fatura";

    [ObservableProperty] public partial int FaturaId { get; set; }
    [ObservableProperty] public partial DateTime DataPagamento { get; set; } = DateTime.Today;
    [ObservableProperty] public partial IReadOnlyCollection<OpcaoDto> Categorias { get; set; } = [];
    [ObservableProperty] public partial IReadOnlyCollection<OpcaoDto> Contas { get; set; } = [];

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SalvarCommand))]
    public partial int? CategoriaId { get; set; }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SalvarCommand))]
    public partial int? ContaId { get; set; }

    /// <summary>
    /// Carrega os dados da fatura a ser paga.
    /// </summary>
    /// <param name="voltar">Função de callback utilizada para retornar à tela anterior.</param>
    /// <param name="faturaId">Identificador da fatura a ser paga.</param>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    public async Task InicializarAsync(
        Func<CancellationToken, Task> voltar,
        int faturaId,
        CancellationToken cancellationToken)
    {
        try
        {
            Carregando = true;
            _voltar = voltar;
            FaturaId = faturaId;

            var opcoes = await client.ObterOpcoesPagamentoAsync(cancellationToken);

            Categorias = opcoes.Categorias;
            Contas = opcoes.Contas;
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

    private bool PodeSalvar() =>
        CategoriaId is not null &&
        ContaId is not null;

    /// <summary>
    /// Realiza o pagamento da fatura atual.
    /// </summary>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    [RelayCommand(CanExecute = nameof(PodeSalvar))]
    private async Task SalvarAsync(CancellationToken cancellationToken)
    {
        try
        {
            Carregando = true;

            await client.PagarAsync(
                FaturaId,
                new FaturaPagamentoDto
                {
                    DataPagamento = DateOnly.FromDateTime(DataPagamento),
                    CategoriaId = CategoriaId!.Value,
                    ContaId = ContaId!.Value
                },
                cancellationToken);

            await _voltar(cancellationToken);
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
        await _voltar(cancellationToken);
    }
}

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Patrimonio.Financas.Contracts.Cartoes.Dtos;
using Patrimonio.Financas.Wpf.Cartoes.HttpClients;
using Patrimonio.Financas.Wpf.Common.Exceptions;
using Patrimonio.Financas.Wpf.Common.ViewModels;

namespace Patrimonio.Financas.Wpf.Cartoes.ViewModels;

/// <summary>
/// ViewModel responsável pela alteração de faturas.
/// </summary>
internal sealed partial class FaturaAlteracaoViewModel(
    FaturaHttpClient client,
    ExceptionHandler exceptionHandler) : BaseViewModel
{
    private Func<CancellationToken, Task> _voltar = _ => Task.CompletedTask;

    /// <inheritdoc />
    public override string Titulo => "Editar fatura";

    [ObservableProperty] public partial int FaturaId { get; set; }
    [ObservableProperty] public partial DateTime DataFechamento { get; set; } = DateTime.Today;
    [ObservableProperty] public partial DateTime DataVencimento { get; set; } = DateTime.Today;

    /// <summary>
    /// Carrega os dados da fatura a ser alterada.
    /// </summary>
    /// <param name="voltar">Função de callback utilizada para retornar à tela anterior.</param>
    /// <param name="faturaId">Identificador da fatura a ser carregada.</param>
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

            var fatura = await client.ObterPorIdAsync(
                faturaId,
                cancellationToken);

            DataFechamento = fatura.DataFechamento.ToDateTime(TimeOnly.MinValue);
            DataVencimento = fatura.DataVencimento.ToDateTime(TimeOnly.MinValue);
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

    /// <summary>
    /// Salva as alterações realizadas na fatura.
    /// </summary>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    [RelayCommand]
    private async Task SalvarAsync(CancellationToken cancellationToken)
    {
        try
        {
            Carregando = true;

            await client.AlterarAsync(
                FaturaId,
                new FaturaAlteracaoDto
                {
                    DataFechamento = DateOnly.FromDateTime(DataFechamento),
                    DataVencimento = DateOnly.FromDateTime(DataVencimento)
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

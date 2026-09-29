using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Patrimonio.Financas.Contracts.Cartoes.Dtos;
using Patrimonio.Financas.Wpf.Cartoes.HttpClients;
using Patrimonio.Financas.Wpf.Common.Exceptions;
using Patrimonio.Financas.Wpf.Common.ViewModels;

namespace Patrimonio.Financas.Wpf.Cartoes.ViewModels;

/// <summary>
/// ViewModel responsável pela criação de faturas.
/// </summary>
internal sealed partial class FaturaCriacaoViewModel(
    FaturaHttpClient client,
    ExceptionHandler exceptionHandler) : BaseViewModel
{
    private Func<CancellationToken, Task> _voltar = _ => Task.CompletedTask;

    /// <inheritdoc />
    public override string Titulo => "Nova fatura";

    [ObservableProperty] public partial DateTime DataFechamento { get; set; } = DateTime.Today;
    [ObservableProperty] public partial DateTime DataVencimento { get; set; } = DateTime.Today;

    private int _cartaoId;

    /// <summary>
    /// Carrega os dados necessários para o funcionamento da tela.
    /// </summary>
    /// <param name="voltar">Função de callback utilizada para retornar à tela anterior.</param>
    /// <param name="cartaoId">Identificador do cartão atual.</param>
    public void Inicializar(Func<CancellationToken, Task> voltar, int cartaoId)
    {
        _voltar = voltar;
        _cartaoId = cartaoId;
    }

    /// <summary>
    /// Cria uma nova fatura utilizando os dados informados no formulário.
    /// </summary>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    [RelayCommand]
    private async Task SalvarAsync(CancellationToken cancellationToken)
    {
        try
        {
            Carregando = true;

            await client.CriarAsync(
                new FaturaCriacaoDto
                {
                    DataFechamento = DateOnly.FromDateTime(DataFechamento),
                    DataVencimento = DateOnly.FromDateTime(DataVencimento),
                    CartaoId = _cartaoId
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

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Patrimonio.Financas.Contracts.Cartoes.Dtos;
using Patrimonio.Financas.Contracts.Common.Dtos;
using Patrimonio.Financas.Wpf.Cartoes.HttpClients;
using Patrimonio.Financas.Wpf.Common.Exceptions;
using Patrimonio.Financas.Wpf.Common.ViewModels;

namespace Patrimonio.Financas.Wpf.Cartoes.ViewModels;

/// <summary>
/// ViewModel responsável pela criação de lançamentos.
/// </summary>
internal sealed partial class LancamentoCriacaoViewModel(
    LancamentoHttpClient client,
    ExceptionHandler exceptionHandler) : BaseViewModel
{
    private Func<CancellationToken, Task> _voltar = _ => Task.CompletedTask;

    /// <inheritdoc />
    public override string Titulo => "Novo lançamento";

    [ObservableProperty] public partial string Descricao { get; set; } = string.Empty;
    [ObservableProperty] public partial decimal Valor { get; set; }
    [ObservableProperty] public partial DateTime DataCompra { get; set; } = DateTime.Today;
    [ObservableProperty] public partial string Estabelecimento { get; set; } = string.Empty;
    [ObservableProperty] public partial string Responsavel { get; set; } = string.Empty;
    [ObservableProperty] public partial int NumeroParcela { get; set; } = 1;
    [ObservableProperty] public partial int TotalParcelas { get; set; } = 1;
    [ObservableProperty] public partial IReadOnlyCollection<OpcaoDto> Categorias { get; set; } = [];

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SalvarCommand))]
    public partial int? CategoriaId { get; set; }

    private int _faturaId;

    /// <summary>
    /// Carrega os dados necessários para o funcionamento da tela.
    /// </summary>
    /// <param name="voltar">Função de callback utilizada para retornar à tela anterior.</param>
    /// <param name="faturaId">Identificador da fatura atual.</param>
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
            _faturaId = faturaId;

            var opcoes = await client.ObterOpcoesCriacaoAsync(cancellationToken);

            Categorias = opcoes.Categorias;
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

    private bool PodeSalvar() => CategoriaId is not null;

    /// <summary>
    /// Cria um novo lançamento utilizando os dados informados no formulário.
    /// </summary>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    [RelayCommand(CanExecute = nameof(PodeSalvar))]
    private async Task SalvarAsync(CancellationToken cancellationToken)
    {
        try
        {
            Carregando = true;

            await client.CriarAsync(
                new LancamentoCriacaoDto
                {
                    Descricao = Descricao,
                    Valor = Valor,
                    DataCompra = DateOnly.FromDateTime(DataCompra),
                    Estabelecimento = Estabelecimento,
                    Responsavel = Responsavel,
                    TotalParcelas = TotalParcelas,
                    FaturaId = _faturaId,
                    CategoriaId = CategoriaId!.Value
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

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Patrimonio.Financas.Contracts.Common.Dtos;
using Patrimonio.Financas.Contracts.Movimentacoes.Dtos;
using Patrimonio.Financas.SharedKernel.Movimentacoes.Enums;
using Patrimonio.Financas.Wpf.Common.Exceptions;
using Patrimonio.Financas.Wpf.Common.ViewModels;
using Patrimonio.Financas.Wpf.Movimentacoes.HttpClients;

namespace Patrimonio.Financas.Wpf.Movimentacoes.ViewModels;

/// <summary>
/// ViewModel responsável pela criação de movimentações financeiras.
/// </summary>
internal sealed partial class MovimentacaoCriacaoViewModel(
    MovimentacaoHttpClient client,
    ExceptionHandler exceptionHandler) : BaseViewModel
{
    private Func<CancellationToken, Task> _voltar = _ => Task.CompletedTask;

    /// <inheritdoc />
    public override string Titulo => "Nova movimentação";

    [ObservableProperty] public partial DateTime Data { get; set; } = DateTime.Today;
    [ObservableProperty] public partial decimal Valor { get; set; }
    [ObservableProperty] public partial string Descricao { get; set; } = string.Empty;
    [ObservableProperty] public partial IReadOnlyCollection<OpcaoDto> Contas { get; set; } = [];
    [ObservableProperty] public partial IReadOnlyCollection<OpcaoDto> Categorias { get; set; } = [];

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SalvarCommand))]
    public partial Natureza? Natureza { get; set; }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SalvarCommand))]
    public partial TipoMovimentacao? Tipo { get; set; }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SalvarCommand))]
    public partial int? ContaId { get; set; }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SalvarCommand))]
    public partial int? CategoriaId { get; set; }

    public static IReadOnlyCollection<Natureza> Naturezas { get; } = Enum.GetValues<Natureza>();
    public static IReadOnlyCollection<TipoMovimentacao> Tipos { get; } = Enum.GetValues<TipoMovimentacao>();

    /// <summary>
    /// Carrega os dados necessários para o preenchimento do formulário.
    /// </summary>
    /// <param name="voltar">Função de callback utilizada para retornar à tela anterior.</param>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    public async Task InicializarAsync(
        Func<CancellationToken, Task> voltar,
        CancellationToken cancellationToken)
    {
        try
        {
            Carregando = true;
            _voltar = voltar;

            var opcoes = await client.ObterOpcoesCriacaoAsync(cancellationToken);

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
        ContaId is not null &&
        CategoriaId is not null &&
        Natureza is not null &&
        Tipo is not null;

    /// <summary>
    /// Cria uma nova movimentação com os dados informados no formulário.
    /// </summary>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    [RelayCommand(CanExecute = nameof(PodeSalvar))]
    private async Task SalvarAsync(CancellationToken cancellationToken)
    {
        try
        {
            Carregando = true;

            await client.CriarAsync(
                new MovimentacaoCriacaoDto
                {
                    Data = DateOnly.FromDateTime(Data),
                    Valor = Valor,
                    Descricao = Descricao,
                    Natureza = Natureza!.Value,
                    Tipo = Tipo!.Value,
                    ContaId = ContaId!.Value,
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

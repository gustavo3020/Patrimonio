using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Patrimonio.Financas.Contracts.Common.Dtos;
using Patrimonio.Financas.Contracts.Movimentacoes.Dtos;
using Patrimonio.Financas.Contracts.Movimentacoes.Services;
using Patrimonio.Financas.SharedKernel.Movimentacoes.Enums;
using Patrimonio.Financas.Wpf.Common.Exceptions;
using Patrimonio.Financas.Wpf.Common.ViewModels;
using Patrimonio.Financas.Wpf.Movimentacoes.Navigation;

namespace Patrimonio.Financas.Wpf.Movimentacoes.ViewModels;

/// <summary>
/// ViewModel responsável pela alteração de movimentações financeiras.
/// </summary>
internal sealed partial class MovimentacaoAlteracaoViewModel(
    IMovimentacaoCommandService commandService,
    IMovimentacaoOpcoesCriacaoService opcoesCriacaoService,
    IMovimentacaoQueryService queryService,
    MovimentacaoNavigation navigation,
    ExceptionHandler exceptionHandler) : BaseViewModel
{
    /// <inheritdoc />
    public override string Titulo => "Editar movimentação";

    [ObservableProperty] private int movimentacaoId;
    [ObservableProperty] private DateTime data;
    [ObservableProperty] private decimal valor;
    [ObservableProperty] private string descricao = string.Empty;
    [ObservableProperty] private IReadOnlyCollection<OpcaoDto> contas = [];
    [ObservableProperty] private IReadOnlyCollection<OpcaoDto> categorias = [];

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SalvarCommand))]
    private Natureza? natureza;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SalvarCommand))]
    private TipoMovimentacao? tipo;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SalvarCommand))]
    private int? contaId;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SalvarCommand))]
    private int? categoriaId;

    public static IReadOnlyCollection<Natureza> Naturezas { get; } = Enum.GetValues<Natureza>();
    public static IReadOnlyCollection<TipoMovimentacao> Tipos { get; } = Enum.GetValues<TipoMovimentacao>();

    /// <summary>
    /// Carrega a movimentação e os dados necessários para o preenchimento do formulário.
    /// </summary>
    /// <param name="movimentacaoId">Identificador da movimentação a ser carregada.</param>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    [RelayCommand]
    public async Task InicializarAsync(
        int movimentacaoId,
        CancellationToken cancellationToken)
    {
        try
        {
            Carregando = true;
            MovimentacaoId = movimentacaoId;

            var movimentacaoTask = queryService.ObterPorIdAsync(
                movimentacaoId,
                cancellationToken);

            var opcoesTask = opcoesCriacaoService.ObterOpcoesCriacaoAsync(
                cancellationToken);

            await Task.WhenAll(
                movimentacaoTask,
                opcoesTask);

            var movimentacao = await movimentacaoTask;
            var opcoes = await opcoesTask;

            Data = movimentacao.Data.ToDateTime(TimeOnly.MinValue);
            Valor = movimentacao.Valor;
            Descricao = movimentacao.Descricao ?? string.Empty;
            Natureza = movimentacao.Natureza;
            Tipo = movimentacao.Tipo;
            ContaId = movimentacao.ContaId;
            CategoriaId = movimentacao.CategoriaId;

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
    /// Altera a movimentação com os dados informados no formulário.
    /// </summary>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    [RelayCommand(CanExecute = nameof(PodeSalvar))]
    private async Task SalvarAsync(CancellationToken cancellationToken)
    {
        try
        {
            Carregando = true;

            await commandService.AlterarAsync(
                MovimentacaoId,
                new MovimentacaoAlteracaoDto
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
        await navigation.AbrirListaAsync(cancellationToken);
    }
}

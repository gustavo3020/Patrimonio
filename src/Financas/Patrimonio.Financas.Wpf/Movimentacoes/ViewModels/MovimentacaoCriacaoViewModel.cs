using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Patrimonio.Financas.Contracts.Categorias.Dtos;
using Patrimonio.Financas.Contracts.Categorias.Services;
using Patrimonio.Financas.Contracts.Contas.Dtos;
using Patrimonio.Financas.Contracts.Contas.Services;
using Patrimonio.Financas.Contracts.Movimentacoes.Dtos;
using Patrimonio.Financas.Contracts.Movimentacoes.Services;
using Patrimonio.Financas.SharedKernel.Movimentacoes.Enums;
using Patrimonio.Financas.Wpf.Common.Exceptions;
using Patrimonio.Financas.Wpf.Common.ViewModels;

namespace Patrimonio.Financas.Wpf.Movimentacoes.ViewModels;

/// <summary>
/// ViewModel responsável pela criação de movimentações financeiras.
/// </summary>
internal partial class MovimentacaoCriacaoViewModel(
    IMovimentacaoCommandService commandService,
    IContaQueryService contaQueryService,
    ICategoriaQueryService categoriaQueryService,
    ExceptionHandler exceptionHandler) : BaseViewModel
{
    /// <inheritdoc />
    public override string Titulo => "Nova movimentação";

    [ObservableProperty] private DateOnly data = DateOnly.FromDateTime(DateTime.Today);
    [ObservableProperty] private decimal valor;
    [ObservableProperty] private string descricao = string.Empty;
    [ObservableProperty] private IReadOnlyCollection<ContaListaDto> contas = [];
    [ObservableProperty] private IReadOnlyCollection<CategoriaListaDto> categorias = [];

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
    /// Carrega os dados necessários para o preenchimento do formulário.
    /// </summary>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    [RelayCommand]
    private async Task CarregarAsync(CancellationToken cancellationToken)
    {
        try
        {
            Carregando = true;

            var contasTask = contaQueryService.ListarAsync(
                cancellationToken);

            var categoriasTask = categoriaQueryService.ListarAsync(
                cancellationToken);

            await Task.WhenAll(
                contasTask,
                categoriasTask);

            Contas = await contasTask;
            Categorias = await categoriasTask;
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

            await commandService.CriarAsync(
                new MovimentacaoCriacaoDto
                {
                    Data = Data,
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
}

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Patrimonio.Financas.Contracts.Cartoes.Dtos;
using Patrimonio.Financas.Contracts.Common.Dtos;
using Patrimonio.Financas.SharedKernel.Movimentacoes.Enums;
using Patrimonio.Financas.Wpf.Cartoes.HttpClients;
using Patrimonio.Financas.Wpf.Common.Exceptions;
using Patrimonio.Financas.Wpf.Common.ViewModels;

namespace Patrimonio.Financas.Wpf.Cartoes.ViewModels;

/// <summary>
/// ViewModel responsável pela alteração de lançamentos.
/// </summary>
internal sealed partial class LancamentoAlteracaoViewModel(
    LancamentoHttpClient client,
    ExceptionHandler exceptionHandler) : BaseViewModel
{
    private Func<CancellationToken, Task> _voltar = _ => Task.CompletedTask;

    /// <inheritdoc />
    public override string Titulo => "Editar lançamento";

    [ObservableProperty] public partial int LancamentoId { get; set; }
    [ObservableProperty] public partial string Descricao { get; set; } = string.Empty;
    [ObservableProperty] public partial decimal Valor { get; set; }
    [ObservableProperty] public partial DateTime DataCompra { get; set; } = DateTime.Today;
    [ObservableProperty] public partial string Estabelecimento { get; set; } = string.Empty;
    [ObservableProperty] public partial string Responsavel { get; set; } = string.Empty;
    [ObservableProperty] public partial int NumeroParcela { get; set; }
    [ObservableProperty] public partial int TotalParcelas { get; set; }
    [ObservableProperty] public partial Natureza Natureza { get; set; }
    [ObservableProperty] public partial IReadOnlyCollection<OpcaoDto> Categorias { get; set; } = [];

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SalvarCommand))]
    public partial int? CategoriaId { get; set; }

    public static IReadOnlyCollection<Natureza> Naturezas { get; } = Enum.GetValues<Natureza>();

    /// <summary>
    /// Carrega os dados do lançamento a ser alterado.
    /// </summary>
    /// <param name="voltar">Função de callback utilizada para retornar à tela anterior.</param>
    /// <param name="lancamentoId">Identificador do lançamento a ser carregado.</param>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    public async Task InicializarAsync(
        Func<CancellationToken, Task> voltar,
        int lancamentoId,
        CancellationToken cancellationToken)
    {
        try
        {
            Carregando = true;
            _voltar = voltar;
            LancamentoId = lancamentoId;

            var lancamentoTask = client.ObterPorIdAsync(
                lancamentoId,
                cancellationToken);

            var opcoesTask = client.ObterOpcoesCriacaoAsync(
                cancellationToken);

            await Task.WhenAll(
                lancamentoTask,
                opcoesTask);

            var lancamento = await lancamentoTask;
            var opcoes = await opcoesTask;

            Descricao = lancamento.Descricao;
            Valor = lancamento.Valor;
            DataCompra = lancamento.DataCompra.ToDateTime(TimeOnly.MinValue);
            Estabelecimento = lancamento.Estabelecimento;
            Responsavel = lancamento.Responsavel;
            NumeroParcela = lancamento.NumeroParcela;
            TotalParcelas = lancamento.TotalParcelas;
            Natureza = lancamento.Natureza;
            CategoriaId = lancamento.CategoriaId;

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
    public bool PodeAlterarParcelas { get; } = false;

    /// <summary>
    /// Salva as alterações realizadas no lançamento.
    /// </summary>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    [RelayCommand(CanExecute = nameof(PodeSalvar))]
    private async Task SalvarAsync(CancellationToken cancellationToken)
    {
        try
        {
            Carregando = true;

            await client.AlterarAsync(
                LancamentoId,
                new LancamentoAlteracaoDto
                {
                    Descricao = Descricao,
                    Valor = Valor,
                    DataCompra = DateOnly.FromDateTime(DataCompra),
                    Estabelecimento = Estabelecimento,
                    Responsavel = Responsavel,
                    Natureza = Natureza,
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

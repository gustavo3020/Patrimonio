using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Patrimonio.Financas.Contracts.Common.Dtos;
using Patrimonio.Financas.Contracts.Contas.Dtos;
using Patrimonio.Financas.Contracts.Contas.Services;
using Patrimonio.Financas.Wpf.Common.Exceptions;
using Patrimonio.Financas.Wpf.Common.ViewModels;
using Patrimonio.Financas.Wpf.Contas.Navigation;

namespace Patrimonio.Financas.Wpf.Contas.ViewModels;

/// <summary>
/// ViewModel responsável pela alteração de contas.
/// </summary>
internal sealed partial class ContaAlteracaoViewModel(
    IContaCommandService commandService,
    IContaOpcoesCriacaoService contaOpcoesCriacaoService,
    IContaQueryService queryService,
    ContaNavigation navigation,
    ExceptionHandler exceptionHandler) : BaseViewModel
{
    /// <inheritdoc />
    public override string Titulo => "Editar conta";

    [ObservableProperty] private int contaId;
    [ObservableProperty] private string nome = string.Empty;
    [ObservableProperty] private IReadOnlyCollection<OpcaoDto> instituicoes = [];

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SalvarCommand))]
    private int? instituicaoId;

    /// <summary>
    /// Carrega os dados da conta a ser alterada.
    /// </summary>
    /// <param name="contaId">Identificador da conta a ser carregada.</param>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    [RelayCommand]
    public async Task InicializarAsync(
        int contaId,
        CancellationToken cancellationToken)
    {
        try
        {
            Carregando = true;
            ContaId = contaId;

            var contaTask = queryService.ObterPorIdAsync(
                contaId,
                cancellationToken);

            var opcoesTask = contaOpcoesCriacaoService.ObterOpcoesCriacaoAsync(
                cancellationToken);

            await Task.WhenAll(
                contaTask,
                opcoesTask);

            var conta = await contaTask;
            var opcoes = await opcoesTask;

            Nome = conta.Nome;
            InstituicaoId = conta.InstituicaoId;

            Instituicoes = opcoes.Instituicoes;
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

    private bool PodeSalvar() => InstituicaoId is not null;

    /// <summary>
    /// Salva as alterações realizadas na conta.
    /// </summary>
    /// <param name="cancellationToken">
    /// Token utilizado para cancelar a operação.
    /// </param>
    [RelayCommand(CanExecute = nameof(PodeSalvar))]
    private async Task SalvarAsync(CancellationToken cancellationToken)
    {
        try
        {
            Carregando = true;

            await commandService.AlterarAsync(
                ContaId,
                new ContaAlteracaoDto
                {
                    Nome = Nome,
                    InstituicaoId = InstituicaoId!.Value
                },
                cancellationToken);

            await navigation.AbrirListaAsync(cancellationToken);
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

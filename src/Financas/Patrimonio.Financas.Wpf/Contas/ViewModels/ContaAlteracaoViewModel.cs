using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Patrimonio.Financas.Contracts.Contas.Dtos;
using Patrimonio.Financas.Contracts.Contas.Services;
using Patrimonio.Financas.Contracts.Instituicoes.Dtos;
using Patrimonio.Financas.Contracts.Instituicoes.Services;
using Patrimonio.Financas.Wpf.Common.Exceptions;
using Patrimonio.Financas.Wpf.Common.ViewModels;

namespace Patrimonio.Financas.Wpf.Contas.ViewModels;

/// <summary>
/// ViewModel responsável pela alteração de contas.
/// </summary>
internal partial class ContaAlteracaoViewModel(
    IContaCommandService commandService,
    IContaQueryService queryService,
    IInstituicaoQueryService instituicaoQueryService,
    ExceptionHandler exceptionHandler) : BaseViewModel
{
    public override string Titulo => "Editar conta";

    [ObservableProperty] private int contaId;
    [ObservableProperty] private string nome = string.Empty;
    [ObservableProperty] private IReadOnlyCollection<InstituicaoListaDto> instituicoes = [];

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SalvarCommand))]
    private int? instituicaoId;

    /// <summary>
    /// Carrega os dados da conta a ser alterada.
    /// </summary>
    /// <param name="contaId">
    /// Identificador da conta a ser carregada.
    /// </param>
    /// <param name="cancellationToken">
    /// Token utilizado para cancelar a operação.
    /// </param>
    [RelayCommand]
    private async Task CarregarAsync(
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

            var instituicoesTask = instituicaoQueryService.ListarAsync(
                cancellationToken);

            await Task.WhenAll(
                contaTask,
                instituicoesTask);

            var conta = await contaTask;

            Nome = conta.Nome;
            InstituicaoId = conta.InstituicaoId;
            Instituicoes = await instituicoesTask;
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

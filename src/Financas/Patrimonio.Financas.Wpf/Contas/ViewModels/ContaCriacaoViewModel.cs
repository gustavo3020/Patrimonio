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
/// ViewModel responsável pela criação de contas.
/// </summary>
internal partial class ContaCriacaoViewModel(
    IContaCommandService commandService,
    IInstituicaoQueryService instituicaoQueryService,
    ExceptionHandler exceptionHandler) : BaseViewModel
{
    public override string Titulo => "Nova conta";

    [ObservableProperty] private string nome = string.Empty;
    [ObservableProperty] private IReadOnlyCollection<InstituicaoListaDto> instituicoes = [];

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SalvarCommand))]
    private int? instituicaoId;

    /// <summary>
    /// Carrega as instituições disponíveis para seleção no formulário.
    /// </summary>
    /// <param name="cancellationToken">
    /// Token utilizado para cancelar a operação.
    /// </param>
    [RelayCommand]
    private async Task CarregarAsync(CancellationToken cancellationToken)
    {
        try
        {
            Carregando = true;

            Instituicoes = await instituicaoQueryService.ListarAsync(
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

    private bool PodeSalvar() => InstituicaoId is not null;

    /// <summary>
    /// Cria uma nova conta utilizando os dados informados no formulário.
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

            await commandService.CriarAsync(
                new ContaCriacaoDto
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

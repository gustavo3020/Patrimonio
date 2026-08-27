using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Patrimonio.Financas.Contracts.Instituicoes.Dtos;
using Patrimonio.Financas.Contracts.Instituicoes.Services;
using Patrimonio.Financas.Wpf.Common.Exceptions;
using Patrimonio.Financas.Wpf.Common.ViewModels;

namespace Patrimonio.Financas.Wpf.Instituicoes.ViewModels;

/// <summary>
/// ViewModel responsável pela alteração de instituições.
/// </summary>
internal sealed partial class InstituicaoAlteracaoViewModel(
    IInstituicaoCommandService commandService,
    IInstituicaoQueryService queryService,
    ExceptionHandler exceptionHandler) : BaseViewModel
{
    /// <inheritdoc />
    public override string Titulo => "Editar instituição";

    [ObservableProperty] private int instituicaoId;
    [ObservableProperty] private string nome = string.Empty;

    /// <summary>
    /// Carrega os dados da instituição a ser alterada.
    /// </summary>
    /// <param name="instituicaoId">
    /// Identificador da instituição a ser carregada.
    /// </param>
    /// <param name="cancellationToken">
    /// Token utilizado para cancelar a operação.
    /// </param>
    [RelayCommand]
    private async Task CarregarAsync(
        int instituicaoId,
        CancellationToken cancellationToken)
    {
        try
        {
            Carregando = true;
            InstituicaoId = instituicaoId;

            var instituicao = await queryService.ObterPorIdAsync(
                instituicaoId,
                cancellationToken);

            Nome = instituicao.Nome;
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
    /// Salva as alterações realizadas na instituição.
    /// </summary>
    /// <param name="cancellationToken">
    /// Token utilizado para cancelar a operação.
    /// </param>
    [RelayCommand]
    private async Task SalvarAsync(
        CancellationToken cancellationToken)
    {
        try
        {
            Carregando = true;

            await commandService.AlterarAsync(
                InstituicaoId,
                new InstituicaoAlteracaoDto
                {
                    Nome = Nome
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

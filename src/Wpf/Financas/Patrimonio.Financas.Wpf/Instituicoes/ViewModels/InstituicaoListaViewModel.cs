using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Patrimonio.Financas.Contracts.Instituicoes.Dtos;
using Patrimonio.Financas.Contracts.Instituicoes.Services;
using Patrimonio.Financas.Wpf.Common.Dialogs;
using Patrimonio.Financas.Wpf.Common.Exceptions;
using Patrimonio.Financas.Wpf.Common.ViewModels;
using Patrimonio.Financas.Wpf.Instituicoes.Navigation;
using System.Collections.ObjectModel;

namespace Patrimonio.Financas.Wpf.Instituicoes.ViewModels;

/// <summary>
/// ViewModel responsável pela listagem de instituições.
/// </summary>
internal sealed partial class InstituicaoListaViewModel(
    IInstituicaoCommandService commandService,
    IInstituicaoQueryService queryService,
    IDialogService dialogService,
    InstituicaoNavigation navigation,
    ExceptionHandler exceptionHandler) : BaseViewModel
{
    /// <inheritdoc />
    public override string Titulo => "Instituições";

    [ObservableProperty] private ObservableCollection<InstituicaoListaDto> instituicoes = [];

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AbrirAlteracaoCommand))]
    [NotifyCanExecuteChangedFor(nameof(ExcluirCommand))]
    private InstituicaoListaDto? selecionado;

    private bool AlterarHabilitado => Selecionado != null && !Carregando;

    /// <summary>
    /// Carrega as instituições disponíveis.
    /// </summary>
    /// <param name="cancellationToken">
    /// Token utilizado para cancelar a operação.
    /// </param>
    public async Task InicializarAsync(CancellationToken cancellationToken)
    {
        try
        {
            Carregando = true;

            var resultado = await queryService.ListarAsync(cancellationToken);

            Instituicoes = new ObservableCollection<InstituicaoListaDto>(resultado);
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
    private void AbrirCriacao()
        => navigation.AbrirCriacao();

    [RelayCommand(CanExecute = nameof(AlterarHabilitado))]
    private async Task AbrirAlteracaoAsync(CancellationToken cancellationToken)
        => await navigation.AbrirAlteracaoAsync(Selecionado!.Id, cancellationToken);

    [RelayCommand(CanExecute = nameof(AlterarHabilitado))]
    private async Task Excluir(CancellationToken cancellationToken)
    {
        var resultado = dialogService.ShowConfirmation("Tem certeza que deseja excluir essa instituição?");

        if (!resultado)
            return;

        try
        {
            Carregando = true;

            await commandService.ExcluirAsync(Selecionado!.Id, cancellationToken);

            Instituicoes.Remove(Selecionado);
            Selecionado = null;
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

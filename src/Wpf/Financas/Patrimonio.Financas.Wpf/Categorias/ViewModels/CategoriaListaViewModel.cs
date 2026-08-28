using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Patrimonio.Financas.Contracts.Categorias.Dtos;
using Patrimonio.Financas.Contracts.Categorias.Services;
using Patrimonio.Financas.Wpf.Categorias.Navigation;
using Patrimonio.Financas.Wpf.Common.Dialogs;
using Patrimonio.Financas.Wpf.Common.Exceptions;
using Patrimonio.Financas.Wpf.Common.ViewModels;
using System.Collections.ObjectModel;

namespace Patrimonio.Financas.Wpf.Categorias.ViewModels;

/// <summary>
/// ViewModel responsável pela listagem de categorias.
/// </summary>
internal sealed partial class CategoriaListaViewModel(
    ICategoriaCommandService commandService,
    ICategoriaQueryService queryService,
    IDialogService dialogService,
    CategoriaNavigation navigation,
    ExceptionHandler exceptionHandler) : BaseViewModel
{
    /// <inheritdoc />
    public override string Titulo => "Categorias";

    [ObservableProperty] private ObservableCollection<CategoriaListaDto> categorias = [];

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AbrirAlteracaoCommand))]
    [NotifyCanExecuteChangedFor(nameof(ExcluirCommand))]
    private CategoriaListaDto? selecionado;

    private bool AlterarHabilitado => Selecionado != null && !Carregando;

    /// <summary>
    /// Carrega as categorias disponíveis.
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

            Categorias = new ObservableCollection<CategoriaListaDto>(resultado);
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
        var resultado = dialogService.ShowConfirmation("Tem certeza que deseja excluir essa categoria?");

        if (!resultado)
            return;

        try
        {
            Carregando = true;

            await commandService.ExcluirAsync(Selecionado!.Id, cancellationToken);

            Categorias.Remove(Selecionado);
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

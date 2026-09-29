using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Patrimonio.Financas.Contracts.Categorias.Dtos;
using Patrimonio.Financas.Wpf.Categorias.HttpClients;
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
    CategoriaHttpClient client,
    CategoriaNavigation navigation,
    ExceptionHandler exceptionHandler,
    IDialogService dialogService) : BaseViewModel
{
    /// <inheritdoc />
    public override string Titulo => $"Categorias ({Categorias.Count})";

    partial void OnCategoriasChanged(ObservableCollection<CategoriaListaDto> value)
    {
        value.CollectionChanged += (_, __) => OnPropertyChanged(nameof(Titulo));
        OnPropertyChanged(nameof(Titulo));
    }

    [ObservableProperty]
    public partial ObservableCollection<CategoriaListaDto> Categorias { get; set; } = [];

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AbrirAlteracaoCommand))]
    [NotifyCanExecuteChangedFor(nameof(ExcluirCommand))]
    public partial CategoriaListaDto? Selecionado { get; set; }

    private bool AlterarHabilitado => Selecionado != null && !Carregando;

    /// <summary>
    /// Carrega as categorias disponíveis.
    /// </summary>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    public async Task InicializarAsync(CancellationToken cancellationToken)
    {
        try
        {
            Carregando = true;

            var resultado = await client.ListarAsync(cancellationToken);

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

            await client.ExcluirAsync(Selecionado!.Id, cancellationToken);

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

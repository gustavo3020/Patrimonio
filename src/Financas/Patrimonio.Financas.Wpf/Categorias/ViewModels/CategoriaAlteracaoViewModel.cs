using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Patrimonio.Financas.Contracts.Categorias.Dtos;
using Patrimonio.Financas.Contracts.Categorias.Services;
using Patrimonio.Financas.Wpf.Common.Exceptions;
using Patrimonio.Financas.Wpf.Common.ViewModels;

namespace Patrimonio.Financas.Wpf.Categorias.ViewModels;

/// <summary>
/// ViewModel responsável pela alteração de categorias.
/// </summary>
internal partial class CategoriaAlteracaoViewModel(
    ICategoriaCommandService commandService,
    ICategoriaQueryService queryService,
    ExceptionHandler exceptionHandler) : BaseViewModel
{
    /// <inheritdoc />
    public override string Titulo => "Editar categoria";

    [ObservableProperty] private int categoriaId;
    [ObservableProperty] private string nome = string.Empty;

    /// <summary>
    /// Carrega os dados da categoria a ser alterada.
    /// </summary>
    /// <param name="categoriaId">
    /// Identificador da categoria a ser carregada.
    /// </param>
    /// <param name="cancellationToken">
    /// Token utilizado para cancelar a operação.
    /// </param>
    [RelayCommand]
    private async Task CarregarAsync(
        int categoriaId,
        CancellationToken cancellationToken)
    {
        try
        {
            Carregando = true;
            CategoriaId = categoriaId;

            var categoria = await queryService.ObterPorIdAsync(
                categoriaId,
                cancellationToken);

            Nome = categoria.Nome;
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
    /// Salva as alterações realizadas na categoria.
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
                CategoriaId,
                new CategoriaAlteracaoDto
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

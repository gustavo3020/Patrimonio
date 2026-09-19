using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Patrimonio.Financas.Contracts.Categorias.Dtos;
using Patrimonio.Financas.Wpf.Categorias.HttpClients;
using Patrimonio.Financas.Wpf.Common.Exceptions;
using Patrimonio.Financas.Wpf.Common.ViewModels;

namespace Patrimonio.Financas.Wpf.Categorias.ViewModels;

/// <summary>
/// ViewModel responsável pela alteração de categorias.
/// </summary>
internal sealed partial class CategoriaAlteracaoViewModel(
    CategoriaHttpClient client,
    ExceptionHandler exceptionHandler) : BaseViewModel
{
    private Func<CancellationToken, Task> _voltar = _ => Task.CompletedTask;

    /// <inheritdoc />
    public override string Titulo => "Editar categoria";

    [ObservableProperty] public partial int CategoriaId { get; set; }
    [ObservableProperty] public partial string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Carrega os dados da categoria a ser alterada.
    /// </summary>
    /// <param name="voltar">Função de callback utilizada para retornar à tela anterior.</param>
    /// <param name="categoriaId">Identificador da categoria a ser carregada.</param>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    public async Task InicializarAsync(
        Func<CancellationToken, Task> voltar,
        int categoriaId,
        CancellationToken cancellationToken)
    {
        try
        {
            Carregando = true;
            _voltar = voltar;
            CategoriaId = categoriaId;

            var categoria = await client.ObterPorIdAsync(
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
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    [RelayCommand]
    private async Task SalvarAsync(
        CancellationToken cancellationToken)
    {
        try
        {
            Carregando = true;

            await client.AlterarAsync(
                CategoriaId,
                new CategoriaAlteracaoDto
                {
                    Nome = Nome
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

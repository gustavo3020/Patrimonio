using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Patrimonio.Financas.Contracts.Categorias.Dtos;
using Patrimonio.Financas.Wpf.Categorias.HttpClients;
using Patrimonio.Financas.Wpf.Common.Exceptions;
using Patrimonio.Financas.Wpf.Common.ViewModels;

namespace Patrimonio.Financas.Wpf.Categorias.ViewModels;

/// <summary>
/// ViewModel responsável pela criação de categorias.
/// </summary>
internal sealed partial class CategoriaCriacaoViewModel(
    CategoriaHttpClient client,
    ExceptionHandler exceptionHandler) : BaseViewModel
{
    private Func<CancellationToken, Task> _voltar = _ => Task.CompletedTask;

    /// <inheritdoc />
    public override string Titulo => "Nova categoria";

    [ObservableProperty] public partial string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Inicializa a tela de criação de categoria,
    /// configurando a ação de retorno para a tela anterior.
    /// </summary>
    /// <param name="voltar">Função de callback utilizada para retornar à tela anterior.</param>
    public void Inicializar(Func<CancellationToken, Task> voltar)
    {
        _voltar = voltar;
    }

    /// <summary>
    /// Cria uma nova categoria utilizando os dados informados no formulário.
    /// </summary>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    [RelayCommand]
    private async Task SalvarAsync(CancellationToken cancellationToken)
    {
        try
        {
            Carregando = true;

            await client.CriarAsync(
                new CategoriaCriacaoDto
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

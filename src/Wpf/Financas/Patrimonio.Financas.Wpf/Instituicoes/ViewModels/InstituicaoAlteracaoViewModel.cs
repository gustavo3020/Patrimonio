using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Patrimonio.Financas.Contracts.Instituicoes.Dtos;
using Patrimonio.Financas.Wpf.Common.Exceptions;
using Patrimonio.Financas.Wpf.Common.ViewModels;
using Patrimonio.Financas.Wpf.Instituicoes.HttpClients;

namespace Patrimonio.Financas.Wpf.Instituicoes.ViewModels;

/// <summary>
/// ViewModel responsável pela alteração de instituições.
/// </summary>
internal sealed partial class InstituicaoAlteracaoViewModel(
    InstituicaoHttpClient client,
    ExceptionHandler exceptionHandler) : BaseViewModel
{
    private Func<CancellationToken, Task> _voltar = _ => Task.CompletedTask;

    /// <inheritdoc />
    public override string Titulo => "Editar instituição";

    [ObservableProperty] public partial int InstituicaoId { get; set; }
    [ObservableProperty] public partial string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Carrega os dados da instituição a ser alterada.
    /// </summary>
    /// <param name="voltar">Função de callback utilizada para retornar à tela anterior.</param>
    /// <param name="instituicaoId">Identificador da instituição a ser carregada.</param>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    public async Task InicializarAsync(
        Func<CancellationToken, Task> voltar,
        int instituicaoId,
        CancellationToken cancellationToken)
    {
        try
        {
            Carregando = true;
            _voltar = voltar;
            InstituicaoId = instituicaoId;

            var instituicao = await client.ObterPorIdAsync(
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
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    [RelayCommand]
    private async Task SalvarAsync(
        CancellationToken cancellationToken)
    {
        try
        {
            Carregando = true;

            await client.AlterarAsync(
                InstituicaoId,
                new InstituicaoAlteracaoDto
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

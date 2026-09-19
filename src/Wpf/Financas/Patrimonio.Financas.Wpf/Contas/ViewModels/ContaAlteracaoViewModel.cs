using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Patrimonio.Financas.Contracts.Common.Dtos;
using Patrimonio.Financas.Contracts.Contas.Dtos;
using Patrimonio.Financas.Wpf.Common.Exceptions;
using Patrimonio.Financas.Wpf.Common.ViewModels;
using Patrimonio.Financas.Wpf.Contas.HttpClients;

namespace Patrimonio.Financas.Wpf.Contas.ViewModels;

/// <summary>
/// ViewModel responsável pela alteração de contas.
/// </summary>
internal sealed partial class ContaAlteracaoViewModel(
    ContaHttpClient client,
    ExceptionHandler exceptionHandler) : BaseViewModel
{
    private Func<CancellationToken, Task> _voltar = _ => Task.CompletedTask;

    /// <inheritdoc />
    public override string Titulo => "Editar conta";

    [ObservableProperty] public partial int ContaId { get; set; }
    [ObservableProperty] public partial string Nome { get; set; } = string.Empty;
    [ObservableProperty] public partial IReadOnlyCollection<OpcaoDto> Instituicoes { get; set; } = [];

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SalvarCommand))]
    public partial int? InstituicaoId { get; set; }

    /// <summary>
    /// Carrega os dados da conta a ser alterada.
    /// </summary>
    /// <param name="voltar">Função de callback utilizada para retornar à tela anterior.</param>
    /// <param name="contaId">Identificador da conta a ser carregada.</param>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    public async Task InicializarAsync(
        Func<CancellationToken, Task> voltar,
        int contaId,
        CancellationToken cancellationToken)
    {
        try
        {
            Carregando = true;
            _voltar = voltar;
            ContaId = contaId;

            var contaTask = client.ObterPorIdAsync(
                contaId,
                cancellationToken);

            var opcoesTask = client.ObterOpcoesCriacaoAsync(
                cancellationToken);

            await Task.WhenAll(
                contaTask,
                opcoesTask);

            var conta = await contaTask;
            var opcoes = await opcoesTask;

            Nome = conta.Nome;
            InstituicaoId = conta.InstituicaoId;

            Instituicoes = opcoes.Instituicoes;
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
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    [RelayCommand(CanExecute = nameof(PodeSalvar))]
    private async Task SalvarAsync(CancellationToken cancellationToken)
    {
        try
        {
            Carregando = true;

            await client.AlterarAsync(
                ContaId,
                new ContaAlteracaoDto
                {
                    Nome = Nome,
                    InstituicaoId = InstituicaoId!.Value
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

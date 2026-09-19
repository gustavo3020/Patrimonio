using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Patrimonio.Financas.Contracts.Cartoes.Dtos;
using Patrimonio.Financas.Contracts.Common.Dtos;
using Patrimonio.Financas.SharedKernel.Cartoes.Enums;
using Patrimonio.Financas.Wpf.Cartoes.HttpClients;
using Patrimonio.Financas.Wpf.Common.Exceptions;
using Patrimonio.Financas.Wpf.Common.ViewModels;

namespace Patrimonio.Financas.Wpf.Cartoes.ViewModels;

/// <summary>
/// ViewModel responsável pela alteração de cartões.
/// </summary>
internal sealed partial class CartaoAlteracaoViewModel(
    CartaoHttpClient client,
    ExceptionHandler exceptionHandler) : BaseViewModel
{
    private Func<CancellationToken, Task> _voltar = _ => Task.CompletedTask;

    /// <inheritdoc />
    public override string Titulo => "Editar Cartão";

    [ObservableProperty] public partial int CartaoId { get; set; }
    [ObservableProperty] public partial string Nome { get; set; } = string.Empty;
    [ObservableProperty] public partial decimal Limite { get; set; }
    [ObservableProperty] public partial int DiaFechamento { get; set; }
    [ObservableProperty] public partial int DiaVencimento { get; set; }
    [ObservableProperty] public partial IReadOnlyCollection<OpcaoDto> Instituicoes { get; set; } = [];


    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SalvarCommand))]
    public partial BandeiraCartao? Bandeira { get; set; }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SalvarCommand))]
    public partial int? InstituicaoId { get; set; }

    public static IReadOnlyCollection<BandeiraCartao> Bandeiras { get; } = Enum.GetValues<BandeiraCartao>();


    /// <summary>
    /// Carrega os dados do cartão a ser alterado.
    /// </summary>
    /// <param name="voltar">Função de callback utilizada para retornar à tela anterior.</param>
    /// <param name="cartaoId">Identificador do cartão a ser carregado.</param>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    public async Task InicializarAsync(
        Func<CancellationToken, Task> voltar,
        int cartaoId,
        CancellationToken cancellationToken)
    {
        try
        {
            Carregando = true;
            _voltar = voltar;
            CartaoId = cartaoId;

            var cartaoTask = client.ObterPorIdAsync(
                cartaoId,
                cancellationToken);

            var opcoesTask = client.ObterOpcoesCriacaoAsync(
                cancellationToken);

            await Task.WhenAll(
                cartaoTask,
                opcoesTask);

            var cartao = await cartaoTask;
            var opcoes = await opcoesTask;

            Nome = cartao.Nome;
            Bandeira = cartao.Bandeira;
            Limite = cartao.Limite;
            DiaFechamento = cartao.DiaFechamento;
            DiaVencimento = cartao.DiaVencimento;
            InstituicaoId = cartao.InstituicaoId;

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

    public bool PodeAlterarInstituicao { get; } = false;
    private bool PodeSalvar() => Bandeira is not null;

    /// <summary>
    /// Salva as alterações realizadas no cartão.
    /// </summary>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    [RelayCommand(CanExecute = nameof(PodeSalvar))]
    private async Task SalvarAsync(CancellationToken cancellationToken)
    {
        try
        {
            Carregando = true;

            await client.AlterarAsync(
                CartaoId,
                new CartaoAlteracaoDto
                {
                    Nome = Nome,
                    Bandeira = Bandeira!.Value,
                    Limite = Limite,
                    DiaFechamento = DiaFechamento,
                    DiaVencimento = DiaVencimento
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

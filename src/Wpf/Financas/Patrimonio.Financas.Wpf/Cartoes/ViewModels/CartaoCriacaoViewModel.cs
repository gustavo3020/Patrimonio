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
/// ViewModel responsável pela criação de cartoes.
/// </summary>
internal sealed partial class CartaoCriacaoViewModel(
    CartaoHttpClient client,
    ExceptionHandler exceptionHandler) : BaseViewModel
{
    private Func<CancellationToken, Task> _voltar = _ => Task.CompletedTask;

    /// <inheritdoc />
    public override string Titulo => "Novo cartão";

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
    /// Carrega as instituições disponíveis para seleção no formulário.
    /// </summary>
    /// <param name="voltar">Função de callback utilizada para retornar à tela anterior.</param>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    public async Task InicializarAsync(
        Func<CancellationToken, Task> voltar,
        CancellationToken cancellationToken)
    {
        try
        {
            Carregando = true;
            _voltar = voltar;

            var opcoes = await client.ObterOpcoesCriacaoAsync(cancellationToken);

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

    private bool PodeSalvar() =>
        Bandeira is not null &&
        InstituicaoId is not null;

    /// <summary>
    /// Cria uma nova cartao utilizando os dados informados no formulário.
    /// </summary>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    [RelayCommand(CanExecute = nameof(PodeSalvar))]
    private async Task SalvarAsync(CancellationToken cancellationToken)
    {
        try
        {
            Carregando = true;

            await client.CriarAsync(
                new CartaoCriacaoDto
                {
                    Nome = Nome,
                    Bandeira = Bandeira!.Value,
                    Limite = Limite,
                    DiaFechamento = DiaFechamento,
                    DiaVencimento = DiaVencimento,
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

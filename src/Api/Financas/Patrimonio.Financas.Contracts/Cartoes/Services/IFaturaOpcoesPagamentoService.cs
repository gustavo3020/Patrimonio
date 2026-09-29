using Patrimonio.Financas.Contracts.Cartoes.Dtos;

namespace Patrimonio.Financas.Contracts.Cartoes.Services;

/// <summary>
/// Implementa a obtenção das opções de referência necessárias para o pagamento de uma fatura.
/// </summary>
public interface IFaturaOpcoesPagamentoService
{
    Task<FaturaOpcoesPagamentoDto> ObterOpcoesPagamentoAsync(CancellationToken cancellationToken);
}

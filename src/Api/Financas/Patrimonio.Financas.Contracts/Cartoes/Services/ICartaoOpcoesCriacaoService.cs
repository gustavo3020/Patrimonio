using Patrimonio.Financas.Contracts.Cartoes.Dtos;

namespace Patrimonio.Financas.Contracts.Cartoes.Services;

/// <summary>
/// Implementa a obtenção das opções de referência necessárias para a criação de um cartão.
/// </summary>
public interface ICartaoOpcoesCriacaoService
{
    Task<CartaoOpcoesCriacaoDto> ObterOpcoesCriacaoAsync(CancellationToken cancellationToken);
}

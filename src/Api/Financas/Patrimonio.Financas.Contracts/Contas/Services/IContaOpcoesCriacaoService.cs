using Patrimonio.Financas.Contracts.Contas.Dtos;

namespace Patrimonio.Financas.Contracts.Contas.Services;

/// <summary>
/// Implementa a obtenção das opções de referência necessárias para a criação de uma conta.
/// </summary>
public interface IContaOpcoesCriacaoService
{
    Task<ContaOpcoesCriacaoDto> ObterOpcoesCriacaoAsync(CancellationToken cancellationToken);
}

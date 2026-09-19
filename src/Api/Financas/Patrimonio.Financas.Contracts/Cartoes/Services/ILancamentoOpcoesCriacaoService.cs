using Patrimonio.Financas.Contracts.Cartoes.Dtos;

namespace Patrimonio.Financas.Contracts.Cartoes.Services;

/// <summary>
/// Implementa a obtenção das opções de referência necessárias para a criação de um lançamento.
/// </summary>
public interface ILancamentoOpcoesCriacaoService
{
    Task<LancamentoOpcoesCriacaoDto> ObterOpcoesCriacaoAsync(CancellationToken cancellationToken);
}

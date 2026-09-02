using Patrimonio.Financas.Contracts.Movimentacoes.Dtos;

namespace Patrimonio.Financas.Contracts.Movimentacoes.Services;

/// <summary>
/// Implementa a obtenção das opções de referência necessárias para a criação de uma movimentação.
/// </summary>
public interface IMovimentacaoOpcoesCriacaoService
{
    Task<MovimentacaoOpcoesCriacaoDto> ObterOpcoesCriacaoAsync(CancellationToken cancellationToken);
}

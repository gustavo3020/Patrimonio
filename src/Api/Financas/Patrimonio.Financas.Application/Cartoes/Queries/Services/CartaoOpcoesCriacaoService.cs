using Patrimonio.Financas.Application.Instituicoes.Lookups.Abstractions;
using Patrimonio.Financas.Contracts.Cartoes.Dtos;
using Patrimonio.Financas.Contracts.Cartoes.Services;
using Patrimonio.Financas.Contracts.Common.Dtos;

namespace Patrimonio.Financas.Application.Cartoes.Queries.Services;

/// <summary>
/// Implementa as operações para obtenção das opções necessárias à criação de um cartão.
/// </summary>
internal class CartaoOpcoesCriacaoService(
    IInstituicaoLookupRepository instituicaoLookupRepository) : ICartaoOpcoesCriacaoService
{
    /// <inheritdoc/>
    public async Task<CartaoOpcoesCriacaoDto> ObterOpcoesCriacaoAsync(CancellationToken cancellationToken)
    {
        var instituicoes = await instituicaoLookupRepository.ListarOpcoesAsync(cancellationToken);

        return new CartaoOpcoesCriacaoDto
        {
            Instituicoes = [.. instituicoes
                .Select(c => new OpcaoDto
                {
                    Id = c.Id,
                    Nome = c.Nome
                })]
        };
    }
}

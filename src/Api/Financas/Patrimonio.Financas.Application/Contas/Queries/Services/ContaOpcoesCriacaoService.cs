using Patrimonio.Financas.Application.Instituicoes.Lookups.Abstractions;
using Patrimonio.Financas.Contracts.Common.Dtos;
using Patrimonio.Financas.Contracts.Contas.Dtos;
using Patrimonio.Financas.Contracts.Contas.Services;

namespace Patrimonio.Financas.Application.Contas.Queries.Services;

/// <summary>
/// Implementa as operações para obtenção das opções necessárias à criação de uma conta.
/// </summary>
internal class ContaOpcoesCriacaoService(
    IInstituicaoLookupRepository instituicaoLookupRepository) : IContaOpcoesCriacaoService
{
    /// <inheritdoc/>
    public async Task<ContaOpcoesCriacaoDto> ObterOpcoesCriacaoAsync(CancellationToken cancellationToken)
    {
        var instituicoes = await instituicaoLookupRepository.ListarOpcoesAsync(cancellationToken);

        return new ContaOpcoesCriacaoDto
        {
            Instituicoes = [.. instituicoes
                .Select(i => new OpcaoDto
                {
                    Id = i.Id,
                    Nome = i.Nome
                })]
        };
    }
}

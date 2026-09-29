using Patrimonio.Financas.Application.Categorias.Lookups.Abstractions;
using Patrimonio.Financas.Contracts.Cartoes.Dtos;
using Patrimonio.Financas.Contracts.Cartoes.Services;
using Patrimonio.Financas.Contracts.Common.Dtos;

namespace Patrimonio.Financas.Application.Cartoes.Queries.Services;

/// <summary>
/// Implementa as operações para obtenção das opções necessárias à criação de um lançamento.
/// </summary>
internal sealed class LancamentoOpcoesCriacaoService(
    ICategoriaLookupRepository categoriaLookupRepository) : ILancamentoOpcoesCriacaoService
{
    /// <inheritdoc/>
    public async Task<LancamentoOpcoesCriacaoDto> ObterOpcoesCriacaoAsync(CancellationToken cancellationToken)
    {
        var categorias = await categoriaLookupRepository.ListarOpcoesAsync(cancellationToken);

        return new LancamentoOpcoesCriacaoDto
        {
            Categorias = [.. categorias
                .Select(c => new OpcaoDto
                {
                    Id = c.Id,
                    Nome = c.Nome
                })]
        };
    }
}

using Patrimonio.Financas.Application.Categorias.Lookups.Abstractions;
using Patrimonio.Financas.Application.Contas.Lookups.Abstractions;
using Patrimonio.Financas.Contracts.Common.Dtos;
using Patrimonio.Financas.Contracts.Movimentacoes.Dtos;
using Patrimonio.Financas.Contracts.Movimentacoes.Services;

namespace Patrimonio.Financas.Application.Movimentacoes.Queries.Services;

/// <summary>
/// Implementa as operações para obtenção das opções necessárias à criação de uma movimentação.
/// </summary>
internal class MovimentacaoOpcoesCriacaoService(
    ICategoriaLookupRepository categoriaLookupRepository,
    IContaLookupRepository contaLookupRepository) : IMovimentacaoOpcoesCriacaoService
{
    /// <inheritdoc/>
    public async Task<MovimentacaoOpcoesCriacaoDto> ObterOpcoesCriacaoAsync(CancellationToken cancellationToken)
    {
        var categorias = await categoriaLookupRepository.ListarOpcoesAsync(cancellationToken);
        var contas = await contaLookupRepository.ListarOpcoesAsync(cancellationToken);

        return new MovimentacaoOpcoesCriacaoDto
        {
            Categorias = [.. categorias
                .Select(c => new OpcaoDto
                {
                    Id = c.Id,
                    Nome = c.Nome
                })],

            Contas = [.. contas
                .Select(c => new OpcaoDto
                {
                    Id = c.Id,
                    Nome = c.Nome
                })]
        };
    }
}

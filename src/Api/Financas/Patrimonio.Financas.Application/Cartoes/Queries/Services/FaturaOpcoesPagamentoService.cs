using Patrimonio.Financas.Application.Categorias.Lookups.Abstractions;
using Patrimonio.Financas.Application.Contas.Lookups.Abstractions;
using Patrimonio.Financas.Contracts.Cartoes.Dtos;
using Patrimonio.Financas.Contracts.Cartoes.Services;
using Patrimonio.Financas.Contracts.Common.Dtos;

namespace Patrimonio.Financas.Application.Cartoes.Queries.Services;

/// <summary>
/// Implementa as operações para obtenção das opções necessárias ao pagamento de uma fatura.
/// </summary>
internal sealed class FaturaOpcoesPagamentoService(
    ICategoriaLookupRepository categoriaLookupRepository,
    IContaLookupRepository contaLookupRepository) : IFaturaOpcoesPagamentoService
{
    /// <inheritdoc/>
    public async Task<FaturaOpcoesPagamentoDto> ObterOpcoesPagamentoAsync(CancellationToken cancellationToken)
    {
        var categorias = await categoriaLookupRepository.ListarOpcoesAsync(cancellationToken);
        var contas = await contaLookupRepository.ListarOpcoesAsync(cancellationToken);

        return new FaturaOpcoesPagamentoDto
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

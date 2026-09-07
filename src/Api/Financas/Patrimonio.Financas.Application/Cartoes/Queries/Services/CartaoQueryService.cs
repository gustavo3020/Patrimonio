using Microsoft.Extensions.Logging;
using Patrimonio.Financas.Application.Cartoes.Mappers;
using Patrimonio.Financas.Application.Cartoes.Queries.Abstractions;
using Patrimonio.Financas.Contracts.Cartoes.Dtos;
using Patrimonio.Financas.Contracts.Cartoes.Services;
using Patrimonio.Financas.Domain.Exceptions;

namespace Patrimonio.Financas.Application.Cartoes.Queries.Services;

/// <summary>
/// Implementa as operações de leitura disponíveis para cartões.
/// </summary>
internal sealed class CartaoQueryService(
    ICartaoQueryRepository queryRepository,
    ILogger<CartaoQueryService> logger) : ICartaoQueryService
{
    /// <inheritdoc />
    public async Task<IReadOnlyCollection<CartaoListaDto>> ListarAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Listando cartões.");

        var movimentacoes = await queryRepository.ListarAsync(cancellationToken);

        return [.. movimentacoes.Select(CartaoMapper.Mapear)];
    }

    /// <inheritdoc />
    public async Task<CartaoDetalheDto> ObterPorIdAsync(int cartaoId, CancellationToken cancellationToken)
    {
        logger.LogInformation("Obtendo cartão por ID {CartaoId}.", cartaoId);

        var movimentacao = await queryRepository.ObterPorIdAsync(cartaoId, cancellationToken)
            ?? throw new RecursoNaoEncontradoException($"Cartão com Id {cartaoId} não encontrado.");

        return CartaoMapper.Mapear(movimentacao);
    }
}

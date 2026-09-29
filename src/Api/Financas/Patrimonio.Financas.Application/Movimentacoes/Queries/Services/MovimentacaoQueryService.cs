using Microsoft.Extensions.Logging;
using Patrimonio.Financas.Application.Movimentacoes.Mappers;
using Patrimonio.Financas.Application.Movimentacoes.Queries.Abstractions;
using Patrimonio.Financas.Contracts.Movimentacoes.Dtos;
using Patrimonio.Financas.Contracts.Movimentacoes.Services;
using Patrimonio.Financas.Domain.Exceptions;

namespace Patrimonio.Financas.Application.Movimentacoes.Queries.Services;

/// <summary>
/// Implementa as operações de leitura disponíveis para movimentações.
/// </summary>
internal sealed class MovimentacaoQueryService(
    IMovimentacaoQueryRepository queryRepository,
    ILogger<MovimentacaoQueryService> logger) : IMovimentacaoQueryService
{
    /// <inheritdoc />
    public async Task<IReadOnlyCollection<MovimentacaoListaDto>> ListarAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Listando movimentações.");

        var movimentacoes = await queryRepository.ListarAsync(cancellationToken);

        return [.. movimentacoes.Select(MovimentacaoMapper.Mapear)];
    }

    /// <inheritdoc />
    public async Task<MovimentacaoDetalheDto> ObterPorIdAsync(int movimentacaoId, CancellationToken cancellationToken)
    {
        logger.LogInformation("Obtendo movimentação por ID {MovimentacaoId}.", movimentacaoId);

        var movimentacao = await queryRepository.ObterPorIdAsync(movimentacaoId, cancellationToken)
            ?? throw new RecursoNaoEncontradoException($"Movimentação com Id {movimentacaoId} não encontrada.");

        return MovimentacaoMapper.Mapear(movimentacao);
    }
}

using Microsoft.Extensions.Logging;
using Patrimonio.Financas.Application.Cartoes.Mappers;
using Patrimonio.Financas.Application.Cartoes.Queries.Abstractions;
using Patrimonio.Financas.Contracts.Cartoes.Dtos;
using Patrimonio.Financas.Contracts.Cartoes.Services;
using Patrimonio.Financas.Domain.Exceptions;

namespace Patrimonio.Financas.Application.Cartoes.Queries.Services;

/// <summary>
/// Implementa as operações de leitura disponíveis para lançamentos.
/// </summary>
internal sealed class LancamentoQueryService(
    ILancamentoQueryRepository queryRepository,
    ILogger<LancamentoQueryService> logger) : ILancamentoQueryService
{
    /// <inheritdoc />
    public async Task<IReadOnlyCollection<LancamentoListaDto>> ListarAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Listando lançamentos.");

        var lancamentos = await queryRepository.ListarAsync(cancellationToken);

        return [.. lancamentos.Select(LancamentoMapper.Mapear)];
    }

    /// <inheritdoc />
    public async Task<LancamentoDetalheDto> ObterPorIdAsync(int lancamentoId, CancellationToken cancellationToken)
    {
        logger.LogInformation("Obtendo lançamento por ID {LancamentoId}.", lancamentoId);

        var lancamento = await queryRepository.ObterPorIdAsync(lancamentoId, cancellationToken)
            ?? throw new RecursoNaoEncontradoException($"Lançamento com Id {lancamentoId} não encontrado.");

        return LancamentoMapper.Mapear(lancamento);
    }
}

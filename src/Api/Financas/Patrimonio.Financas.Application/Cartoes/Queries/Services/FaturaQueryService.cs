using Microsoft.Extensions.Logging;
using Patrimonio.Financas.Application.Cartoes.Mappers;
using Patrimonio.Financas.Application.Cartoes.Queries.Abstractions;
using Patrimonio.Financas.Contracts.Cartoes.Dtos;
using Patrimonio.Financas.Contracts.Cartoes.Services;
using Patrimonio.Financas.Domain.Exceptions;

namespace Patrimonio.Financas.Application.Cartoes.Queries.Services;

/// <summary>
/// Implementa as operações de leitura disponíveis para faturas.
/// </summary>
internal sealed class FaturaQueryService(
    IFaturaQueryRepository queryRepository,
    ILogger<FaturaQueryService> logger) : IFaturaQueryService
{
    /// <inheritdoc />
    public async Task<IReadOnlyCollection<FaturaListaDto>> ListarAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Listando faturas.");

        var faturas = await queryRepository.ListarAsync(cancellationToken);

        return [.. faturas.Select(FaturaMapper.Mapear)];
    }

    /// <inheritdoc />
    public async Task<FaturaDetalheDto> ObterPorIdAsync(int faturaId, CancellationToken cancellationToken)
    {
        logger.LogInformation("Obtendo fatura por ID {FaturaId}.", faturaId);

        var fatura = await queryRepository.ObterPorIdAsync(faturaId, cancellationToken)
            ?? throw new RecursoNaoEncontradoException($"Fatura com Id {faturaId} não encontrada.");

        return FaturaMapper.Mapear(fatura);
    }
}

using Microsoft.Extensions.Logging;
using Patrimonio.Financas.Application.Instituicoes.Mappers;
using Patrimonio.Financas.Application.Instituicoes.Queries.Abstractions;
using Patrimonio.Financas.Contracts.Instituicoes.Dtos;
using Patrimonio.Financas.Contracts.Instituicoes.Services;
using Patrimonio.Financas.Domain.Exceptions;

namespace Patrimonio.Financas.Application.Instituicoes.Queries.Services;

/// <summary>
/// Implementa as operações de leitura disponíveis para instituições.
/// </summary>
internal sealed class InstituicaoQueryService(
    IInstituicaoQueryRepository queryRepository,
    ILogger<InstituicaoQueryService> logger) : IInstituicaoQueryService
{
    /// <inheritdoc />
    public async Task<IReadOnlyCollection<InstituicaoListaDto>> ListarAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Listando instituições.");

        var instituicoes = await queryRepository.ListarAsync(cancellationToken);

        return [.. instituicoes.Select(InstituicaoMapper.Mapear)];
    }

    /// <inheritdoc />
    public async Task<InstituicaoDetalheDto> ObterPorIdAsync(int instituicaoId, CancellationToken cancellationToken)
    {
        logger.LogInformation("Obtendo instituição por ID {InstituicaoId}.", instituicaoId);

        var instituicao = await queryRepository.ObterPorIdAsync(instituicaoId, cancellationToken)
            ?? throw new RecursoNaoEncontradoException($"Instituição com Id {instituicaoId} não encontrada.");

        return InstituicaoMapper.Mapear(instituicao);
    }
}

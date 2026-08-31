using Microsoft.Extensions.Logging;
using Patrimonio.Financas.Application.Contas.Mappers;
using Patrimonio.Financas.Application.Contas.Queries.Abstractions;
using Patrimonio.Financas.Contracts.Contas.Dtos;
using Patrimonio.Financas.Contracts.Contas.Services;
using Patrimonio.Financas.Domain.Exceptions;

namespace Patrimonio.Financas.Application.Contas.Queries.Services;

/// <summary>
/// Implementa as operações de leitura disponíveis para contas.
/// </summary>
internal sealed class ContaQueryService(
    IContaQueryRepository queryRepository,
    ILogger<ContaQueryService> logger) : IContaQueryService
{
    /// <inheritdoc />
    public async Task<IReadOnlyCollection<ContaListaDto>> ListarAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Listando contas.");

        var contas = await queryRepository.ListarAsync(cancellationToken);

        return [.. contas.Select(ContaMapper.Mapear)];
    }

    /// <inheritdoc />
    public async Task<ContaDetalheDto> ObterPorIdAsync(int contaId, CancellationToken cancellationToken)
    {
        logger.LogInformation("Obtendo conta por ID {ContaId}.", contaId);

        var conta = await queryRepository.ObterPorIdAsync(contaId, cancellationToken)
            ?? throw new RecursoNaoEncontradoException($"Conta com Id {contaId} não encontrada.");

        return ContaMapper.Mapear(conta);
    }
}

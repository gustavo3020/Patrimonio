using Microsoft.Extensions.Logging;
using Patrimonio.Financas.Application.Instituicoes.Commands.Abstractions;
using Patrimonio.Financas.Application.Instituicoes.Mappers;
using Patrimonio.Financas.Application.Instituicoes.Queries.Abstractions;
using Patrimonio.Financas.Contracts.Instituicoes.Dtos;
using Patrimonio.Financas.Contracts.Instituicoes.Services;
using Patrimonio.Financas.Domain.Common.ValueObjects;
using Patrimonio.Financas.Domain.Exceptions;
using Patrimonio.Financas.Domain.Instituicoes.Entities;

namespace Patrimonio.Financas.Application.Instituicoes.Commands.Services;

/// <summary>
/// Implementa as operações de escrita disponíveis para instituições.
/// </summary>
internal sealed class InstituicaoCommandService(
    IInstituicaoCommandRepository commandRepository,
    IInstituicaoQueryRepository queryRepository,
    ILogger<InstituicaoCommandService> logger) : IInstituicaoCommandService
{
    /// <inheritdoc />
    public async Task<InstituicaoDetalheDto> CriarAsync(InstituicaoCriacaoDto dto, CancellationToken cancellationToken)
    {
        logger.LogInformation("Criando instituição {Nome}.", dto.Nome);

        var entidade = Instituicao.Criar(new Nome(dto.Nome));

        commandRepository.Adicionar(entidade);

        await commandRepository.SalvarAsync(cancellationToken);

        var entidadeDetalhe = await queryRepository.ObterPorIdAsync(entidade.Id, cancellationToken);

        return InstituicaoMapper.Mapear(entidadeDetalhe!);
    }

    /// <inheritdoc />
    public async Task AlterarAsync(int instituicaoId, InstituicaoAlteracaoDto dto, CancellationToken cancellationToken)
    {
        logger.LogInformation("Alterando instituição {InstituicaoId}.", instituicaoId);

        var entidade = await commandRepository.ObterPorIdAsync(instituicaoId, cancellationToken)
            ?? throw new RecursoNaoEncontradoException($"Instituição com Id {instituicaoId} não encontrada.");

        entidade.AlterarNome(new Nome(dto.Nome));

        await commandRepository.SalvarAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task ExcluirAsync(int instituicaoId, CancellationToken cancellationToken)
    {
        logger.LogInformation("Excluindo instituição {InstituicaoId}.", instituicaoId);

        var entidade = await commandRepository.ObterPorIdAsync(instituicaoId, cancellationToken)
            ?? throw new RecursoNaoEncontradoException($"Instituição com Id {instituicaoId} não encontrada.");

        commandRepository.Remover(entidade);

        await commandRepository.SalvarAsync(cancellationToken);
    }
}

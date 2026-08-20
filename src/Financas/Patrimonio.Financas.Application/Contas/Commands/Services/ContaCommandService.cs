using Microsoft.Extensions.Logging;
using Patrimonio.Financas.Application.Contas.Commands.Abstractions;
using Patrimonio.Financas.Application.Contas.Mappers;
using Patrimonio.Financas.Contracts.Contas.Dtos;
using Patrimonio.Financas.Contracts.Contas.Services;
using Patrimonio.Financas.Domain.Contas.Entities;
using Patrimonio.Financas.Domain.Common.ValueObjects;
using Patrimonio.Financas.Application.Contas.Queries.Abstractions;

namespace Patrimonio.Financas.Application.Contas.Commands.Services;

/// <summary>
/// Implementa as operações de escrita disponíveis para contas.
/// </summary>
internal sealed class ContaCommandService(
    IContaCommandRepository commandRepository,
    IContaQueryRepository queryRepository,
    ILogger<ContaCommandService> logger) : IContaCommandService
{
    /// <inheritdoc />
    public async Task<ContaDetalheDto> CriarAsync(ContaCriacaoDto dto, CancellationToken cancellationToken)
    {
        logger.LogInformation("Criando conta {Nome}.", dto.Nome);

        var entidade = Conta.Criar(new Nome(dto.Nome), dto.InstituicaoId);

        commandRepository.Adicionar(entidade);

        await commandRepository.SalvarAsync(cancellationToken);

        var entidadeDetalhe = await queryRepository.ObterPorIdAsync(entidade.Id, cancellationToken);

        return ContaMapper.Mapear(entidadeDetalhe);
    }

    /// <inheritdoc />
    public async Task AlterarAsync(int contaId, ContaAlteracaoDto dto, CancellationToken cancellationToken)
    {
        logger.LogInformation("Alterando conta {ContaId}.", contaId);

        var entidade = await commandRepository.ObterPorIdAsync(contaId, cancellationToken);

        entidade.AlterarNome(new Nome(dto.Nome));
        entidade.AlterarInstituicao(dto.InstituicaoId);

        await commandRepository.SalvarAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task ExcluirAsync(int contaId, CancellationToken cancellationToken)
    {
        logger.LogInformation("Excluindo conta {ContaId}.", contaId);

        var entidade = await commandRepository.ObterPorIdAsync(contaId, cancellationToken);

        commandRepository.Remover(entidade);

        await commandRepository.SalvarAsync(cancellationToken);
    }
}

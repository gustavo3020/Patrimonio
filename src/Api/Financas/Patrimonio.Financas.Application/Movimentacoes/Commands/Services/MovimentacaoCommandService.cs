using Microsoft.Extensions.Logging;
using Patrimonio.Financas.Application.Movimentacoes.Commands.Abstractions;
using Patrimonio.Financas.Application.Movimentacoes.Mappers;
using Patrimonio.Financas.Application.Movimentacoes.Queries.Abstractions;
using Patrimonio.Financas.Contracts.Movimentacoes.Dtos;
using Patrimonio.Financas.Contracts.Movimentacoes.Services;
using Patrimonio.Financas.Domain.Common.ValueObjects;
using Patrimonio.Financas.Domain.Exceptions;
using Patrimonio.Financas.Domain.Movimentacoes.Entities;

namespace Patrimonio.Financas.Application.Movimentacoes.Commands.Services;

/// <summary>
/// Implementa as operações de escrita disponíveis para movimentações.
/// </summary>
internal sealed class MovimentacaoCommandService(
    IMovimentacaoCommandRepository commandRepository,
    IMovimentacaoQueryRepository queryRepository,
    ILogger<MovimentacaoCommandService> logger) : IMovimentacaoCommandService
{
    /// <inheritdoc />
    public async Task<MovimentacaoDetalheDto> CriarAsync(MovimentacaoCriacaoDto dto, CancellationToken cancellationToken)
    {
        logger.LogInformation("Criando movimentação para a data {Data}.", dto.Data);

        var descricao = string.IsNullOrWhiteSpace(dto.Descricao)
            ? null
            : new Descricao(dto.Descricao);

        var entidade = Movimentacao.Criar(
            dto.Data,
            new Dinheiro(dto.Valor),
            dto.Natureza,
            dto.Tipo,
            descricao,
            dto.ContaId,
            dto.CategoriaId);

        commandRepository.Adicionar(entidade);

        await commandRepository.SalvarAsync(cancellationToken);

        var entidadeDetalhe = await queryRepository.ObterPorIdAsync(entidade.Id, cancellationToken);

        return MovimentacaoMapper.Mapear(entidadeDetalhe!);
    }

    /// <inheritdoc />
    public async Task AlterarAsync(int movimentacaoId, MovimentacaoAlteracaoDto dto, CancellationToken cancellationToken)
    {
        logger.LogInformation("Alterando movimentação {MovimentacaoId}.", movimentacaoId);

        var entidade = await commandRepository.ObterPorIdAsync(movimentacaoId, cancellationToken)
            ?? throw new RecursoNaoEncontradoException($"Movimentação com Id {movimentacaoId} não encontrada.");

        var descricao = string.IsNullOrWhiteSpace(dto.Descricao)
            ? null
            : new Descricao(dto.Descricao);

        entidade.Alterar(
            dto.Data,
            new Dinheiro(dto.Valor),
            dto.Natureza,
            dto.Tipo,
            descricao,
            dto.ContaId,
            dto.CategoriaId);

        await commandRepository.SalvarAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task ExcluirAsync(int movimentacaoId, CancellationToken cancellationToken)
    {
        logger.LogInformation("Excluindo movimentação {MovimentacaoId}.", movimentacaoId);

        var entidade = await commandRepository.ObterPorIdAsync(movimentacaoId, cancellationToken)
            ?? throw new RecursoNaoEncontradoException($"Movimentação com Id {movimentacaoId} não encontrada.");

        commandRepository.Remover(entidade);

        await commandRepository.SalvarAsync(cancellationToken);
    }
}

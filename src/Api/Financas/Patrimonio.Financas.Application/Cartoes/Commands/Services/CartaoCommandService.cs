using Microsoft.Extensions.Logging;
using Patrimonio.Financas.Application.Cartoes.Commands.Abstractions;
using Patrimonio.Financas.Application.Cartoes.Mappers;
using Patrimonio.Financas.Application.Cartoes.Queries.Abstractions;
using Patrimonio.Financas.Contracts.Cartoes.Dtos;
using Patrimonio.Financas.Contracts.Cartoes.Services;
using Patrimonio.Financas.Domain.Cartoes.Entities;
using Patrimonio.Financas.Domain.Common.ValueObjects;
using Patrimonio.Financas.Domain.Exceptions;

namespace Patrimonio.Financas.Application.Cartoes.Commands.Services;

/// <summary>
/// Implementa as operações de escrita disponíveis para cartões.
/// </summary>
internal sealed class CartaoCommandService(
    ICartaoCommandRepository commandRepository,
    ICartaoQueryRepository queryRepository,
    ILogger<CartaoCommandService> logger) : ICartaoCommandService
{
    /// <inheritdoc />
    public async Task<CartaoDetalheDto> CriarAsync(CartaoCriacaoDto dto, CancellationToken cancellationToken)
    {
        logger.LogInformation("Criando cartão para a instituição {Instituicao}.", dto.InstituicaoId);

        var entidade = Cartao.Criar(
            new Nome(dto.Nome),
            dto.Bandeira,
            new Dinheiro(dto.Limite),
            new DiaDoMes(dto.DiaFechamento),
            new DiaDoMes(dto.DiaVencimento),
            dto.InstituicaoId);

        commandRepository.Adicionar(entidade);

        await commandRepository.SalvarAsync(cancellationToken);

        var entidadeDetalhe = await queryRepository.ObterPorIdAsync(entidade.Id, cancellationToken);

        return CartaoMapper.Mapear(entidadeDetalhe!);
    }

    /// <inheritdoc />
    public async Task AlterarAsync(int cartaoId, CartaoAlteracaoDto dto, CancellationToken cancellationToken)
    {
        logger.LogInformation("Alterando cartão {CartaoId}.", cartaoId);

        var entidade = await commandRepository.ObterPorIdAsync(cartaoId, cancellationToken)
            ?? throw new RecursoNaoEncontradoException($"Cartão com Id {cartaoId} não encontrado.");

        entidade.Alterar(
            new Nome(dto.Nome),
            dto.Bandeira,
            new Dinheiro(dto.Limite),
            new DiaDoMes(dto.DiaFechamento),
            new DiaDoMes(dto.DiaVencimento));

        await commandRepository.SalvarAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task ExcluirAsync(int cartaoId, CancellationToken cancellationToken)
    {
        logger.LogInformation("Excluindo cartão {CartaoId}.", cartaoId);

        var entidade = await commandRepository.ObterPorIdAsync(cartaoId, cancellationToken)
            ?? throw new RecursoNaoEncontradoException($"Cartão com Id {cartaoId} não encontrado.");

        commandRepository.Remover(entidade);

        await commandRepository.SalvarAsync(cancellationToken);
    }
}

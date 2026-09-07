using Microsoft.Extensions.Logging;
using Patrimonio.Financas.Application.Cartoes.Commands.Abstractions;
using Patrimonio.Financas.Application.Cartoes.Mappers;
using Patrimonio.Financas.Application.Cartoes.Queries.Abstractions;
using Patrimonio.Financas.Application.Movimentacoes.Commands.Abstractions;
using Patrimonio.Financas.Contracts.Cartoes.Dtos;
using Patrimonio.Financas.Contracts.Cartoes.Services;
using Patrimonio.Financas.Domain.Cartoes.Entities;
using Patrimonio.Financas.Domain.Common.ValueObjects;
using Patrimonio.Financas.Domain.Exceptions;
using Patrimonio.Financas.Domain.Movimentacoes.Entities;
using Patrimonio.Financas.SharedKernel.Movimentacoes.Enums;

namespace Patrimonio.Financas.Application.Cartoes.Commands.Services;

/// <summary>
/// Implementa as operações de escrita disponíveis para faturas.
/// </summary>
internal sealed class FaturaCommandService(
    IFaturaCommandRepository commandRepository,
    IFaturaQueryRepository queryRepository,
    ILancamentoQueryRepository lancamentoQueryRepository,
    IMovimentacaoCommandRepository movimentacaoCommandRepository,
    ILogger<FaturaCommandService> logger) : IFaturaCommandService
{
    /// <inheritdoc />
    public async Task<FaturaDetalheDto> CriarAsync(FaturaCriacaoDto dto, CancellationToken cancellationToken)
    {
        logger.LogInformation("Criando fatura.");

        var entidade = Fatura.Criar(dto.DataFechamento, dto.DataVencimento, dto.CartaoId);

        commandRepository.Adicionar(entidade);

        await commandRepository.SalvarAsync(cancellationToken);

        var entidadeDetalhe = await queryRepository.ObterPorIdAsync(entidade.Id, cancellationToken);

        return FaturaMapper.Mapear(entidadeDetalhe!);
    }

    /// <inheritdoc />
    public async Task AlterarAsync(int faturaId, FaturaAlteracaoDto dto, CancellationToken cancellationToken)
    {
        logger.LogInformation("Alterando fatura {FaturaId}.", faturaId);

        var entidade = await commandRepository.ObterPorIdAsync(faturaId, cancellationToken)
            ?? throw new RecursoNaoEncontradoException($"Fatura com Id {faturaId} não encontrada.");

        entidade.Alterar(dto.DataFechamento, dto.DataVencimento);

        await commandRepository.SalvarAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task ExcluirAsync(int faturaId, CancellationToken cancellationToken)
    {
        logger.LogInformation("Excluindo fatura {FaturaId}.", faturaId);

        var entidade = await commandRepository.ObterPorIdAsync(faturaId, cancellationToken)
            ?? throw new RecursoNaoEncontradoException($"Fatura com Id {faturaId} não encontrada.");

        entidade.Excluir();

        commandRepository.Remover(entidade);

        await commandRepository.SalvarAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task FecharAsync(int faturaId, CancellationToken cancellationToken)
    {
        logger.LogInformation("Fechando fatura {FaturaId}.", faturaId);

        var entidade = await commandRepository.ObterPorIdAsync(faturaId, cancellationToken)
            ?? throw new RecursoNaoEncontradoException($"Fatura com Id {faturaId} não encontrada.");

        entidade.Fechar();

        await commandRepository.SalvarAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task PagarAsync(int faturaId, FaturaPagamentoDto dto, CancellationToken cancellationToken)
    {
        logger.LogInformation("Pagando fatura {FaturaId}.", faturaId);

        var entidade = await commandRepository.ObterPorIdAsync(faturaId, cancellationToken)
            ?? throw new RecursoNaoEncontradoException($"Fatura com Id {faturaId} não encontrada.");

        entidade.Pagar(dto.DataPagamento);

        var valor = await lancamentoQueryRepository
            .ObterValorTotalPorFaturaAsync(faturaId, cancellationToken);

        var movimentacao = Movimentacao.Criar(
            dto.DataPagamento,
            new Dinheiro(valor),
            Natureza.Saida,
            TipoMovimentacao.Debito,
            "Pagamento fatura cartão",
            dto.ContaId,
            dto.CategoriaId);

        movimentacaoCommandRepository.Adicionar(movimentacao);

        await commandRepository.SalvarAsync(cancellationToken);
    }
}

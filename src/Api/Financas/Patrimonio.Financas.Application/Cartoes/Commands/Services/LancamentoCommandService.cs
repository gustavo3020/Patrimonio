using Microsoft.Extensions.Logging;
using Patrimonio.Financas.Application.Cartoes.Commands.Abstractions;
using Patrimonio.Financas.Application.Cartoes.Lookups.Abstractions;
using Patrimonio.Financas.Application.Cartoes.Lookups.ReadModels;
using Patrimonio.Financas.Application.Cartoes.Mappers;
using Patrimonio.Financas.Application.Cartoes.Queries.Abstractions;
using Patrimonio.Financas.Application.Cartoes.Queries.ReadModels;
using Patrimonio.Financas.Contracts.Cartoes.Dtos;
using Patrimonio.Financas.Contracts.Cartoes.Services;
using Patrimonio.Financas.Domain.Cartoes.Entities;
using Patrimonio.Financas.Domain.Cartoes.Services;
using Patrimonio.Financas.Domain.Cartoes.ValueObjects;
using Patrimonio.Financas.Domain.Common.ValueObjects;
using Patrimonio.Financas.Domain.Exceptions;
using Patrimonio.Financas.SharedKernel.Cartoes.Enums;

namespace Patrimonio.Financas.Application.Cartoes.Commands.Services;

/// <summary>
/// Implementa as operações de escrita disponíveis para lancamentos.
/// </summary>
internal sealed class LancamentoCommandService(
    ILancamentoCommandRepository commandRepository,
    ILancamentoQueryRepository queryRepository,
    IFaturaLookupRepository faturaLookupRepository,
    IFaturaQueryRepository faturaQueryRepository,
    IFaturaCommandService faturaCommandService,
    ILogger<LancamentoCommandService> logger) : ILancamentoCommandService
{
    /// <inheritdoc />
    public async Task<LancamentoDetalheDto> CriarAsync(LancamentoCriacaoDto dto, CancellationToken cancellationToken)
    {
        logger.LogInformation("Criando lançamentos {Descricao}.", dto.Descricao);

        var primeiraFatura = await ValidarFaturaInicial(dto.FaturaId, cancellationToken);
        var faturas = await faturaLookupRepository.ListarPorCartaoAsync(primeiraFatura.CartaoId, cancellationToken);
        var parcelas = ParcelamentoService.CalcularParcelas(dto.Valor, dto.TotalParcelas);

        Lancamento? primeiraParcela = null;
        var grupoId = Guid.NewGuid();

        for (int numeroParcela = 1; numeroParcela <= dto.TotalParcelas; numeroParcela++)
        {
            var fatura = await ObterOuCriarFatura(faturas, primeiraFatura, numeroParcela, cancellationToken);
            var entidade = CriarLancamento(dto, parcelas[numeroParcela - 1], numeroParcela, fatura.Id, grupoId);

            commandRepository.Adicionar(entidade);

            if (numeroParcela == 1)
                primeiraParcela = entidade;
        }

        await commandRepository.SalvarAsync(cancellationToken);

        var detalhe = await queryRepository.ObterPorIdAsync(primeiraParcela!.Id, cancellationToken);
        return LancamentoMapper.Mapear(detalhe!);
    }

    /// <inheritdoc />
    public async Task AlterarAsync(int lancamentoId, LancamentoAlteracaoDto dto, CancellationToken cancellationToken)
    {
        logger.LogInformation("Alterando lançamento {LancamentoId}.", lancamentoId);

        var entidade = await commandRepository.ObterPorIdAsync(lancamentoId, cancellationToken)
            ?? throw new RecursoNaoEncontradoException($"Lançamento com Id {lancamentoId} não encontrado.");

        var fatura = await faturaQueryRepository.ObterPorIdAsync(entidade.FaturaId, cancellationToken)
            ?? throw new RecursoNaoEncontradoException($"Fatura com Id {entidade.FaturaId} não encontrada.");

        if (fatura.Status != StatusFatura.Aberta)
            throw new RegraDeNegocioException($"Só é possível alterar um lançamento associado a uma fatura que esteja aberta.");

        entidade.Alterar(
            new Descricao(dto.Descricao),
            new Dinheiro(dto.Valor),
            dto.DataCompra,
            new Estabelecimento(dto.Estabelecimento),
            new Responsavel(dto.Responsavel),
            dto.Natureza,
            dto.CategoriaId
        );

        await commandRepository.SalvarAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task ExcluirAsync(int lancamentoId, CancellationToken cancellationToken)
    {
        logger.LogInformation("Excluindo lançamento {LancamentoId}.", lancamentoId);

        var entidade = await commandRepository.ObterPorIdAsync(lancamentoId, cancellationToken)
            ?? throw new RecursoNaoEncontradoException($"Lançamento com Id {lancamentoId} não encontrado.");

        var fatura = await faturaQueryRepository.ObterPorIdAsync(entidade.FaturaId, cancellationToken)
            ?? throw new RecursoNaoEncontradoException($"Fatura com Id {entidade.FaturaId} não encontrada.");

        if (fatura.Status != StatusFatura.Aberta)
            throw new RegraDeNegocioException($"Só é possível excluir um lançamento associado a uma fatura que esteja aberta.");

        commandRepository.Remover(entidade);

        await commandRepository.SalvarAsync(cancellationToken);
    }

    // ============================
    // Métodos privados auxiliares
    // ============================

    private async Task<FaturaDetalheReadModel> ValidarFaturaInicial(int faturaId, CancellationToken cancellationToken)
    {
        var fatura = await faturaQueryRepository.ObterPorIdAsync(faturaId, cancellationToken)
            ?? throw new RecursoNaoEncontradoException($"Fatura com Id {faturaId} não encontrada.");

        if (fatura.Status != StatusFatura.Aberta)
            throw new RegraDeNegocioException("Só é possível criar um lançamento em fatura aberta.");

        return fatura;
    }

    private async Task<FaturaOpcaoReadModel> ObterOuCriarFatura(
        IEnumerable<FaturaOpcaoReadModel> faturas,
        FaturaDetalheReadModel primeiraFatura,
        int numeroParcela,
        CancellationToken cancellationToken)
    {
        var dataFechamento = primeiraFatura.DataFechamento.AddMonths(numeroParcela - 1);
        var dataVencimento = primeiraFatura.DataVencimento.AddMonths(numeroParcela - 1);

        var fatura = faturas.FirstOrDefault(f => f.DataVencimento == dataVencimento);

        if (fatura == null)
        {
            var criarFaturaDto = new FaturaCriacaoDto
            {
                DataFechamento = dataFechamento,
                DataVencimento = dataVencimento,
                CartaoId = primeiraFatura.CartaoId
            };

            var faturaDetalhe = await faturaCommandService.CriarAsync(criarFaturaDto, cancellationToken);
            fatura = new FaturaOpcaoReadModel
            {
                Id = faturaDetalhe.Id,
                DataVencimento = faturaDetalhe.DataVencimento
            };
        }

        return fatura;
    }

    private static Lancamento CriarLancamento(
        LancamentoCriacaoDto dto,
        decimal valorParcela,
        int numeroParcela,
        int faturaId,
        Guid grupoId)
    {
        return Lancamento.Criar(
            new Descricao(dto.Descricao),
            new Dinheiro(valorParcela),
            dto.DataCompra,
            new Estabelecimento(dto.Estabelecimento),
            new Responsavel(dto.Responsavel),
            new Parcelamento(grupoId, numeroParcela, dto.TotalParcelas),
            dto.Natureza,
            faturaId,
            dto.CategoriaId
        );
    }
}

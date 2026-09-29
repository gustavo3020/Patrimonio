using Microsoft.Extensions.Logging;
using Patrimonio.Financas.Application.Categorias.Commands.Abstractions;
using Patrimonio.Financas.Application.Categorias.Mappers;
using Patrimonio.Financas.Application.Categorias.Queries.Abstractions;
using Patrimonio.Financas.Contracts.Categorias.Dtos;
using Patrimonio.Financas.Contracts.Categorias.Services;
using Patrimonio.Financas.Domain.Categorias.Entities;
using Patrimonio.Financas.Domain.Common.ValueObjects;
using Patrimonio.Financas.Domain.Exceptions;

namespace Patrimonio.Financas.Application.Categorias.Commands.Services;

/// <summary>
/// Implementa as operações de escrita disponíveis para categorias.
/// </summary>
internal sealed class CategoriaCommandService(
    ICategoriaCommandRepository commandRepository,
    ICategoriaQueryRepository queryRepository,
    ILogger<CategoriaCommandService> logger) : ICategoriaCommandService
{
    /// <inheritdoc />
    public async Task<CategoriaDetalheDto> CriarAsync(CategoriaCriacaoDto dto, CancellationToken cancellationToken)
    {
        logger.LogInformation("Criando categoria {Nome}.", dto.Nome);

        var entidade = Categoria.Criar(new Nome(dto.Nome));

        commandRepository.Adicionar(entidade);

        await commandRepository.SalvarAsync(cancellationToken);

        var entidadeDetalhe = await queryRepository.ObterPorIdAsync(entidade.Id, cancellationToken);

        return CategoriaMapper.Mapear(entidadeDetalhe!);
    }

    /// <inheritdoc />
    public async Task AlterarAsync(int categoriaId, CategoriaAlteracaoDto dto, CancellationToken cancellationToken)
    {
        logger.LogInformation("Alterando categoria {CategoriaId}.", categoriaId);

        var entidade = await commandRepository.ObterPorIdAsync(categoriaId, cancellationToken)
            ?? throw new RecursoNaoEncontradoException($"Categoria com Id {categoriaId} não encontrada.");

        entidade.AlterarNome(new Nome(dto.Nome));

        await commandRepository.SalvarAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task ExcluirAsync(int categoriaId, CancellationToken cancellationToken)
    {
        logger.LogInformation("Excluindo categoria {CategoriaId}.", categoriaId);

        var entidade = await commandRepository.ObterPorIdAsync(categoriaId, cancellationToken)
            ?? throw new RecursoNaoEncontradoException($"Categoria com Id {categoriaId} não encontrada.");

        commandRepository.Remover(entidade);

        await commandRepository.SalvarAsync(cancellationToken);
    }
}

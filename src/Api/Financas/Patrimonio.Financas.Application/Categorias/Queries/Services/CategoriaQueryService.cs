using Microsoft.Extensions.Logging;
using Patrimonio.Financas.Application.Categorias.Mappers;
using Patrimonio.Financas.Application.Categorias.Queries.Abstractions;
using Patrimonio.Financas.Contracts.Categorias.Dtos;
using Patrimonio.Financas.Contracts.Categorias.Services;
using Patrimonio.Financas.Domain.Exceptions;

namespace Patrimonio.Financas.Application.Categorias.Queries.Services;

/// <summary>
/// Implementa as operações de leitura disponíveis para categorias.
/// </summary>
internal sealed class CategoriaQueryService(
    ICategoriaQueryRepository queryRepository,
    ILogger<CategoriaQueryService> logger) : ICategoriaQueryService
{
    /// <inheritdoc />
    public async Task<IReadOnlyCollection<CategoriaListaDto>> ListarAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Listando categorias.");

        var categorias = await queryRepository.ListarAsync(cancellationToken);

        return [.. categorias.Select(CategoriaMapper.Mapear)];
    }

    /// <inheritdoc />
    public async Task<CategoriaDetalheDto> ObterPorIdAsync(int categoriaId, CancellationToken cancellationToken)
    {
        logger.LogInformation("Obtendo categoria por ID {CategoriaId}.", categoriaId);

        var categoria = await queryRepository.ObterPorIdAsync(categoriaId, cancellationToken)
            ?? throw new RecursoNaoEncontradoException($"Categoria com Id {categoriaId} não encontrada.");

        return CategoriaMapper.Mapear(categoria);
    }
}

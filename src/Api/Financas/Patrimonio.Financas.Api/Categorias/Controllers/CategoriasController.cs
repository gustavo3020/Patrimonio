using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Patrimonio.Financas.Contracts.Categorias.Dtos;
using Patrimonio.Financas.Contracts.Categorias.Services;

namespace Patrimonio.Financas.Api.Categorias.Controllers;

/// <summary>
/// Disponibiliza as operações de gerenciamento de categorias.
/// </summary>
[ApiController]
[Route("api/v1/financas/categorias")]
public sealed class CategoriasController(
    ICategoriaCommandService commandService,
    ICategoriaQueryService queryService) : ControllerBase
{
    /// <summary>
    /// Lista todas as categorias cadastradas.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Listar(CancellationToken cancellationToken)
    {
        var categorias = await queryService.ListarAsync(cancellationToken);
        return Ok(categorias);
    }

    /// <summary>
    /// Obtém uma categoria específica pelo seu identificador.
    /// </summary>
    [HttpGet("{categoriaId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(int categoriaId, CancellationToken cancellationToken)
    {
        var categoria = await queryService.ObterPorIdAsync(categoriaId, cancellationToken);
        return Ok(categoria);
    }

    /// <summary>
    /// Cria uma nova categoria.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Criar(CategoriaCriacaoDto dto, CancellationToken cancellationToken)
    {
        var categoria = await commandService.CriarAsync(dto, cancellationToken);

        return CreatedAtAction(
            nameof(ObterPorId),
            new { categoriaId = categoria.Id },
            categoria
        );
    }

    /// <summary>
    /// Altera uma categoria existente.
    /// </summary>
    [HttpPut("{categoriaId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Alterar(int categoriaId, CategoriaAlteracaoDto dto, CancellationToken cancellationToken)
    {
        await commandService.AlterarAsync(categoriaId, dto, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Exclui uma categoria existente.
    /// </summary>
    [HttpDelete("{categoriaId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Excluir(int categoriaId, CancellationToken cancellationToken)
    {
        await commandService.ExcluirAsync(categoriaId, cancellationToken);
        return NoContent();
    }
}

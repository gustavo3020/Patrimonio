using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Patrimonio.Financas.Contracts.Cartoes.Dtos;
using Patrimonio.Financas.Contracts.Cartoes.Services;

namespace Patrimonio.Financas.Api.Cartoes.Controllers;

/// <summary>
/// Disponibiliza as operações de gerenciamento de cartões.
/// </summary>
[ApiController]
[Route("api/v1/financas/cartoes")]
public sealed class CartaoController(
    ICartaoCommandService commandService,
    ICartaoQueryService queryService) : ControllerBase
{
    /// <summary>
    /// Lista todos os cartões cadastrados.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Listar(CancellationToken cancellationToken)
    {
        var cartoes = await queryService.ListarAsync(cancellationToken);
        return Ok(cartoes);
    }

    /// <summary>
    /// Obtém um cartão específico pelo seu identificador.
    /// </summary>
    [HttpGet("{cartaoId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(int cartaoId, CancellationToken cancellationToken)
    {
        var cartao = await queryService.ObterPorIdAsync(cartaoId, cancellationToken);
        return Ok(cartao);
    }

    /// <summary>
    /// Cria um novo cartão.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Criar(CartaoCriacaoDto dto, CancellationToken cancellationToken)
    {
        var cartao = await commandService.CriarAsync(dto, cancellationToken);

        return CreatedAtAction(
            nameof(ObterPorId),
            new { cartaoId = cartao.Id },
            cartao
        );
    }

    /// <summary>
    /// Altera um cartão existente.
    /// </summary>
    [HttpPut("{cartaoId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Alterar(int cartaoId, CartaoAlteracaoDto dto, CancellationToken cancellationToken)
    {
        await commandService.AlterarAsync(cartaoId, dto, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Exclui um cartão existente.
    /// </summary>
    [HttpDelete("{cartaoId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Excluir(int cartaoId, CancellationToken cancellationToken)
    {
        await commandService.ExcluirAsync(cartaoId, cancellationToken);
        return NoContent();
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Patrimonio.Financas.Contracts.Contas.Dtos;
using Patrimonio.Financas.Contracts.Contas.Services;

namespace Patrimonio.Financas.Api.Contas.Controllers;

/// <summary>
/// Disponibiliza as operações de gerenciamento de contas.
/// </summary>
[ApiController]
[Route("api/v1/financas/contas")]
public sealed class ContasController(
    IContaCommandService commandService,
    IContaQueryService queryService) : ControllerBase
{
    /// <summary>
    /// Lista todas as contas cadastradas.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Listar(CancellationToken cancellationToken)
    {
        var contas = await queryService.ListarAsync(cancellationToken);
        return Ok(contas);
    }

    /// <summary>
    /// Obtém uma conta específica pelo seu identificador.
    /// </summary>
    [HttpGet("{contaId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(int contaId, CancellationToken cancellationToken)
    {
        var conta = await queryService.ObterPorIdAsync(contaId, cancellationToken);
        return Ok(conta);
    }

    /// <summary>
    /// Cria uma nova conta.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Criar(ContaCriacaoDto dto, CancellationToken cancellationToken)
    {
        var conta = await commandService.CriarAsync(dto, cancellationToken);

        return CreatedAtAction(
            nameof(ObterPorId),
            new { contaId = conta.Id },
            conta
        );
    }

    /// <summary>
    /// Altera uma conta existente.
    /// </summary>
    [HttpPut("{contaId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Alterar(int contaId, ContaAlteracaoDto dto, CancellationToken cancellationToken)
    {
        await commandService.AlterarAsync(contaId, dto, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Exclui uma conta existente.
    /// </summary>
    [HttpDelete("{contaId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Excluir(int contaId, CancellationToken cancellationToken)
    {
        await commandService.ExcluirAsync(contaId, cancellationToken);
        return NoContent();
    }
}

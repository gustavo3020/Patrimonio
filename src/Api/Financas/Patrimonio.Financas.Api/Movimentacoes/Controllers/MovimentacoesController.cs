using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Patrimonio.Financas.Contracts.Movimentacoes.Dtos;
using Patrimonio.Financas.Contracts.Movimentacoes.Services;

namespace Patrimonio.Financas.Api.Movimentacoes.Controllers;

/// <summary>
/// Disponibiliza as operações de gerenciamento de movimentações.
/// </summary>
[ApiController]
[Route("api/v1/financas/movimentacoes")]
public sealed class MovimentacoesController(
    IMovimentacaoCommandService commandService,
    IMovimentacaoQueryService queryService) : ControllerBase
{
    /// <summary>
    /// Lista todas as movimentações cadastradas.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Listar(CancellationToken cancellationToken)
    {
        var movimentacoes = await queryService.ListarAsync(cancellationToken);
        return Ok(movimentacoes);
    }

    /// <summary>
    /// Obtém uma movimentação específica pelo seu identificador.
    /// </summary>
    [HttpGet("{movimentacaoId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(int movimentacaoId, CancellationToken cancellationToken)
    {
        var movimentacao = await queryService.ObterPorIdAsync(movimentacaoId, cancellationToken);
        return Ok(movimentacao);
    }

    /// <summary>
    /// Cria uma nova movimentação.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Criar(MovimentacaoCriacaoDto dto, CancellationToken cancellationToken)
    {
        var movimentacao = await commandService.CriarAsync(dto, cancellationToken);

        return CreatedAtAction(
            nameof(ObterPorId),
            new { movimentacaoId = movimentacao.Id },
            movimentacao
        );
    }

    /// <summary>
    /// Altera uma movimentação existente.
    /// </summary>
    [HttpPut("{movimentacaoId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Alterar(int movimentacaoId, MovimentacaoAlteracaoDto dto, CancellationToken cancellationToken)
    {
        await commandService.AlterarAsync(movimentacaoId, dto, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Exclui uma movimentação existente.
    /// </summary>
    [HttpDelete("{movimentacaoId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Excluir(int movimentacaoId, CancellationToken cancellationToken)
    {
        await commandService.ExcluirAsync(movimentacaoId, cancellationToken);
        return NoContent();
    }
}

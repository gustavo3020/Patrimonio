using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Patrimonio.Financas.Contracts.Cartoes.Dtos;
using Patrimonio.Financas.Contracts.Cartoes.Services;

namespace Patrimonio.Financas.Api.Cartoes.Controllers;

/// <summary>
/// Disponibiliza as operações de gerenciamento de lançamentos.
/// </summary>
[ApiController]
[Route("api/v1/financas/lancamentos")]
public sealed class LancamentoController(
    ILancamentoCommandService commandService,
    ILancamentoQueryService queryService) : ControllerBase
{
    /// <summary>
    /// Lista todos os lançamentos cadastrados.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Listar(CancellationToken cancellationToken)
    {
        var lancamentos = await queryService.ListarAsync(cancellationToken);
        return Ok(lancamentos);
    }

    /// <summary>
    /// Obtém um lançamento específico pelo seu identificador.
    /// </summary>
    [HttpGet("{lancamentoId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(int lancamentoId, CancellationToken cancellationToken)
    {
        var lancamento = await queryService.ObterPorIdAsync(lancamentoId, cancellationToken);
        return Ok(lancamento);
    }

    /// <summary>
    /// Cria um novo lançamento.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Criar(LancamentoCriacaoDto dto, CancellationToken cancellationToken)
    {
        var lancamento = await commandService.CriarAsync(dto, cancellationToken);

        return CreatedAtAction(
            nameof(ObterPorId),
            new { lancamentoId = lancamento.Id },
            lancamento
        );
    }

    /// <summary>
    /// Altera um lançamento existente.
    /// </summary>
    [HttpPut("{lancamentoId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Alterar(int lancamentoId, LancamentoAlteracaoDto dto, CancellationToken cancellationToken)
    {
        await commandService.AlterarAsync(lancamentoId, dto, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Exclui um lançamento existente.
    /// </summary>
    [HttpDelete("{lancamentoId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Excluir(int lancamentoId, CancellationToken cancellationToken)
    {
        await commandService.ExcluirAsync(lancamentoId, cancellationToken);
        return NoContent();
    }
}

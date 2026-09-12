using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Patrimonio.Financas.Contracts.Cartoes.Dtos;
using Patrimonio.Financas.Contracts.Cartoes.Services;

namespace Patrimonio.Financas.Api.Cartoes.Controllers;

/// <summary>
/// Disponibiliza as operações de gerenciamento de faturas.
/// </summary>
[ApiController]
[Route("api/v1/financas/faturas")]
public sealed class FaturaController(
    IFaturaCommandService commandService,
    IFaturaQueryService queryService) : ControllerBase
{
    /// <summary>
    /// Lista todas as faturas cadastradas.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Listar(CancellationToken cancellationToken)
    {
        var faturas = await queryService.ListarAsync(cancellationToken);
        return Ok(faturas);
    }

    /// <summary>
    /// Obtém uma fatura específica pelo seu identificador.
    /// </summary>
    [HttpGet("{faturaId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(int faturaId, CancellationToken cancellationToken)
    {
        var fatura = await queryService.ObterPorIdAsync(faturaId, cancellationToken);
        return Ok(fatura);
    }

    /// <summary>
    /// Cria uma nova fatura.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Criar(FaturaCriacaoDto dto, CancellationToken cancellationToken)
    {
        var fatura = await commandService.CriarAsync(dto, cancellationToken);

        return CreatedAtAction(
            nameof(ObterPorId),
            new { faturaId = fatura.Id },
            fatura
        );
    }

    /// <summary>
    /// Altera uma fatura existente.
    /// </summary>
    [HttpPut("{faturaId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Alterar(int faturaId, FaturaAlteracaoDto dto, CancellationToken cancellationToken)
    {
        await commandService.AlterarAsync(faturaId, dto, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Exclui uma fatura existente.
    /// </summary>
    [HttpDelete("{faturaId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Excluir(int faturaId, CancellationToken cancellationToken)
    {
        await commandService.ExcluirAsync(faturaId, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Fecha uma fatura existente.
    /// </summary>
    [HttpPut("{faturaId:int}/fechar")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Fechar(int faturaId, CancellationToken cancellationToken)
    {
        await commandService.FecharAsync(faturaId, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Paga uma fatura existente.
    /// </summary>
    [HttpPut("{faturaId:int}/pagar")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Pagar(int faturaId, [FromBody] FaturaPagamentoDto dto, CancellationToken cancellationToken)
    {
        await commandService.PagarAsync(faturaId, dto, cancellationToken);
        return NoContent();
    }
}

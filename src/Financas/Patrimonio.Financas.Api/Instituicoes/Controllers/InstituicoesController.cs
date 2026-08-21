using Microsoft.AspNetCore.Mvc;
using Patrimonio.Financas.Contracts.Instituicoes.Dtos;
using Patrimonio.Financas.Contracts.Instituicoes.Services;

namespace Patrimonio.Financas.Api.Instituicoes.Controllers;

/// <summary>
/// Disponibiliza as operações de gerenciamento de instituições.
/// </summary>
[ApiController]
[Route("api/v1/financas/instituicoes")]
public sealed class InstituicoesController(
    IInstituicaoCommandService commandService,
    IInstituicaoQueryService queryService) : ControllerBase
{
    /// <summary>
    /// Lista todas as instituições cadastradas.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Listar(CancellationToken cancellationToken)
    {
        var instituicoes = await queryService.ListarAsync(cancellationToken);
        return Ok(instituicoes);
    }

    /// <summary>
    /// Obtém uma instituição específica pelo seu identificador.
    /// </summary>
    [HttpGet("{instituicaoId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(int instituicaoId, CancellationToken cancellationToken)
    {
        var instituicao = await queryService.ObterPorIdAsync(instituicaoId, cancellationToken);
        return Ok(instituicao);
    }

    /// <summary>
    /// Cria uma nova instituição.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Criar(InstituicaoCriacaoDto dto, CancellationToken cancellationToken)
    {
        var instituicao = await commandService.CriarAsync(dto, cancellationToken);

        return CreatedAtAction(
            nameof(ObterPorId),
            new { instituicaoId = instituicao.Id },
            instituicao
        );
    }

    /// <summary>
    /// Altera uma instituição existente.
    /// </summary>
    [HttpPut("{instituicaoId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Alterar(int instituicaoId, InstituicaoAlteracaoDto dto, CancellationToken cancellationToken)
    {
        await commandService.AlterarAsync(instituicaoId, dto, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Exclui uma instituição existente.
    /// </summary>
    [HttpDelete("{instituicaoId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Excluir(int instituicaoId, CancellationToken cancellationToken)
    {
        await commandService.ExcluirAsync(instituicaoId, cancellationToken);
        return NoContent();
    }
}

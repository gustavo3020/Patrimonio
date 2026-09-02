using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Patrimonio.Financas.Contracts.Movimentacoes.Services;

namespace Patrimonio.Financas.Api.Movimentacoes.Controllers;

/// <summary>
/// Disponibiliza as opções de referência necessárias para a criação de uma movimentação.
/// </summary>
[ApiController]
[Route("api/v1/financas/movimentacoes/opcoes-criacao")]
public sealed class MovimentacaoOpcoesCriacaoController(
    IMovimentacaoOpcoesCriacaoService opcoesCriacaoService) : ControllerBase
{
    /// <summary>
    /// Obtém as opções de referência necessárias para a criação de uma movimentação.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ObterOpcoesCriacao(CancellationToken cancellationToken)
    {
        var opcoes = await opcoesCriacaoService.ObterOpcoesCriacaoAsync(cancellationToken);
        return Ok(opcoes);
    }
}

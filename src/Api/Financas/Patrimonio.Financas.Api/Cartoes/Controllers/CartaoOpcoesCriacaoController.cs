using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Patrimonio.Financas.Contracts.Cartoes.Services;

namespace Patrimonio.Financas.Api.Cartoes.Controllers;

/// <summary>
/// Disponibiliza as opções de referência necessárias para a criação de um cartão.
/// </summary>
[ApiController]
[Route("api/v1/financas/cartoes/opcoes-criacao")]
public sealed class CartaoOpcoesCriacaoController(
    ICartaoOpcoesCriacaoService opcoesCriacaoService) : ControllerBase
{
    /// <summary>
    /// Obtém as opções de referência necessárias para a criação de um cartão.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ObterOpcoesCriacao(CancellationToken cancellationToken)
    {
        var opcoes = await opcoesCriacaoService.ObterOpcoesCriacaoAsync(cancellationToken);
        return Ok(opcoes);
    }
}

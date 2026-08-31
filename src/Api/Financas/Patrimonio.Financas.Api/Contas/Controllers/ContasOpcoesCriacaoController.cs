using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Patrimonio.Financas.Contracts.Contas.Services;

namespace Patrimonio.Financas.Api.Contas.Controllers;

/// <summary>
/// Disponibiliza as opções de referência necessárias para a criação de uma conta.
/// </summary>
[ApiController]
[Route("api/v1/financas/contas/opcoes-criacao")]
public sealed class ContasOpcoesCriacaoController(
    IContaOpcoesCriacaoService opcoesCriacaoService) : ControllerBase
{
    /// <summary>
    /// Obtém as opções de referência necessárias para a criação de uma conta.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ObterOpcoesCriacao(CancellationToken cancellationToken)
    {
        var opcoes = await opcoesCriacaoService.ObterOpcoesCriacaoAsync(cancellationToken);
        return Ok(opcoes);
    }
}

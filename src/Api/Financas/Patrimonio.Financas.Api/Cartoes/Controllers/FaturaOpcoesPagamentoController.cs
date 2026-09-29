using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Patrimonio.Financas.Contracts.Cartoes.Services;

namespace Patrimonio.Financas.Api.Cartoes.Controllers;

/// <summary>
/// Disponibiliza as opções de referência necessárias para o pagamento de uma fatura.
/// </summary>
[ApiController]
[Route("api/v1/financas/faturas/opcoes-pagamento")]
public sealed class FaturaOpcoesPagamentoController(
    IFaturaOpcoesPagamentoService opcoesPagamentoService) : ControllerBase
{
    /// <summary>
    /// Obtém as opções de referência necessárias para o pagamento de uma fatura.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ObterOpcoesPagamento(CancellationToken cancellationToken)
    {
        var opcoes = await opcoesPagamentoService.ObterOpcoesPagamentoAsync(cancellationToken);
        return Ok(opcoes);
    }
}

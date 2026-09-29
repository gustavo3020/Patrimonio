using Patrimonio.Financas.Contracts.Movimentacoes.Dtos;
using Patrimonio.Financas.Contracts.Movimentacoes.Services;
using Patrimonio.Financas.Wpf.Common.Http;
using System.Net.Http;
using System.Net.Http.Json;

namespace Patrimonio.Financas.Wpf.Movimentacoes.HttpClients;

/// <summary>
/// Implementa as operações HTTP de leitura e escrita de movimentações,
/// utilizando os contratos públicos disponibilizados pelo módulo.
/// </summary>
internal sealed class MovimentacaoHttpClient(HttpClient httpClient)
    : HttpClientBase<MovimentacaoListaDto, MovimentacaoDetalheDto, MovimentacaoCriacaoDto, MovimentacaoAlteracaoDto>(
        httpClient, MovimentacaoEndpoints.Base),
    IMovimentacaoCommandService, IMovimentacaoOpcoesCriacaoService, IMovimentacaoQueryService
{
    /// <summary>
    /// Obtém as opções de referência necessárias para a criação de uma movimentação.
    /// </summary>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    /// <returns>A coleção de recursos retornada pela API, ou uma coleção vazia caso não haja nenhum.</returns>
    public async Task<MovimentacaoOpcoesCriacaoDto> ObterOpcoesCriacaoAsync(CancellationToken cancellationToken)
    {
        return await HttpClient.GetFromJsonAsync<MovimentacaoOpcoesCriacaoDto>(
            MovimentacaoEndpoints.OpcoesCriacao(),
            cancellationToken)
            ?? new MovimentacaoOpcoesCriacaoDto
            {
                Categorias = [],
                Contas = []
            };
    }
}

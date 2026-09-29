using Patrimonio.Financas.Contracts.Cartoes.Dtos;
using Patrimonio.Financas.Contracts.Cartoes.Services;
using Patrimonio.Financas.Wpf.Common.Http;
using System.Net.Http;
using System.Net.Http.Json;

namespace Patrimonio.Financas.Wpf.Cartoes.HttpClients;

/// <summary>
/// Implementa as operações HTTP de leitura e escrita de cartões,
/// utilizando os contratos públicos disponibilizados pelo módulo.
/// </summary>
internal sealed class CartaoHttpClient(HttpClient httpClient)
    : HttpClientBase<CartaoListaDto, CartaoDetalheDto, CartaoCriacaoDto, CartaoAlteracaoDto>(
        httpClient, CartaoEndpoints.Base), ICartaoCommandService, ICartaoOpcoesCriacaoService, ICartaoQueryService
{
    /// <summary>
    /// Obtém as opções de referência necessárias para a criação de um cartão.
    /// </summary>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    /// <returns>A coleção de recursos retornada pela API, ou uma coleção vazia caso não haja nenhum.</returns>
    public async Task<CartaoOpcoesCriacaoDto> ObterOpcoesCriacaoAsync(CancellationToken cancellationToken)
    {
        return await HttpClient.GetFromJsonAsync<CartaoOpcoesCriacaoDto>(
            CartaoEndpoints.OpcoesCriacao(),
            cancellationToken)
            ?? new CartaoOpcoesCriacaoDto
            {
                Instituicoes = []
            };
    }
}

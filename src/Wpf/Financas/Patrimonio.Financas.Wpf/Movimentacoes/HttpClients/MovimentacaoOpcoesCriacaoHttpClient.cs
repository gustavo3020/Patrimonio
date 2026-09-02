using Patrimonio.Financas.Contracts.Movimentacoes.Dtos;
using Patrimonio.Financas.Contracts.Movimentacoes.Services;
using System.Net.Http;
using System.Net.Http.Json;

namespace Patrimonio.Financas.Wpf.Movimentacoes.HttpClients;

internal sealed class MovimentacaoOpcoesCriacaoHttpClient(HttpClient httpClient) : IMovimentacaoOpcoesCriacaoService
{
    /// <summary>
    /// Obtém as opções de referência necessárias para a criação de uma movimentação.
    /// </summary>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    /// <returns>A coleção de recursos retornada pela API, ou uma coleção vazia caso não haja nenhum.</returns>
    public async Task<MovimentacaoOpcoesCriacaoDto> ObterOpcoesCriacaoAsync(CancellationToken cancellationToken)
    {
        return await httpClient.GetFromJsonAsync<MovimentacaoOpcoesCriacaoDto>(
            MovimentacaoEndpoints.OpcoesCriacao(),
            cancellationToken)
            ?? new MovimentacaoOpcoesCriacaoDto
            {
                Categorias = [],
                Contas = []
            };
    }
}

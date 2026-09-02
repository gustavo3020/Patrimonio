using Patrimonio.Financas.Contracts.Contas.Dtos;
using Patrimonio.Financas.Contracts.Contas.Services;
using System.Net.Http;
using System.Net.Http.Json;

namespace Patrimonio.Financas.Wpf.Contas.HttpClients;

internal sealed class ContaOpcoesCriacaoHttpClient(HttpClient httpClient) : IContaOpcoesCriacaoService
{
    /// <summary>
    /// Obtém as opções de referência necessárias para a criação de uma conta.
    /// </summary>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    /// <returns>A coleção de recursos retornada pela API, ou uma coleção vazia caso não haja nenhum.</returns>
    public async Task<ContaOpcoesCriacaoDto> ObterOpcoesCriacaoAsync(CancellationToken cancellationToken)
    {
        return await httpClient.GetFromJsonAsync<ContaOpcoesCriacaoDto>(
            ContaEndpoints.OpcoesCriacao(),
            cancellationToken)
            ?? new ContaOpcoesCriacaoDto
            {
                Instituicoes = []
            };
    }
}

using Patrimonio.Financas.Contracts.Contas.Dtos;
using Patrimonio.Financas.Contracts.Contas.Services;
using Patrimonio.Financas.Wpf.Common.Http;
using System.Net.Http;
using System.Net.Http.Json;

namespace Patrimonio.Financas.Wpf.Contas.HttpClients;

/// <summary>
/// Implementa as operações HTTP de leitura e escrita de contas,
/// utilizando os contratos públicos disponibilizados pelo módulo.
/// </summary>
internal sealed class ContaHttpClient(HttpClient httpClient)
    : HttpClientBase<ContaListaDto, ContaDetalheDto, ContaCriacaoDto, ContaAlteracaoDto>(
        httpClient, ContaEndpoints.Base), IContaCommandService, IContaOpcoesCriacaoService, IContaQueryService
{
    /// <summary>
    /// Obtém as opções de referência necessárias para a criação de uma conta.
    /// </summary>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    /// <returns>A coleção de recursos retornada pela API, ou uma coleção vazia caso não haja nenhum.</returns>
    public async Task<ContaOpcoesCriacaoDto> ObterOpcoesCriacaoAsync(CancellationToken cancellationToken)
    {
        return await HttpClient.GetFromJsonAsync<ContaOpcoesCriacaoDto>(
            ContaEndpoints.OpcoesCriacao(),
            cancellationToken)
            ?? new ContaOpcoesCriacaoDto
            {
                Instituicoes = []
            };
    }
}

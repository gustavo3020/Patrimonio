using Patrimonio.Financas.Contracts.Cartoes.Dtos;
using Patrimonio.Financas.Contracts.Cartoes.Services;
using Patrimonio.Financas.Wpf.Common.Http;
using System.Net.Http;
using System.Net.Http.Json;

namespace Patrimonio.Financas.Wpf.Cartoes.HttpClients;

/// <summary>
/// Implementa as operações HTTP de leitura e escrita de faturas,
/// utilizando os contratos públicos disponibilizados pelo módulo.
/// </summary>
internal sealed class FaturaHttpClient(HttpClient httpClient)
    : HttpClientBase<FaturaListaDto, FaturaDetalheDto, FaturaCriacaoDto, FaturaAlteracaoDto>(
        httpClient, FaturaEndpoints.Base), IFaturaCommandService, IFaturaOpcoesPagamentoService, IFaturaQueryService
{
    /// <inheritdoc/>
    public async Task<IReadOnlyCollection<FaturaListaDto>> ListarAsync(int cartaoId, CancellationToken cancellationToken)
    {
        return await HttpClient.GetFromJsonAsync<IReadOnlyCollection<FaturaListaDto>>(
            FaturaEndpoints.Listar(cartaoId),
            cancellationToken
        ) ?? [];
    }

    /// <inheritdoc/>
    public Task FecharAsync(int faturaId, CancellationToken cancellationToken)
    {
        return HttpClient.PutAsync(
            $"{FaturaEndpoints.Base}/{faturaId}/fechar",
            null,
            cancellationToken);
    }

    /// <inheritdoc/>
    public Task PagarAsync(int faturaId, FaturaPagamentoDto dto, CancellationToken cancellationToken)
    {
        return HttpClient.PutAsJsonAsync(
            $"{FaturaEndpoints.Base}/{faturaId}/pagar",
            dto,
            cancellationToken);
    }

    /// <summary>
    /// Obtém as opções de referência necessárias para o pagamento de uma fatura.
    /// </summary>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    /// <returns>A coleção de recursos retornada pela API, ou uma coleção vazia caso não haja nenhum.</returns>
    public async Task<FaturaOpcoesPagamentoDto> ObterOpcoesPagamentoAsync(CancellationToken cancellationToken)
    {
        return await HttpClient.GetFromJsonAsync<FaturaOpcoesPagamentoDto>(
            FaturaEndpoints.OpcoesPagamento(),
            cancellationToken)
            ?? new FaturaOpcoesPagamentoDto
            {
                Categorias = [],
                Contas = [],
            };
    }
}

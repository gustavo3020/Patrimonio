using Patrimonio.Financas.Contracts.Cartoes.Dtos;
using Patrimonio.Financas.Contracts.Cartoes.Services;
using Patrimonio.Financas.Wpf.Common.Http;
using System.Net.Http;
using System.Net.Http.Json;

namespace Patrimonio.Financas.Wpf.Cartoes.HttpClients;

/// <summary>
/// Implementa as operações HTTP de leitura e escrita de lançamentos,
/// utilizando os contratos públicos disponibilizados pelo módulo.
/// </summary>
internal sealed class LancamentoHttpClient(HttpClient httpClient)
    : HttpClientBase<LancamentoListaDto, LancamentoDetalheDto, LancamentoCriacaoDto, LancamentoAlteracaoDto>(
        httpClient, LancamentoEndpoints.Base), ILancamentoCommandService, ILancamentoQueryService
{
    /// <summary>
    /// Lista os lançamentos de uma fatura específica.
    /// </summary>
    /// <param name="faturaId">Identificador da fatura.</param>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    /// <returns>A coleção de lançamentos retornada pela API.</returns>
    public async Task<IReadOnlyCollection<LancamentoListaDto>> ListarAsync(int faturaId, CancellationToken cancellationToken)
    {
        return await HttpClient.GetFromJsonAsync<IReadOnlyCollection<LancamentoListaDto>>(
            LancamentoEndpoints.Listar(faturaId),
            cancellationToken
        ) ?? [];
    }

    /// <summary>
    /// Obtém as opções de referência necessárias para a criação de um lançamento.
    /// </summary>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    /// <returns>A coleção de recursos retornada pela API, ou uma coleção vazia caso não haja nenhum.</returns>
    public async Task<LancamentoOpcoesCriacaoDto> ObterOpcoesCriacaoAsync(CancellationToken cancellationToken)
    {
        return await HttpClient.GetFromJsonAsync<LancamentoOpcoesCriacaoDto>(
            LancamentoEndpoints.OpcoesCriacao(),
            cancellationToken)
            ?? new LancamentoOpcoesCriacaoDto
            {
                Categorias = []
            };
    }
}

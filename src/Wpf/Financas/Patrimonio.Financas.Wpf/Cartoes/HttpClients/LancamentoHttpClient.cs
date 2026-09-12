using Patrimonio.Financas.Contracts.Cartoes.Dtos;
using Patrimonio.Financas.Contracts.Cartoes.Services;
using Patrimonio.Financas.Wpf.Common.Http;
using System.Net.Http;

namespace Patrimonio.Financas.Wpf.Cartoes.HttpClients;

/// <summary>
/// Implementa as operações HTTP de leitura e escrita de lançamentos,
/// utilizando os contratos públicos disponibilizados pelo módulo.
/// </summary>
internal sealed class LancamentoHttpClient(HttpClient httpClient)
    : HttpClientBase<LancamentoListaDto, LancamentoDetalheDto, LancamentoCriacaoDto, LancamentoAlteracaoDto>(
        httpClient, LancamentoEndpoints.Base), ILancamentoCommandService, ILancamentoQueryService;

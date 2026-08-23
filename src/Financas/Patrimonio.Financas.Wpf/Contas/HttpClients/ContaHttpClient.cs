using Patrimonio.Financas.Contracts.Contas.Dtos;
using Patrimonio.Financas.Contracts.Contas.Services;
using Patrimonio.Financas.Wpf.Common.Http;

namespace Patrimonio.Financas.Wpf.Contas.HttpClients;

/// <summary>
/// Implementa as operações HTTP de leitura e escrita de contas,
/// utilizando os contratos públicos disponibilizados pelo módulo.
/// </summary>
internal sealed class ContaHttpClient(HttpClient httpClient)
    : HttpClientBase<ContaListaDto, ContaDetalheDto, ContaCriacaoDto, ContaAlteracaoDto>(
        httpClient, ContaEndpoints.Base), IContaCommandService, IContaQueryService;

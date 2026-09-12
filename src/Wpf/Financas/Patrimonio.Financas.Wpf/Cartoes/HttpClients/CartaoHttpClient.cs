using Patrimonio.Financas.Contracts.Cartoes.Dtos;
using Patrimonio.Financas.Contracts.Cartoes.Services;
using Patrimonio.Financas.Wpf.Common.Http;
using System.Net.Http;

namespace Patrimonio.Financas.Wpf.Cartoes.HttpClients;

/// <summary>
/// Implementa as operações HTTP de leitura e escrita de cartões,
/// utilizando os contratos públicos disponibilizados pelo módulo.
/// </summary>
internal sealed class CartaoHttpClient(HttpClient httpClient)
    : HttpClientBase<CartaoListaDto, CartaoDetalheDto, CartaoCriacaoDto, CartaoAlteracaoDto>(
        httpClient, CartaoEndpoints.Base), ICartaoCommandService, ICartaoQueryService;

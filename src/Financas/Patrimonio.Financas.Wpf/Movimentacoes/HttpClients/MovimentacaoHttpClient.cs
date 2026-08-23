using Patrimonio.Financas.Contracts.Movimentacoes.Dtos;
using Patrimonio.Financas.Contracts.Movimentacoes.Services;
using Patrimonio.Financas.Wpf.Common.Http;

namespace Patrimonio.Financas.Wpf.Movimentacoes.HttpClients;

/// <summary>
/// Implementa as operações HTTP de leitura e escrita de movimentações,
/// utilizando os contratos públicos disponibilizados pelo módulo.
/// </summary>
internal sealed class MovimentacaoHttpClient(HttpClient httpClient)
    : HttpClientBase<MovimentacaoListaDto, MovimentacaoDetalheDto, MovimentacaoCriacaoDto, MovimentacaoAlteracaoDto>(
        httpClient, MovimentacaoEndpoints.Base), IMovimentacaoCommandService, IMovimentacaoQueryService;

using Patrimonio.Financas.Contracts.Instituicoes.Dtos;
using Patrimonio.Financas.Contracts.Instituicoes.Services;
using Patrimonio.Financas.Wpf.Common.Http;
using System.Net.Http;

namespace Patrimonio.Financas.Wpf.Instituicoes.HttpClients;

/// <summary>
/// Implementa as operações HTTP de leitura e escrita de instituições,
/// utilizando os contratos públicos disponibilizados pelo módulo.
/// </summary>
internal sealed class InstituicaoHttpClient(HttpClient httpClient)
    : HttpClientBase<InstituicaoListaDto, InstituicaoDetalheDto, InstituicaoCriacaoDto, InstituicaoAlteracaoDto>(
        httpClient, InstituicaoEndpoints.Base), IInstituicaoCommandService, IInstituicaoQueryService;

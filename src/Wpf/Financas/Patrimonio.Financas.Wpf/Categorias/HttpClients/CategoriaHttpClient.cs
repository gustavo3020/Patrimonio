using Patrimonio.Financas.Contracts.Categorias.Dtos;
using Patrimonio.Financas.Contracts.Categorias.Services;
using Patrimonio.Financas.Wpf.Common.Http;
using System.Net.Http;

namespace Patrimonio.Financas.Wpf.Categorias.HttpClients;

/// <summary>
/// Implementa as operações HTTP de leitura e escrita de categorias,
/// utilizando os contratos públicos disponibilizados pelo módulo.
/// </summary>
internal sealed class CategoriaHttpClient(HttpClient httpClient)
    : HttpClientBase<CategoriaListaDto, CategoriaDetalheDto, CategoriaCriacaoDto, CategoriaAlteracaoDto>(
        httpClient, CategoriaEndpoints.Base), ICategoriaCommandService, ICategoriaQueryService;

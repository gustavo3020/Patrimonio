using Patrimonio.Financas.Wpf.Common.Exceptions;
using System.Net.Http;
using System.Net.Http.Json;

namespace Patrimonio.Financas.Wpf.Common.Http;

/// <summary>
/// Implementa as operações HTTP padrão de leitura e escrita para um recurso da API,
/// servindo como base para os clientes HTTP específicos de cada entidade.
/// </summary>
/// <typeparam name="TListaDto">Tipo do DTO utilizado na listagem do recurso.</typeparam>
/// <typeparam name="TDetalheDto">Tipo do DTO utilizado no detalhamento do recurso.</typeparam>
/// <typeparam name="TCriacaoDto">Tipo do DTO utilizado na criação do recurso.</typeparam>
/// <typeparam name="TAlteracaoDto">Tipo do DTO utilizado na alteração do recurso.</typeparam>
/// <param name="httpClient">Cliente HTTP utilizado para comunicação com a API.</param>
/// <param name="rotaBase">Rota base do recurso, utilizada para montar os endpoints das operações.</param>
internal abstract class HttpClientBase<TListaDto, TDetalheDto, TCriacaoDto, TAlteracaoDto>(
    HttpClient httpClient,
    string rotaBase)
{
    /// <summary>
    /// Obtém a lista de recursos disponíveis na API.
    /// </summary>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    /// <returns>A coleção de recursos retornada pela API, ou uma coleção vazia caso não haja nenhum.</returns>
    public async Task<IReadOnlyCollection<TListaDto>> ListarAsync(CancellationToken cancellationToken)
    {
        return await httpClient.GetFromJsonAsync<IReadOnlyCollection<TListaDto>>(
            rotaBase,
            cancellationToken)
            ?? [];
    }

    /// <summary>
    /// Obtém os dados detalhados de um recurso pelo seu identificador.
    /// </summary>
    /// <param name="id">Identificador do recurso.</param>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    /// <returns>Os dados detalhados do recurso correspondente ao identificador informado.</returns>
    /// <exception cref="ApiResponseException">
    /// Lançada quando a API retorna uma resposta bem-sucedida sem os dados esperados.
    /// </exception>
    public async Task<TDetalheDto> ObterPorIdAsync(int id, CancellationToken cancellationToken)
    {
        return await httpClient.GetFromJsonAsync<TDetalheDto>(
            $"{rotaBase}/{id}",
            cancellationToken)
            ?? throw new ApiResponseException("A API não retornou os dados esperados.");
    }

    /// <summary>
    /// Cria um novo recurso na API.
    /// </summary>
    /// <param name="dto">Dados necessários para a criação do recurso.</param>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    /// <returns>Os dados detalhados do recurso recém-criado.</returns>
    /// <exception cref="ApiResponseException">
    /// Lançada quando a API retorna uma resposta bem-sucedida sem os dados esperados.
    /// </exception>
    public async Task<TDetalheDto> CriarAsync(TCriacaoDto dto, CancellationToken cancellationToken)
    {
        using var response = await httpClient.PostAsJsonAsync(
            rotaBase,
            dto,
            cancellationToken);

        return await response.Content.ReadFromJsonAsync<TDetalheDto>(
            cancellationToken)
            ?? throw new ApiResponseException("A API não retornou os dados do recurso criado.");
    }

    /// <summary>
    /// Altera os dados de um recurso existente.
    /// </summary>
    /// <param name="id">Identificador do recurso a ser alterado.</param>
    /// <param name="dto">Dados a serem aplicados ao recurso.</param>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    public Task AlterarAsync(int id, TAlteracaoDto dto, CancellationToken cancellationToken)
    {
        return httpClient.PutAsJsonAsync(
            $"{rotaBase}/{id}",
            dto,
            cancellationToken);
    }

    /// <summary>
    /// Exclui um recurso existente.
    /// </summary>
    /// <param name="id">Identificador do recurso a ser excluído.</param>
    /// <param name="cancellationToken">Token utilizado para cancelar a operação.</param>
    public Task ExcluirAsync(int id, CancellationToken cancellationToken)
    {
        return httpClient.DeleteAsync(
            $"{rotaBase}/{id}",
            cancellationToken);
    }
}

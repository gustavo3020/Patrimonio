namespace Patrimonio.Financas.Wpf.Categorias.HttpClients;

/// <summary>
/// Centraliza os endpoints HTTP utilizados pelas operações de categorias.
/// </summary>
internal static class CategoriaEndpoints
{
    /// <summary>
    /// Obtém a rota base para as operações de categorias.
    /// </summary>
    public const string Base = "api/v1/financas/categorias";

    /// <summary>
    /// Obtém a rota para operações direcionadas a uma categoria específica.
    /// </summary>
    /// <param name="categoriaId">Identificador da categoria.</param>
    /// <returns>A rota HTTP correspondente à categoria informada.</returns>
    public static string PorId(int categoriaId) => $"{Base}/{categoriaId}";
}

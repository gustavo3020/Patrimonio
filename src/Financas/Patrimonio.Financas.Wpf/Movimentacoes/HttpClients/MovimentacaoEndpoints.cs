namespace Patrimonio.Financas.Wpf.Movimentacoes.HttpClients;

/// <summary>
/// Centraliza os endpoints HTTP utilizados pelas operações de movimentações.
/// </summary>
internal static class MovimentacaoEndpoints
{
    /// <summary>
    /// Obtém a rota base para as operações de movimentações.
    /// </summary>
    public const string Base = "api/v1/financas/movimentacoes";

    /// <summary>
    /// Obtém a rota para operações direcionadas a uma movimentação específica.
    /// </summary>
    /// <param name="movimentacaoId">Identificador da movimentação.</param>
    /// <returns>A rota HTTP correspondente à movimentação informada.</returns>
    public static string PorId(int movimentacaoId) => $"{Base}/{movimentacaoId}";
}

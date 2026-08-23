namespace Patrimonio.Financas.Wpf.Instituicoes.HttpClients;

/// <summary>
/// Centraliza os endpoints HTTP utilizados pelas operações de instituições.
/// </summary>
internal static class InstituicaoEndpoints
{
    /// <summary>
    /// Obtém a rota base para as operações de instituições.
    /// </summary>
    public const string Base = "api/v1/financas/instituicoes";

    /// <summary>
    /// Obtém a rota para operações direcionadas a uma instituição específica.
    /// </summary>
    /// <param name="instituicaoId">Identificador da instituição.</param>
    /// <returns>A rota HTTP correspondente à instituição informada.</returns>
    public static string PorId(int instituicaoId) => $"{Base}/{instituicaoId}";
}

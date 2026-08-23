namespace Patrimonio.Financas.Wpf.Contas.HttpClients;

/// <summary>
/// Centraliza os endpoints HTTP utilizados pelas operações de contas.
/// </summary>
internal static class ContaEndpoints
{
    /// <summary>
    /// Obtém a rota base para as operações de contas.
    /// </summary>
    public const string Base = "api/v1/financas/contas";

    /// <summary>
    /// Obtém a rota para operações direcionadas a uma conta específica.
    /// </summary>
    /// <param name="contaId">Identificador da conta.</param>
    /// <returns>A rota HTTP correspondente à conta informada.</returns>
    public static string PorId(int contaId) => $"{Base}/{contaId}";
}

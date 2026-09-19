namespace Patrimonio.Financas.Wpf.Cartoes.HttpClients;

/// <summary>
/// Centraliza os endpoints HTTP utilizados pelas operações de lançamentos.
/// </summary>
internal static class LancamentoEndpoints
{
    /// <summary>
    /// Obtém a rota base para as operações de lançamentos.
    /// </summary>
    public const string Base = "api/v1/financas/lancamentos";
    public static string Listar(int faturaId) => $"{Base}?faturaId={faturaId}";
    public static string OpcoesCriacao() => $"{Base}/opcoes-criacao";
}

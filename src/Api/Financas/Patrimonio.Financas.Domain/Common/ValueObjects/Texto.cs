using Patrimonio.Financas.Domain.Exceptions;

namespace Patrimonio.Financas.Domain.Common.ValueObjects;

/// <summary>
/// Representa um texto válido utilizado pelas entidades do domínio.
/// </summary>
public abstract record Texto
{
    /// <summary>
    /// Inicializa uma nova instância de <see cref="Texto"/>.
    /// </summary>
    /// <param name="valor">Valor do texto.</param>
    /// <param name="tamanhoMaximo">Tamanho máximo permitido para o texto.</param>
    /// <param name="nomeCampo">Nome do campo para mensagens de erro.</param>
    /// <returns>Uma instância de <see cref="Texto"/>.</returns>
    /// <exception cref="RegraDeNegocioException">
    /// Lançada quando o texto não é informado ou ultrapassa o tamanho máximo permitido.
    /// </exception>
    protected Texto(string valor, int tamanhoMaximo, string nomeCampo)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new RegraDeNegocioException($"O campo {nomeCampo} é obrigatório.");

        valor = valor.Trim();

        if (valor.Length > tamanhoMaximo)
            throw new RegraDeNegocioException($"O campo {nomeCampo} deve possuir no máximo {tamanhoMaximo} caracteres.");

        Valor = valor;
    }

    public string Valor { get; }
}

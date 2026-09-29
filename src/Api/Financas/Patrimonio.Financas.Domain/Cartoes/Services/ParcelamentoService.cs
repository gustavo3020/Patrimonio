using Patrimonio.Financas.Domain.Exceptions;

namespace Patrimonio.Financas.Domain.Cartoes.Services;

/// <summary>
/// Serviço responsável por calcular o valor das parcelas de um lançamento.
/// </summary>
public static class ParcelamentoService
{
    public static decimal[] CalcularParcelas(decimal valorTotal, int totalParcelas)
    {
        if (totalParcelas <= 0)
            throw new RegraDeNegocioException("O número total de parcelas deve ser maior que zero.");

        var valorParcelaBase = Math.Round(valorTotal / totalParcelas, 2, MidpointRounding.ToZero);
        var somaBase = valorParcelaBase * totalParcelas;
        var diferenca = valorTotal - somaBase;

        var parcelas = Enumerable.Repeat(valorParcelaBase, totalParcelas).ToArray();

        parcelas[0] += diferenca;

        return parcelas;
    }
}

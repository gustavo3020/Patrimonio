using Patrimonio.Financas.Domain.Cartoes.Services;
using Patrimonio.Financas.Domain.Exceptions;

namespace Patrimonio.Financas.Tests.Unit.Cartoes.Domain.Services;

public sealed class ParcelamentoServiceTests
{
    [Fact]
    public void DeveDividir100Em3ParcelasCorretamente()
    {
        var parcelas = ParcelamentoService.CalcularParcelas(100m, 3);

        Assert.Equal(3, parcelas.Length);
        Assert.Equal(33.34m, parcelas[0]);
        Assert.Equal(33.33m, parcelas[1]);
        Assert.Equal(33.33m, parcelas[2]);
        Assert.Equal(100m, parcelas.Sum());
    }

    [Fact]
    public void DeveDividir100_01Em3ParcelasCorretamente()
    {
        var parcelas = ParcelamentoService.CalcularParcelas(100.01m, 3);

        Assert.Equal(3, parcelas.Length);
        Assert.Equal(33.35m, parcelas[0]);
        Assert.Equal(33.33m, parcelas[1]);
        Assert.Equal(33.33m, parcelas[2]);
        Assert.Equal(100.01m, parcelas.Sum());
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void DeveLancarExcecaoQuandoTotalParcelasForMenorQueUm(int totalParcelas)
    {
        Assert.Throws<RegraDeNegocioException>(() => ParcelamentoService.CalcularParcelas(100m, totalParcelas));
    }
}


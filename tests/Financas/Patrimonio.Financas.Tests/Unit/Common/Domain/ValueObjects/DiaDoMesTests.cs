using FluentAssertions;
using Patrimonio.Financas.Domain.Common.ValueObjects;
using Patrimonio.Financas.Domain.Exceptions;

namespace Patrimonio.Financas.Tests.Unit.Common.Domain.ValueObjects;

public sealed class DiaDoMesTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(15)]
    [InlineData(31)]
    public void DeveCriarDiaDoMesComValorValido(int valor)
    {
        // Act
        var dia = new DiaDoMes(valor);

        // Assert
        dia.Valor.Should().Be(valor);
    }

    [Theory]
    [InlineData(-10)]
    [InlineData(0)]
    [InlineData(32)]
    public void DeveRejeitarDiaDoMesInvalido(int valorInvalido)
    {
        // Act
        var acao = () => new DiaDoMes(valorInvalido);

        // Assert
        acao.Should().Throw<RegraDeNegocioException>();
    }

    [Fact]
    public void DoisDiasDoMesComMesmoValorDevemSerIguais()
    {
        // Arrange
        var dia1 = new DiaDoMes(10);
        var dia2 = new DiaDoMes(10);

        // Assert
        dia1.Should().Be(dia2);
        dia1.GetHashCode().Should().Be(dia2.GetHashCode());
    }

    [Fact]
    public void DoisDiasDoMesComValoresDiferentesNaoDevemSerIguais()
    {
        // Arrange
        var dia1 = new DiaDoMes(10);
        var dia2 = new DiaDoMes(20);

        // Assert
        dia1.Should().NotBe(dia2);
    }

    [Fact]
    public void ToStringDeveRetornarValorComoTexto()
    {
        new DiaDoMes(12).ToString().Should().Be("12");
    }
}

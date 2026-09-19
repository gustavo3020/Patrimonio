using FluentAssertions;
using Patrimonio.Financas.Domain.Common.ValueObjects;
using Patrimonio.Financas.Domain.Exceptions;

namespace Patrimonio.Financas.Tests.Unit.Common.Domain.ValueObjects;

public sealed class DinheiroTests
{
    [Theory]
    [InlineData(150.75)]
    [InlineData(150.7)]
    [InlineData(150)]
    public void DeveCriarDinheiroComValorValido(decimal valor)
    {
        // Act
        var dinheiro = new Dinheiro(valor);

        // Assert
        dinheiro.Valor.Should().Be(valor);
    }

    [Theory]
    [InlineData(-30)]
    [InlineData(0)]
    [InlineData(150.756)]
    public void DeveRejeitarDinheiroComValorInvalido(decimal valorInvalido)
    {
        // Act
        var acao = () => new Dinheiro(valorInvalido);

        // Assert
        acao.Should().Throw<RegraDeNegocioException>();
    }

    [Fact]
    public void DeveFormatarValorComDuasCasasDecimais()
    {
        // Act
        var dinheiro = new Dinheiro(150m);

        // Assert
        dinheiro.ToString().Should().Be("150.00");
    }

    [Fact]
    public void DoisDinheirosComMesmoValorDevemSerIguais()
    {
        // Arrange
        var a = new Dinheiro(150.75m);
        var b = new Dinheiro(150.75m);

        // Assert
        a.Should().Be(b);
        a.GetHashCode().Should().Be(b.GetHashCode());
    }

    [Fact]
    public void DoisDinheirosComValoresDiferentesNaoDevemSerIguais()
    {
        // Arrange
        var a = new Dinheiro(150.75m);
        var b = new Dinheiro(200m);

        // Assert
        a.Should().NotBe(b);
    }

    [Fact]
    public void ToStringDeveRetornarValorComoTexto()
    {
        new Dinheiro(150.75m).ToString().Should().Be("150.75");
    }
}

using FluentAssertions;
using Patrimonio.Financas.Domain.Exceptions;
using Patrimonio.Financas.Domain.Movimentacoes.ValueObjects;

namespace Patrimonio.Financas.Tests.Domain.Movimentacoes.ValueObjects;

public sealed class DinheiroTests
{
    [Fact]
    public void DeveCriarDinheiroComValorValido()
    {
        var dinheiro = new Dinheiro(150.75m);

        dinheiro.Valor.Should().Be(150.75m);
    }

    [Fact]
    public void DeveAceitarValorComApenasDuasCasasDecimais()
    {
        var dinheiro = new Dinheiro(150.74m);

        dinheiro.Valor.Should().Be(150.74m);
    }

    [Fact]
    public void DeveAceitarValorInteiro()
    {
        var dinheiro = new Dinheiro(150m);

        dinheiro.Valor.Should().Be(150m);
        dinheiro.ToString().Should().Be("150.00");
    }

    [Fact]
    public void DeveRejeitarValorZero()
    {
        var acao = () => new Dinheiro(0m);

        acao.Should()
            .Throw<RegraDeNegocioException>()
            .WithMessage("O valor deve ser maior que zero.");
    }

    [Fact]
    public void DeveRejeitarValorNegativo()
    {
        var acao = () => new Dinheiro(-10m);

        acao.Should()
            .Throw<RegraDeNegocioException>()
            .WithMessage("O valor deve ser maior que zero.");
    }

    [Fact]
    public void DeveRejeitarValorComMaisDeDuasCasasDecimais()
    {
        var acao = () => new Dinheiro(150.751m);

        acao.Should()
            .Throw<RegraDeNegocioException>()
            .WithMessage("O valor deve possuir no máximo duas casas decimais.");
    }

    [Fact]
    public void NaoDeveArredondarValorComMaisDeDuasCasasDecimais()
    {
        var valor = 150.751m;

        var acao = () => new Dinheiro(valor);

        acao.Should()
            .Throw<RegraDeNegocioException>();
    }

    [Fact]
    public void DeveFormatarValorComDuasCasasDecimais()
    {
        var dinheiro = new Dinheiro(150m);

        dinheiro.ToString().Should().Be("150.00");
    }

    [Fact]
    public void DoisDinheirosComMesmoValorDevemSerIguais()
    {
        var a = new Dinheiro(150.75m);
        var b = new Dinheiro(150.75m);

        a.Should().Be(b);
        (a == b).Should().BeTrue();
        a.GetHashCode().Should().Be(b.GetHashCode());
    }

    [Fact]
    public void DoisDinheirosComValoresDiferentesNaoDevemSerIguais()
    {
        var a = new Dinheiro(150.75m);
        var b = new Dinheiro(200m);

        a.Should().NotBe(b);
        (a == b).Should().BeFalse();
    }
}

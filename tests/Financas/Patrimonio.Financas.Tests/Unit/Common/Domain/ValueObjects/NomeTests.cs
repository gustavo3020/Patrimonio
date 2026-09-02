using FluentAssertions;
using Patrimonio.Financas.Domain.Common.ValueObjects;
using Patrimonio.Financas.Domain.Exceptions;

namespace Patrimonio.Financas.Tests.Unit.Common.Domain.ValueObjects;

public sealed class NomeTests
{
    [Fact]
    public void DeveCriarNomeValido()
    {
        var nome = new Nome("Banco do Brasil");

        nome.Valor.Should().Be("Banco do Brasil");
        nome.Normalizado.Should().Be("BANCO DO BRASIL");
    }

    [Fact]
    public void DeveRemoverEspacosNoInicioEFim()
    {
        var nome = new Nome("  Banco do Brasil  ");

        nome.Valor.Should().Be("Banco do Brasil");
        nome.Normalizado.Should().Be("BANCO DO BRASIL");
    }

    [Fact]
    public void DeveNormalizarNomeEmMaiusculas()
    {
        var nome = new Nome("Banco do Brasil");

        nome.Normalizado.Should().Be("BANCO DO BRASIL");
    }

    [Fact]
    public void DeveRejeitarNomeNulo()
    {
        var acao = () => new Nome(null!);

        acao.Should()
            .Throw<RegraDeNegocioException>()
            .WithMessage("O nome deve ser informado.");
    }

    [Fact]
    public void DeveRejeitarNomeVazio()
    {
        var acao = () => new Nome(string.Empty);

        acao.Should()
            .Throw<RegraDeNegocioException>()
            .WithMessage("O nome deve ser informado.");
    }

    [Fact]
    public void DeveRejeitarNomeComApenasEspacos()
    {
        var acao = () => new Nome("     ");

        acao.Should()
            .Throw<RegraDeNegocioException>()
            .WithMessage("O nome deve ser informado.");
    }

    [Fact]
    public void DeveRejeitarNomeComMaisDe200Caracteres()
    {
        var valor = new string('A', 201);

        var acao = () => new Nome(valor);

        acao.Should()
            .Throw<RegraDeNegocioException>()
            .WithMessage("O nome deve possuir no máximo 200 caracteres.");
    }

    [Fact]
    public void DeveAceitarNomeComExatamente200Caracteres()
    {
        var valor = new string('A', 200);

        var nome = new Nome(valor);

        nome.Valor.Should().Be(valor);
    }

    [Fact]
    public void DoisNomesComMesmoValorDevemSerIguais()
    {
        var a = new Nome("Banco do Brasil");
        var b = new Nome("Banco do Brasil");

        a.Should().Be(b);
        (a == b).Should().BeTrue();
        a.GetHashCode().Should().Be(b.GetHashCode());
    }

    [Fact]
    public void DoisNomesComValoresDiferentesNaoDevemSerIguais()
    {
        var a = new Nome("Banco do Brasil");
        var b = new Nome("Bradesco");

        a.Should().NotBe(b);
        (a == b).Should().BeFalse();
    }
}

using FluentAssertions;
using Patrimonio.Financas.Domain.Cartoes.ValueObjects;

namespace Patrimonio.Financas.Tests.Unit.Cartoes.Domain.ValueObjects;

public sealed class ResponsavelTests
{
    [Fact]
    public void Criar_DeveCriarResponsavel_QuandoDadosValidos()
    {
        var nome = new Responsavel("Responsável Teste");

        nome.Valor.Should().Be("Responsável Teste");
    }

    [Fact]
    public void Igualdade_DeveConsiderarIgual_QuandoMesmoValor()
    {
        var a = new Responsavel("Responsável Teste");
        var b = new Responsavel("Responsável Teste");

        a.Should().Be(b);
        a.GetHashCode().Should().Be(b.GetHashCode());
    }

    [Fact]
    public void Igualdade_DeveConsiderarDiferente_QuandoValoresDiferentes()
    {
        var a = new Responsavel("Responsável Teste");
        var b = new Responsavel("Outro Responsável");

        a.Should().NotBe(b);
    }

    [Fact]
    public void ToString_DeveRetornarValorComoTexto_QuandoChamado()
    {
        var nome = new Responsavel("Responsável Teste");

        nome.ToString().Should().Be("Responsável Teste");
    }
}

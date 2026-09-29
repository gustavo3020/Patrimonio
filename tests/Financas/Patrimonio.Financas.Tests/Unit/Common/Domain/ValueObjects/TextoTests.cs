using FluentAssertions;
using Patrimonio.Financas.Domain.Common.ValueObjects;
using Patrimonio.Financas.Domain.Exceptions;

namespace Patrimonio.Financas.Tests.Unit.Common.Domain.ValueObjects;

public sealed class TextoTests
{
    private sealed record TextoTeste(string Valor) : Texto(Valor, 200, "teste");

    [Theory]
    [InlineData("Texto Teste")]
    [InlineData("  Texto Teste  ")]
    public void Criar_DeveCriarTexto_QuandoValorValido(string valor)
    {
        var texto = new TextoTeste(valor);

        texto.Valor.Should().Be("Texto Teste");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Criar_DeveLancarExcecao_QuandoValorInvalido(string valorInvalido)
    {
        var acao = () => new TextoTeste(valorInvalido);

        acao.Should().Throw<RegraDeNegocioException>();
    }

    [Fact]
    public void Criar_DeveAceitarTextoComExatamente200Caracteres_QuandoTamanhoMaximo()
    {
        var valor = new string('A', 200);

        var texto = new TextoTeste(valor);

        texto.Valor.Should().Be(valor);
    }

    [Fact]
    public void Criar_DeveLancarExcecao_QuandoMaiorQueMaximo()
    {
        var valor = new string('A', 201);

        var acao = () => new TextoTeste(valor);

        acao.Should().Throw<RegraDeNegocioException>();
    }
}

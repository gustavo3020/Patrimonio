using FluentAssertions;
using Patrimonio.Financas.Domain.Exceptions;
using Patrimonio.Financas.Domain.Movimentacoes.Entities;
using Patrimonio.Financas.Domain.Movimentacoes.ValueObjects;
using Patrimonio.Financas.SharedKernel.Movimentacoes.Enums;

namespace Patrimonio.Financas.Tests.Unit.Movimentacoes.Domain.Entities;

public sealed class MovimentacaoTests
{
    [Fact]
    public void DeveCriarMovimentacao()
    {
        var data = new DateOnly(2026, 8, 15);
        var valor = new Dinheiro(150.75m);

        var movimentacao = Movimentacao.Criar(
            data,
            valor,
            Natureza.Saida,
            TipoMovimentacao.Pix,
            "Pagamento",
            1,
            2);

        movimentacao.Data.Should().Be(data);
        movimentacao.Valor.Should().Be(valor);
        movimentacao.Natureza.Should().Be(Natureza.Saida);
        movimentacao.Tipo.Should().Be(TipoMovimentacao.Pix);
        movimentacao.Descricao.Should().Be("Pagamento");
        movimentacao.ContaId.Should().Be(1);
        movimentacao.CategoriaId.Should().Be(2);
    }

    [Fact]
    public void DevePermitirDescricaoNula()
    {
        var movimentacao = Movimentacao.Criar(
            new DateOnly(2026, 8, 15),
            new Dinheiro(100m),
            Natureza.Saida,
            TipoMovimentacao.Especie,
            null,
            1,
            2);

        movimentacao.Descricao.Should().BeNull();
    }

    [Fact]
    public void DeveRemoverEspacosDaDescricao()
    {
        var movimentacao = Movimentacao.Criar(
            new DateOnly(2026, 8, 15),
            new Dinheiro(100m),
            Natureza.Saida,
            TipoMovimentacao.Pix,
            "  Pagamento  ",
            1,
            2);

        movimentacao.Descricao.Should().Be("Pagamento");
    }

    [Fact]
    public void DevePermitirAlterarData()
    {
        var movimentacao = CriarMovimentacao();

        var novaData = new DateOnly(2026, 8, 20);

        movimentacao.AlterarData(novaData);

        movimentacao.Data.Should().Be(novaData);
    }

    [Fact]
    public void DevePermitirAlterarValor()
    {
        var movimentacao = CriarMovimentacao();

        var novoValor = new Dinheiro(250.50m);

        movimentacao.AlterarValor(novoValor);

        movimentacao.Valor.Should().Be(novoValor);
    }

    [Fact]
    public void DevePermitirAlterarNatureza()
    {
        var movimentacao = CriarMovimentacao();

        var novaNatureza = Natureza.Entrada;

        movimentacao.AlterarNatureza(novaNatureza);

        movimentacao.Natureza.Should().Be(novaNatureza);
    }

    [Fact]
    public void DevePermitirAlterarTipo()
    {
        var movimentacao = CriarMovimentacao();

        movimentacao.AlterarTipo(TipoMovimentacao.Boleto);

        movimentacao.Tipo.Should().Be(TipoMovimentacao.Boleto);
    }

    [Fact]
    public void DevePermitirAlterarDescricao()
    {
        var movimentacao = CriarMovimentacao();

        movimentacao.AlterarDescricao("Nova descrição");

        movimentacao.Descricao.Should().Be("Nova descrição");
    }

    [Fact]
    public void DevePermitirRemoverDescricao()
    {
        var movimentacao = CriarMovimentacao();

        movimentacao.AlterarDescricao(null);

        movimentacao.Descricao.Should().BeNull();
    }

    [Fact]
    public void DevePermitirAlterarConta()
    {
        var movimentacao = CriarMovimentacao();

        movimentacao.AlterarConta(5);

        movimentacao.ContaId.Should().Be(5);
    }

    [Fact]
    public void DevePermitirAlterarCategoria()
    {
        var movimentacao = CriarMovimentacao();

        movimentacao.AlterarCategoria(8);

        movimentacao.CategoriaId.Should().Be(8);
    }

    [Fact]
    public void DeveRejeitarDescricaoComMaisDe500Caracteres()
    {
        var descricao = new string('A', 501);

        var acao = () => Movimentacao.Criar(
            new DateOnly(2026, 8, 15),
            new Dinheiro(100m),
            Natureza.Saida,
            TipoMovimentacao.Pix,
            descricao,
            1,
            2);

        acao.Should()
            .Throw<RegraDeNegocioException>()
            .WithMessage("A descrição deve possuir no máximo 500 caracteres.");
    }

    [Fact]
    public void DeveRejeitarDescricaoComMaisDe500CaracteresAoAlterar()
    {
        var movimentacao = CriarMovimentacao();

        var descricao = new string('A', 501);

        var acao = () => movimentacao.AlterarDescricao(descricao);

        acao.Should()
            .Throw<RegraDeNegocioException>()
            .WithMessage("A descrição deve possuir no máximo 500 caracteres.");
    }

    private static Movimentacao CriarMovimentacao()
    {
        return Movimentacao.Criar(
            new DateOnly(2026, 8, 15),
            new Dinheiro(100m),
            Natureza.Saida,
            TipoMovimentacao.Pix,
            "Pagamento",
            1,
            2);
    }
}

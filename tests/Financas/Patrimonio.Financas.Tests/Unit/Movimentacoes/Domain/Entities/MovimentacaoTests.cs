using FluentAssertions;
using Patrimonio.Financas.Domain.Common.ValueObjects;
using Patrimonio.Financas.Domain.Exceptions;
using Patrimonio.Financas.Domain.Movimentacoes.Entities;
using Patrimonio.Financas.SharedKernel.Movimentacoes.Enums;

namespace Patrimonio.Financas.Tests.Unit.Movimentacoes.Domain.Entities;

public sealed class MovimentacaoTests
{
    [Fact]
    public void Criar_DeveCriarMovimentacao_QuandoDadosValidos()
    {
        // Arrange
        var data = new DateOnly(2026, 8, 15);
        var valor = new Dinheiro(150.75m);
        var descricao = new Descricao("Pagamento de teste");

        // Act
        var movimentacao = Movimentacao.Criar(
            data,
            valor,
            Natureza.Saida,
            TipoMovimentacao.Pix,
            descricao,
            1,
            2);

        // Assert
        movimentacao.Data.Should().Be(data);
        movimentacao.Valor.Should().Be(valor);
        movimentacao.Natureza.Should().Be(Natureza.Saida);
        movimentacao.Tipo.Should().Be(TipoMovimentacao.Pix);
        movimentacao.Descricao.Should().Be(descricao);
        movimentacao.ContaId.Should().Be(1);
        movimentacao.CategoriaId.Should().Be(2);
    }

    [Fact]
    public void Criar_DeveCriarMovimentacao_QuandoDescricaoNula()
    {
        // Act
        var movimentacao = Movimentacao.Criar(
            new DateOnly(2026, 8, 15),
            new Dinheiro(100m),
            Natureza.Saida,
            TipoMovimentacao.Especie,
            null,
            1,
            2);

        // Assert
        movimentacao.Descricao.Should().BeNull();
    }

    [Fact]
    public void Alterar_DeveAlterarMovimentacao_QuandoDadosValidos()
    {
        // Arrange
        var movimentacao = CriarMovimentacao();
        var novaData = new DateOnly(2026, 9, 1);
        var novoValor = new Dinheiro(200m);
        var novaDescricao = new Descricao("Nova descrição");

        // Act
        movimentacao.Alterar(
            novaData,
            novoValor,
            Natureza.Entrada,
            TipoMovimentacao.Credito,
            novaDescricao,
            3,
            4);

        // Assert
        movimentacao.Data.Should().Be(novaData);
        movimentacao.Valor.Should().Be(novoValor);
        movimentacao.Natureza.Should().Be(Natureza.Entrada);
        movimentacao.Tipo.Should().Be(TipoMovimentacao.Credito);
        movimentacao.Descricao.Should().Be(novaDescricao);
        movimentacao.ContaId.Should().Be(3);
        movimentacao.CategoriaId.Should().Be(4);
    }

    [Fact]
    public void Alterar_DeveAlterarMovimentacao_QuandoDescricaoNula()
    {
        // Arrange
        var movimentacao = CriarMovimentacao();
        var novaData = new DateOnly(2026, 9, 1);
        var novoValor = new Dinheiro(200m);

        // Act
        movimentacao.Alterar(
            novaData,
            novoValor,
            Natureza.Entrada,
            TipoMovimentacao.Credito,
            null,
            3,
            4);

        // Assert
        movimentacao.Data.Should().Be(novaData);
        movimentacao.Valor.Should().Be(novoValor);
        movimentacao.Natureza.Should().Be(Natureza.Entrada);
        movimentacao.Tipo.Should().Be(TipoMovimentacao.Credito);
        movimentacao.Descricao.Should().Be(null);
        movimentacao.ContaId.Should().Be(3);
        movimentacao.CategoriaId.Should().Be(4);
    }

    [Fact]
    public void Alterar_DeveLancarExcecao_QuandoAssociadaAFatura()
    {
        // Arrange
        var movimentacao = CriarMovimentacao(faturaId: 1);

        // Act
        var acao = () => movimentacao.Alterar(
            new DateOnly(2026, 9, 1),
            new Dinheiro(200m),
            Natureza.Entrada,
            TipoMovimentacao.Credito,
            new Descricao("Nova descrição"),
            3,
            4);

        // Assert
        acao.Should().Throw<RegraDeNegocioException>();
    }

    [Fact]
    public void Excluir_DeveLancarExcecao_QuandoAssociadaAFatura()
    {
        // Arrange
        var movimentacao = CriarMovimentacao(faturaId: 1);

        // Act
        var acao = () => movimentacao.Excluir();

        // Assert
        acao.Should().Throw<RegraDeNegocioException>();
    }

    private static Movimentacao CriarMovimentacao(int? faturaId = null)
    {
        return Movimentacao.Criar(
            new DateOnly(2026, 8, 15),
            new Dinheiro(100m),
            Natureza.Saida,
            TipoMovimentacao.Pix,
            new Descricao("Pagamento"),
            1,
            2,
            faturaId: faturaId);
    }
}

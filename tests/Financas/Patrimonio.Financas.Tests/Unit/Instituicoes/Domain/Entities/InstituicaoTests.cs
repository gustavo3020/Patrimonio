using FluentAssertions;
using Patrimonio.Financas.Domain.Common.ValueObjects;
using Patrimonio.Financas.Domain.Instituicoes.Entities;

namespace Patrimonio.Financas.Tests.Unit.Instituicoes.Domain.Entities;

public sealed class InstituicaoTests
{
    [Fact]
    public void Criar_DeveCriarInstituicao_QuandoDadosValidos()
    {
        var nome = new Nome("Banco do Brasil");

        var instituicao = Instituicao.Criar(nome);

        instituicao.Nome.Should().Be(nome);
    }

    [Fact]
    public void Alterar_DeveAlterarNome_QuandoDadosValidos()
    {
        var nome = new Nome("Banco do Brasil");

        var instituicao = Instituicao.Criar(nome);

        var novoNome = new Nome("Banco Bradesco");

        instituicao.AlterarNome(novoNome);

        instituicao.Nome.Should().Be(novoNome);
    }
}

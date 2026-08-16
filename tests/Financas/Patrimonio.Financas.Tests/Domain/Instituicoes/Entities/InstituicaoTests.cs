using FluentAssertions;
using Patrimonio.Financas.Domain.Common.ValueObjects;
using Patrimonio.Financas.Domain.Instituicoes.Entities;

namespace Patrimonio.Financas.Tests.Domain.Instituicoes.Entities;

public sealed class InstituicaoTests
{
    [Fact]
    public void DeveCriarInstituicao()
    {
        var nome = new Nome("Banco do Brasil");

        var instituicao = Instituicao.Criar(nome);

        instituicao.Nome.Should().Be(nome);
    }

    [Fact]
    public void DeveAlterarNome()
    {
        var nome = new Nome("Banco do Brasil");

        var instituicao = Instituicao.Criar(nome);

        var novoNome = new Nome("Banco Bradesco");

        instituicao.AlterarNome(novoNome);

        instituicao.Nome.Should().Be(novoNome);
    }
}

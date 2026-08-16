using FluentAssertions;
using Patrimonio.Financas.Domain.Common.ValueObjects;
using Patrimonio.Financas.Domain.Contas.Entities;

namespace Patrimonio.Financas.Tests.Domain.Contas.Entities;

public sealed class ContaTests
{
    [Fact]
    public void DeveCriarConta()
    {
        var nome = new Nome("Conta Corrente");

        var conta = Conta.Criar(1, nome);

        conta.InstituicaoId.Should().Be(1);
        conta.Nome.Should().Be(nome);
    }

    [Fact]
    public void DeveAlterarNome()
    {
        var nome = new Nome("Conta Corrente");

        var conta = Conta.Criar(1, nome);

        var novoNome = new Nome("Conta Principal");

        conta.AlterarNome(novoNome);

        conta.Nome.Should().Be(novoNome);
    }

    [Fact]
    public void DeveAlterarInstituicao()
    {
        var nome = new Nome("Conta Corrente");

        var conta = Conta.Criar(1, nome);

        conta.AlterarInstituicao(2);

        conta.InstituicaoId.Should().Be(2);
    }
}

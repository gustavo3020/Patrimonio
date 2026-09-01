using Microsoft.Extensions.DependencyInjection;
using Patrimonio.Financas.Tests.Integration.Config;

namespace Patrimonio.Financas.Tests.Integration.Base;

/// <summary>
/// Fornece a infraestrutura base para os testes de integração de interfaces públicas.
/// </summary>
public abstract class IntegrationTestBase(IntegrationTestFactory factory)
{
    protected IntegrationTestFactory Factory { get; } = factory;

    protected IServiceScope CreateScope() => Factory.Services.CreateScope();
}

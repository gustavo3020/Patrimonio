using Patrimonio.Financas.Tests.Integration.Config;

namespace Patrimonio.Financas.Tests.Integration.Base;

/// <summary>
/// Fornece a infraestrutura base para os testes de integração de chamadas HTTP.
/// </summary>
public abstract class ApiIntegrationTestBase(IntegrationTestFactory factory)
    : IntegrationTestBase(factory)
{
    protected HttpClient Client => Factory.CreateClient();
}

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Patrimonio.Financas.Infrastructure.Persistence;
using Patrimonio.Financas.Tests.Integration.Base;
using Patrimonio.Financas.Tests.Integration.Seeders;

namespace Patrimonio.Financas.Tests.Integration.Config;

/// <summary>
/// Configura e cria a aplicação utilizada nos testes de integração.
/// </summary>
public class IntegrationTestFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    internal BaseData BaseData { get; private set; } = null!;

    /// <summary>
    /// Configura o ambiente e os serviços adicionais utilizados pela aplicação nos testes de integração.
    /// </summary>
    /// <param name="builder">Construtor utilizado para configurar o ambiente de hospedagem da aplicação.</param>
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            services.AddScoped<CategoriaSeeder>();
            services.AddScoped<ContaSeeder>();
            services.AddScoped<InstituicaoSeeder>();
            services.AddScoped<MovimentacaoSeeder>();
        });
    }

    /// <summary>
    /// Inicializa o banco de dados e os dados base utilizados pelos testes de integração.
    /// </summary>
    public async ValueTask InitializeAsync()
    {
        using var scope = Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<FinancasDbContext>();

        await context.Database.MigrateAsync();

        BaseData = new BaseData();

        await scope.ServiceProvider.GetRequiredService<CategoriaSeeder>().SeedAsync(BaseData);
        await scope.ServiceProvider.GetRequiredService<InstituicaoSeeder>().SeedAsync(BaseData);
        await scope.ServiceProvider.GetRequiredService<ContaSeeder>().SeedAsync(BaseData);
        await scope.ServiceProvider.GetRequiredService<MovimentacaoSeeder>().SeedAsync(BaseData);
    }

    public ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
    }
}

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Patrimonio.Financas.Infrastructure.Persistence;
using Patrimonio.Financas.Tests.Integration.Base;
using Patrimonio.Financas.Tests.Integration.Seeders;
using Testcontainers.PostgreSql;

namespace Patrimonio.Financas.Tests.Integration.Config;

/// <summary>
/// Configura e cria a aplicação utilizada nos testes de integração.
/// </summary>
public sealed class IntegrationTestFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    internal BaseData BaseData { get; private set; } = null!;
    private readonly PostgreSqlContainer? _container;

    public IntegrationTestFactory()
    {
        var usarContainer = Environment.GetEnvironmentVariable("GITHUB_ACTIONS") == "true";

        if (usarContainer)
            _container = new PostgreSqlBuilder("postgres:17").Build();
    }

    /// <summary>
    /// Configura o ambiente e os serviços adicionais utilizados pela aplicação nos testes de integração.
    /// </summary>
    /// <param name="builder">Construtor utilizado para configurar o ambiente de hospedagem da aplicação.</param>
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        if (_container is not null)
        {
            builder.ConfigureAppConfiguration((_, config) =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["ConnectionStrings:DatabaseConnectionString"] = _container.GetConnectionString()
                });
            });
        }

        builder.ConfigureServices(services =>
        {
            services.AddScoped<CartaoSeeder>();
            services.AddScoped<CategoriaSeeder>();
            services.AddScoped<ContaSeeder>();
            services.AddScoped<FaturaSeeder>();
            services.AddScoped<InstituicaoSeeder>();
            services.AddScoped<LancamentoSeeder>();
            services.AddScoped<MovimentacaoSeeder>();
        });
    }

    /// <summary>
    /// Inicializa o banco de dados e os dados base utilizados pelos testes de integração.
    /// </summary>
    public async ValueTask InitializeAsync()
    {
        if (_container is not null)
        {
            await _container.StartAsync();

            Environment.SetEnvironmentVariable(
                "ConnectionStrings__DatabaseConnectionString",
                _container.GetConnectionString());
        }

        using var scope = Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<FinancasDbContext>();

        await context.Database.MigrateAsync();

        BaseData = new BaseData();

        await scope.ServiceProvider.GetRequiredService<CategoriaSeeder>().SeedAsync(BaseData);
        await scope.ServiceProvider.GetRequiredService<InstituicaoSeeder>().SeedAsync(BaseData);
        await scope.ServiceProvider.GetRequiredService<ContaSeeder>().SeedAsync(BaseData);
        await scope.ServiceProvider.GetRequiredService<MovimentacaoSeeder>().SeedAsync(BaseData);
        await scope.ServiceProvider.GetRequiredService<CartaoSeeder>().SeedAsync(BaseData);
        await scope.ServiceProvider.GetRequiredService<FaturaSeeder>().SeedAsync(BaseData);
        await scope.ServiceProvider.GetRequiredService<LancamentoSeeder>().SeedAsync(BaseData);
    }

    public override async ValueTask DisposeAsync()
    {
        if (_container is not null)
            await _container.DisposeAsync();
    }
}

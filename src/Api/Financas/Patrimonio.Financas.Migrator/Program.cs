using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Patrimonio.Financas.Infrastructure;
using Patrimonio.Financas.Infrastructure.Persistence;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.AddUserSecrets<Program>();

builder.Services.AddFinancasInfrastructure(builder.Configuration);

using var host = builder.Build();

using var scope = host.Services.CreateScope();

var context = scope.ServiceProvider.GetRequiredService<FinancasDbContext>();

await context.Database.MigrateAsync();

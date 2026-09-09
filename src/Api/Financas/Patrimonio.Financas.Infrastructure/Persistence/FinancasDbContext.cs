using Microsoft.EntityFrameworkCore;
using Patrimonio.Financas.Domain.Cartoes.Entities;
using Patrimonio.Financas.Domain.Categorias.Entities;
using Patrimonio.Financas.Domain.Contas.Entities;
using Patrimonio.Financas.Domain.Instituicoes.Entities;
using Patrimonio.Financas.Domain.Movimentacoes.Entities;

namespace Patrimonio.Financas.Infrastructure.Persistence;

/// <summary>
/// Representa o contexto de persistência do módulo financeiro.
/// </summary>
/// <param name="options">Opções de configuração do contexto do Entity Framework Core.</param>
internal sealed class FinancasDbContext(DbContextOptions<FinancasDbContext> options) : DbContext(options)
{
    // Coleções
    public DbSet<Cartao> Cartoes => Set<Cartao>();
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Conta> Contas => Set<Conta>();
    public DbSet<Fatura> Faturas => Set<Fatura>();
    public DbSet<Instituicao> Instituicoes => Set<Instituicao>();
    public DbSet<Lancamento> Lancamentos => Set<Lancamento>();
    public DbSet<Movimentacao> Movimentacoes => Set<Movimentacao>();

    /// <summary>
    /// Configura o modelo de dados do módulo financeiro.
    /// </summary>
    /// <param name="modelBuilder">
    /// Construtor utilizado para configurar o modelo de dados.
    /// </param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("financas");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FinancasDbContext).Assembly);
    }
}

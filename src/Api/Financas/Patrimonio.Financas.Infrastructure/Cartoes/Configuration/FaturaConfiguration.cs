using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Patrimonio.Financas.Domain.Cartoes.Entities;

namespace Patrimonio.Financas.Infrastructure.Cartoes.Configuration;

/// <summary>
/// Configura o mapeamento da entidade <see cref="Fatura"/> para o banco de dados.
/// </summary>
internal sealed class FaturaConfiguration : IEntityTypeConfiguration<Fatura>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Fatura> builder)
    {
        builder.ToTable("Faturas");

        builder.HasKey(f => f.Id);

        builder.HasOne<Cartao>()
               .WithMany()
               .HasForeignKey(f => f.CartaoId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}

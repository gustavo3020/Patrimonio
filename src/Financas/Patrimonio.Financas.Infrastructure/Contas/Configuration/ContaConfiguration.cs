using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Patrimonio.Financas.Domain.Contas.Entities;
using Patrimonio.Financas.Domain.Instituicoes.Entities;

namespace Patrimonio.Financas.Infrastructure.Contas.Configuration;

/// <summary>
/// Configura o mapeamento da entidade <see cref="Conta"/> para o banco de dados.
/// </summary>
internal sealed class ContaConfiguration : IEntityTypeConfiguration<Conta>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Conta> builder)
    {
        builder.ToTable("Contas");

        builder.HasKey(c => c.Id);

        builder.OwnsOne(c => c.Nome, nome =>
        {
            nome.Property(n => n.Valor)
                .HasColumnName("Nome")
                .HasMaxLength(200);

            nome.Property(n => n.Normalizado)
                .HasColumnName("NomeNormalizado")
                .HasMaxLength(200);

            nome.HasIndex(n => n.Normalizado)
                .IsUnique();
        });

        builder.HasOne<Instituicao>()
               .WithMany()
               .HasForeignKey(c => c.InstituicaoId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}

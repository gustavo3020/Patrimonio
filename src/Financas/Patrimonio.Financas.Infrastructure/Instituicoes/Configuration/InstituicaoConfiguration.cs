using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Patrimonio.Financas.Domain.Instituicoes.Entities;

namespace Patrimonio.Financas.Infrastructure.Instituicoes.Configuration;

/// <summary>
/// Configura o mapeamento da entidade <see cref="Instituicao"/> para o banco de dados.
/// </summary>
internal sealed class InstituicaoConfiguration : IEntityTypeConfiguration<Instituicao>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Instituicao> builder)
    {
        builder.ToTable("Instituicoes");

        builder.HasKey(i => i.Id);

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
    }
}

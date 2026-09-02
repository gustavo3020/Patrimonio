using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Patrimonio.Financas.Domain.Categorias.Entities;

namespace Patrimonio.Financas.Infrastructure.Categorias.Configuration;

/// <summary>
/// Configura o mapeamento da entidade <see cref="Categoria"/> para o banco de dados.
/// </summary>
internal sealed class CategoriaConfiguration : IEntityTypeConfiguration<Categoria>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Categoria> builder)
    {
        builder.ToTable("Categorias");

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
    }
}

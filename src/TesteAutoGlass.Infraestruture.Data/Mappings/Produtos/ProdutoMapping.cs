using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TesteAutoGlass.Produtos.Domain.Entities;

namespace TesteAutoGlass.Infraestruture.Data.Mappings.Produtos
{
    public class ProdutoMapping : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.HasIndex(c => c.Id);

            builder.Property(c => c.Codigo)
                .HasDefaultValueSql("NEXT VALUE FOR ProdutoCodigoSequencial");
            
            builder.Property(c => c.Nome)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(c => c.Ativo)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(c => c.Fabricacao)
                .IsRequired();

            builder.Property(c => c.Validade)
                .IsRequired();

            builder.HasIndex(c => new { c.Codigo }).IsUnique();

            builder.ToTable("Produto");
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VueViteCore.Business.Entities;

namespace VueViteCore.Business.Persistence.Configuration;

public class PageRegionConfiguration : IEntityTypeConfiguration<PageRegion>
{
    public void Configure(EntityTypeBuilder<PageRegion> builder)
    {
        builder.Property(p => p.Page)
            .IsRequired();
        builder.Property(p => p.Region)
            .IsRequired();
        builder.Property(p => p.Content)
            .HasMaxLength(int.MaxValue);

        builder.Property(p => p.Created)
            .IsRequired();

        builder.HasIndex(p => p.Page);
        builder.HasIndex(p => new {p.Page, p.Region})
            .IsUnique();
    }
}
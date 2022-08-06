using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VueViteCore.Business.Entities;

namespace VueViteCore.Business.Persistence.Configuration;

public class SubmissionEntryConfiguration : IEntityTypeConfiguration<SubmissionEntry>
{
    public void Configure(EntityTypeBuilder<SubmissionEntry> builder)
    {
        builder.Property(p => p.Name)
            .IsRequired();
        builder.Property(p => p.Created)
            .IsRequired();

        builder.Property(p => p.ValueOne)
            .HasMaxLength(4000);
        builder.Property(p => p.ValueTwo)
            .HasMaxLength(4000);
        builder.Property(p => p.ValueThree)
            .HasMaxLength(4000);
    }
}
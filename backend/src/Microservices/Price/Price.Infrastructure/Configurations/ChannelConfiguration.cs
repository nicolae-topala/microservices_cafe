using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Price.Infrastructure.Constants;
using Price.Domain.Entities;

namespace Price.Infrastructure.Configurations;

internal class ChannelConfiguration : IEntityTypeConfiguration<Channel>
{
    public void Configure(EntityTypeBuilder<Channel> builder)
    {
        builder.ToTable(TableNames.Channels);

        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Name)
            .HasMaxLength(256)
            .IsRequired();

        builder
            .Property(x => x.Description)
            .HasMaxLength(2000)
            .IsRequired();
    }
}
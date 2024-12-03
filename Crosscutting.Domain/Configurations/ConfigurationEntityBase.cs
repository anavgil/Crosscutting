using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crosscutting.Domain.Configurations;

public abstract class ConfigurationEntityBase<T> : IEntityTypeConfiguration<T>
    where T : class
{
    public void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(x => x.Id);

        AppendConfig(builder);
    }

    public abstract void AppendConfig(EntityTypeBuilder<T> builder);
}

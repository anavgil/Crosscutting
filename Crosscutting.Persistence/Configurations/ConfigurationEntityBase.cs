using Crosscutting.Persistence.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crosscutting.Persistence.Configurations;

public abstract class ConfigurationEntityBase<T> : IEntityTypeConfiguration<T>
    where T : EntityBase
{
    public void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(x => x.Id);

        AppendConfig(builder);
    }

    public abstract void AppendConfig(EntityTypeBuilder<T> builder);
}

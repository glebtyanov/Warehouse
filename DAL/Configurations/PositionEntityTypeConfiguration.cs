using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.EntityTypeConfigurations
{
    public class PositionEntityTypeConfiguration : IEntityTypeConfiguration<Position>
    {
        public void Configure(EntityTypeBuilder<Position> builder)
        {
            builder.HasKey(p => p.PositionId);

            builder.Property(p => p.Name)
                .HasMaxLength(50);

            builder.HasMany(p => p.Workers)
                .WithOne(w => w.Position);
        }
    }
}
